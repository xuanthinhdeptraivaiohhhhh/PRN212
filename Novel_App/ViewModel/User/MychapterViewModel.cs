using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;  // Import cho các thao tác file
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novel_App.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Windows.Input;
using Novel_App.Model;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.Windows.Navigation;
using System.Windows;
using Novel_App.View.User.MyChapter;
using System.Diagnostics;
using System.Net.Http;

namespace Novel_App.ViewModel.User
{
    public class MychapterViewModel : BaseViewModel
    {
        public ObservableCollection<Chapter> allchapters { get; set; }
        public ObservableCollection<Chapter> chapters { get; set; }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
       
        

        // Thuộc tính để binding dữ liệu từ View
        private int _novelId;
        public int NovelId
        {
            get { return _novelId; }
            set { _novelId = value; OnPropertyChanged(); }
        }

        

        private int _chapterID;
        public int ChapterID
        {
            get => _chapterID;
            set
            {
                _chapterID = value;
                OnPropertyChanged(nameof(ChapterID));
            }
        }

        private int _chapterNumber;
        public int ChapterNumber
        {
            get { return _chapterNumber; }
            set { _chapterNumber = value; OnPropertyChanged(); }
        }

        private string _chapterName;
        public string ChapterName
        {
            get { return _chapterName; }
            set { _chapterName = value; OnPropertyChanged(); }
        }

        private Novel _novel;
        private int _userID;

        public Novel Novel
        {
            get => _novel;
            set
            {
                _novel = value;
                OnPropertyChanged(nameof(Novel));
            }
        }

        private string _novelName;
        public string NovelName
        {
            get => _novelName;
            set
            {
                _novelName = value;
                OnPropertyChanged(nameof(NovelName));
            }
        }



        public int UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }



        private string _chapterContent;
        public string ChapterContent  
        {
            get { return _chapterContent; }
            set { _chapterContent = value; OnPropertyChanged(); }
        }

        //Load nội ung Chapter khi select
        private Chapter _selectedChapter;
        public Chapter SelectedChapter
        {
            get => _selectedChapter;
            set
            {
                _selectedChapter = value;
                OnPropertyChanged(nameof(SelectedChapter));

                if (_selectedChapter != null)
                {
                   
                       // MessageBox.Show($"Bạn đã chọn chương: {_selectedChapter.ChapterName}"); 
                    
                    if (!string.IsNullOrEmpty(_selectedChapter.FileUrl))
                    {
                        LoadChapterContentFromFile(_selectedChapter.FileUrl).ContinueWith(task =>
                        {
                            if (task.Exception == null)
                            {
                                ChapterContent = task.Result;  
                            }
                            else
                            {
                                ChapterContent = "Chapter content could not be loaded.";
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        ChapterContent = "This chapter has no content.";
                    }
                }
                else
                {
                    ChapterContent = "";
                }
            }
        }


        private async Task<string> LoadChapterContentFromFile(string fileUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(fileUrl);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading chapter content: {ex.Message}");
                return "Chapter content could not be loaded.";
            }
        }




        public MychapterViewModel(int novelID, string novelName, int userID)
        {
            NovelId = novelID;
            NovelName = novelName;
            UserID = userID;
            // Đọc cấu hình từ appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            LoadHighestChapter(novelID);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(update);
            DeleteCommand = new RelayCommand(Delete);


        }

        public MychapterViewModel(int chapterID,int novelID, string novelName, int userID)
        {
            ChapterID = chapterID;
            NovelId = novelID;
            NovelName = novelName;
            UserID = userID;
            // Đọc cấu hình từ appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
           
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(update);
            DeleteCommand = new RelayCommand(Delete);
            
            Task.Run(async () => await LoadChapterData(chapterID));
           


        }

        public MychapterViewModel(int userID, int novelID)
        {
            NovelId = novelID;
            UserID = userID;
            // Đọc cấu hình từ appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(update);
            DeleteCommand = new RelayCommand(Delete);
        
           
            Load(userID, novelID);

        }

        private Cloudinary _cloudinary;

        public MychapterViewModel()
        {
            // Đọc cấu hình từ appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);

            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(update);
            DeleteCommand = new RelayCommand(Delete);
         
            
        }

      
        // Thêm chapter
        private async void Add(object obj)

        {
            Application.Current.Dispatcher.Invoke(() => {
               // MessageBox.Show($"Thêm chương vào NovelID: {NovelId}");
            });
            if (string.IsNullOrEmpty(ChapterName) || string.IsNullOrEmpty(ChapterContent) || ChapterNumber <= 0 || NovelId <= 0)
            {
                System.Windows.MessageBox.Show("Please fill in the chapter information completely.");
                return;
            }

            try
            {
               
                // Tạo file tạm để lưu nội dung chương
                string tempFilePath = Path.GetTempFileName();
                File.WriteAllText(tempFilePath, ChapterContent);

                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(tempFilePath),
                    PublicId = $"Novel_{NovelId}/Chapter_{ChapterNumber}_{ChapterName.Replace(" ", "_")}",
                    Overwrite = true
                };

                // Upload file lên Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

               

                

                if (uploadResult.Error != null)
                {
                    System.Windows.MessageBox.Show($"Error uploading to Cloudinary: {uploadResult.Error.Message}");
                    return;
                }

                // Get the Cloudinary URL
                string cloudinaryUrl = uploadResult.SecureUrl.ToString();

                // Save the Cloudinary URL to the database
                using (var context = new NovelAppContext())
                {
                    var newChapter = new Chapter
                    {
                        NovelId = NovelId,
                        ChapterNumber = ChapterNumber,
                        ChapterName = ChapterName,
                        FileUrl = cloudinaryUrl, // Store the Cloudinary URL
                        ChapterStatus = "active"
                    };

                    context.Chapters.Add(newChapter);
                    // Xóa file tạm sau khi upload xong
                    File.Delete(tempFilePath);

                    context.SaveChanges();
                }

                Load(NovelId, UserID); // Refresh chapter list
                ChapterName = "";
                ChapterContent = "";
                ChapterNumber = 0;
                System.Windows.MessageBox.Show("Added chapter successfully!");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error adding chapter: {ex.Message}");
            }
        }

        private async Task<int> GetHighestChapterFromDB(int novelId)
        {
            using (var context = new NovelAppContext())
            {
                var highestChapter = await context.Chapters
                    .Where(c => c.NovelId == novelId)
                    .OrderByDescending(c => c.ChapterNumber)
                    .Select(c => c.ChapterNumber)
                    .FirstOrDefaultAsync();

                return highestChapter; // Nếu không có chương nào, mặc định trả về 0
            }
        }
        private async void LoadHighestChapter(int novelId)
        {
            int highestChapter = await GetHighestChapterFromDB(novelId);
            ChapterNumber = highestChapter + 1;
            OnPropertyChanged(nameof(ChapterNumber));
        }







        // Cập nhật

        private async void update(object obj)
        {
           
            if (string.IsNullOrEmpty(ChapterName) || string.IsNullOrEmpty(ChapterContent) || ChapterNumber <= 0 || NovelId <= 0)
            {
                System.Windows.MessageBox.Show("Please fill in the chapter information completely.");
                return;
            }



            int originalChapterNumber = await GetOriginalChapterNumber(ChapterID);
          //  System.Windows.MessageBox.Show($"Debug: ChapterNumber = {ChapterNumber}, OriginalChapterNumber = {originalChapterNumber}");

            if (ChapterNumber != originalChapterNumber)
            {
              //  System.Windows.MessageBox.Show("Vào vòng kiểm tra trùng số chương");
                bool isExists = await IsChapterNumberExists(NovelId, ChapterNumber, ChapterID);
               // System.Windows.MessageBox.Show($"Debug: IsChapterNumberExists = {isExists}");

                if (isExists)
                {
                    System.Windows.MessageBox.Show("This chapter number already exists in the novel. Please choose another number.");
                    return;
                }
            }



            try
            {
                // Generate the new public ID for Cloudinary
                string newPublicId = $"Novel_{NovelId}/Chapter_{ChapterNumber}_{ChapterName.Replace(" ", "_")}";

                // Check if the public ID has changed
                string fileUrl = await GetFileUrl(ChapterID);

                if (newPublicId != GetPublicIdFromUrl(fileUrl))
                {
                    // Delete the old file from Cloudinary (if PublicId has changed)
                    var delParams = new DelResParams()
                    {
                        PublicIds = new List<string>() { GetPublicIdFromUrl(fileUrl) },
                        Invalidate = true,
                        ResourceType = ResourceType.Raw // Specify that you're deleting a raw file
                    };

                    var delResult = await _cloudinary.DeleteResourcesAsync(delParams);

                    if (delResult.Error != null && delResult.Error.Message != "Not Found")
                    {
                        System.Windows.MessageBox.Show($"Error deleting old files on Cloudinary: {delResult.Error.Message}");
                        return;
                    }
                    // Upload the updated content to Cloudinary with the new PublicID
                    string tempFilePath = Path.GetTempFileName();
                    File.WriteAllText(tempFilePath, ChapterContent);

                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(tempFilePath),
                        PublicId = newPublicId,
                        Overwrite = true
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error != null)
                    {
                        System.Windows.MessageBox.Show($"Error uploading to Cloudinary: {uploadResult.Error.Message}");
                        return;
                    }

                    // Get the Cloudinary URL
                    string cloudinaryUrl = uploadResult.SecureUrl.ToString();

                    using (var context = new NovelAppContext())
                    {
                        var chapterToUpdate = await context.Chapters.FindAsync(ChapterID); //Find the chapter by chapterID

                        if (chapterToUpdate != null)
                        {
                            chapterToUpdate.ChapterNumber = ChapterNumber;
                            chapterToUpdate.ChapterName = ChapterName;
                            chapterToUpdate.FileUrl = cloudinaryUrl;
                            context.SaveChanges();

                        }

                    }

                    // ⚡ Làm mới dữ liệu chương sau khi cập nhật
                    await LoadChapterData(ChapterID);
                    File.Delete(tempFilePath);
                    System.Windows.MessageBox.Show("Chapter update successful!");
                }
               


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Lỗi cập nhật chương: {ex.Message}");
            }
        }

        private string GetPublicIdFromUrl(string url)
        {
            // Extract the public ID from the Cloudinary URL
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;

            // Split the path by '/' and take the last part before the extension
            string[] segments = path.Split('/');
            string lastSegment = segments.Last();

            // Remove the file extension (e.g., .txt)
            string publicId = Path.GetFileNameWithoutExtension(lastSegment);

            return publicId;
        }

        private async Task<string> GetFileUrl(int chapterId)
        {
            using (var context = new NovelAppContext())
            {
                var chapter = await context.Chapters
                    .Where(c => c.ChapterId == chapterId)
                    .Select(c => c.FileUrl)
                    .FirstOrDefaultAsync();

                return chapter ?? string.Empty;
            }
        }
       

        private async Task<int> GetOriginalChapterNumber(int chapterId)
        {
            using (var context = new NovelAppContext())
            {
                var chapter = await context.Chapters
                    .Where(c => c.ChapterId == chapterId)
                    .Select(c => c.ChapterNumber)
                    .FirstOrDefaultAsync();

                return chapter; // Trả về số chương hiện tại của chương đó
            }
        }


        private async Task<bool> IsChapterNumberExists(int novelId, int chapterNumber, int chapterId)
        {
            using (var context = new NovelAppContext())
            {
                bool exists = await context.Chapters
                    .AnyAsync(c => c.NovelId == novelId && c.ChapterNumber == chapterNumber && c.ChapterId != chapterId);

              //  System.Windows.MessageBox.Show($"Debug: NovelId={novelId}, ChapterNumber={chapterNumber}, ChapterId={chapterId}, Exists={exists}");

                return exists;
            }
        }









        private async Task LoadChapterData(int chapterId)
        {
            using (var context = new NovelAppContext())
            {
                var chapter = await context.Chapters
                    .Where(c => c.ChapterId == chapterId)
                    .FirstOrDefaultAsync();

                if (chapter != null)
                {
                    ChapterNumber = chapter.ChapterNumber;  // Số chương
                    ChapterName = chapter.ChapterName;      // Tên chương
                    ChapterContent = await LoadChapterContentFromFile(chapter.FileUrl);
                    OnPropertyChanged(nameof(ChapterNumber));
                    OnPropertyChanged(nameof(ChapterName));
                    OnPropertyChanged(nameof(ChapterContent));
                }
            }
        }






        //Danh sách chapter
        public void Load(int userID, int novelID)
        {
            using (var context = new NovelAppContext())
            {
                try
                {
                    var chaptersList = context.Chapters
                        .Include(c => c.Novel)
                        .Where(c => c.Novel.UserId == userID && c.Novel.NovelId == novelID)
                        .ToList();

                    // Gán dữ liệu mới cho ObservableCollection
                    chapters = new ObservableCollection<Chapter>(chaptersList);
                    allchapters = new ObservableCollection<Chapter>(chapters);
                    OnPropertyChanged(nameof(chapters)); // Cập nhật UI


                    Console.WriteLine($"Loaded {chapters.Count} chapters for user ID: {userID}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading chapters: {ex.Message}");
                }
            }
        }






        private async void Delete(object obj)
        {
            if (SelectedChapter == null)
            {
                MessageBox.Show("Please select the chapter to delete.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete? {SelectedChapter.ChapterName}?", "Confirm deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new NovelAppContext())
                    {
                        var chapterToDelete = await context.Chapters.FindAsync(SelectedChapter.ChapterId);

                        if (chapterToDelete != null)
                        {
                            // Xóa file trên Cloudinary nếu có
                            if (!string.IsNullOrEmpty(chapterToDelete.FileUrl))
                            {
                                var delParams = new DelResParams()
                                {
                                    PublicIds = new List<string>() { GetPublicIdFromUrl(chapterToDelete.FileUrl) },
                                    Invalidate = true,
                                    ResourceType = ResourceType.Raw
                                };

                                var delResult = await _cloudinary.DeleteResourcesAsync(delParams);
                                if (delResult.Error != null)
                                {
                                    MessageBox.Show($"Error deleting file on Cloudinary: {delResult.Error.Message}");
                                    return;
                                }
                            }

                            // Xóa chương khỏi database
                            context.Chapters.Remove(chapterToDelete);
                            await context.SaveChangesAsync();

                            // Xóa khỏi danh sách hiện tại thay vì gọi lại Load()
                            chapters.Remove(SelectedChapter);
                            allchapters.Remove(SelectedChapter);
                            OnPropertyChanged(nameof(chapters));
                        }
                    }

                    // Đặt lại SelectedChapter về null để UI cập nhật
                    SelectedChapter = null;
                    OnPropertyChanged(nameof(SelectedChapter));

                    MessageBox.Show("Chapter deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error when deleting chapter: {ex.Message}");
                }
            }
        }







    }
}
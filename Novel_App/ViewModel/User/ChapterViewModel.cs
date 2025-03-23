using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
using Novel_App.Model;
using Novel_App.Utilities;
using Novel_App.View.User;

namespace Novel_App.ViewModel.User
{
    class ChapterViewModel : BaseViewModel
    {
        public ObservableCollection<Chapter> allchapters { get; set; }
        public ObservableCollection<Chapter> Chapters { get; set; }

        // Các command xử lý
        public ICommand SearchCommand { get; }
        public ICommand ViewContentCommand { get; }
        public ICommand BackCommand { get; }

        private readonly int _novelId; // Lưu novelId để lọc chương theo tiểu thuyết

        // Hàm dựng
        public ChapterViewModel(int novelId)
        {
            _novelId = novelId; // Lưu novelId được truyền vào
            allchapters = new ObservableCollection<Chapter>();
            Chapters = new ObservableCollection<Chapter>();
            Load();
            textboxitem = new Chapter();
            SearchCommand = new RelayCommand(Search);
            ViewContentCommand = new RelayCommand(ViewContent);
            BackCommand = new RelayCommand(Back);
        }

        private Chapter _textboxitem;
        public Chapter textboxitem
        {
            get { return _textboxitem; }
            set
            {
                _textboxitem = value;
                OnPropertyChanged(nameof(textboxitem));
            }
        }

        private Chapter _selectitem;
        public Chapter selectitem
        {
            get { return _selectitem; }
            set
            {
                _selectitem = value;
                OnPropertyChanged(nameof(selectitem));

                // Cập nhật textboxitem khi chọn một item
                if (_selectitem != null)
                {
                    // Cấu hình JsonSerializerSettings để bỏ qua vòng lặp
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    _textboxitem = JsonConvert.DeserializeObject<Chapter>(JsonConvert.SerializeObject(_selectitem, settings), settings);
                    OnPropertyChanged(nameof(textboxitem));
                }
            }
        }

        private string _searchName;

        public string SearchName
        {
            get => _searchName;
            set
            {
                _searchName = value;
                OnPropertyChanged(nameof(SearchName));
                Search(null); // Gọi Search mỗi khi SearchName thay đổi
            }
        }

        private void Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchName))
            {
                Chapters = new ObservableCollection<Chapter>(allchapters);
            }
            else
            {
                var filteredChapters = allchapters
                    .Where(c => c.ChapterName != null && c.ChapterName.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Chapters = new ObservableCollection<Chapter>(filteredChapters);
            }

            OnPropertyChanged(nameof(Chapters));
        }

        private void Load()
        {
            using (var context = new NovelAppContext())
            {
                // Chỉ tải các chương thuộc về tiểu thuyết có novelId
                var chapterList = context.Chapters
                    .Where(c => c.NovelId == _novelId)
                    .ToList();
                allchapters = new ObservableCollection<Chapter>(chapterList);
                Chapters = new ObservableCollection<Chapter>(allchapters);
            }
        }

        private void ViewContent(object obj)
        {
            if (_selectitem != null)
            {
                if (!string.IsNullOrEmpty(_selectitem.FileUrl))
                {
                    // Truyền đầy đủ 3 tham số: FileUrl, ChapterNumber, ChapterName
                    var contentView = new ChapterContentView
                    {
                        DataContext = new ChapterContentViewModel(_selectitem.FileUrl, _selectitem.ChapterNumber, _selectitem.ChapterName)
                    };
                    contentView.Show();
                }
                else
                {
                    MessageBox.Show("Chapter content URL is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back(object obj)
        {
            // Đóng cửa sổ hiện tại (ChapterView)
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}
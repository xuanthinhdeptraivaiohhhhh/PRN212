using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;
using Novel_App.Model;
using Novel_App.Utilities;
using Novel_App.View.User;

namespace Novel_App.ViewModel.User
{
    class NovelViewModel : BaseViewModel
    {
        public ObservableCollection<Novel> allnovels { get; set; }
        public ObservableCollection<Novel> Novels { get; set; }

        // Các command xử lý
        public ICommand SearchCommand { get; }
        public ICommand ViewChaptersCommand { get; }

        // Hàm dựng
        public NovelViewModel()
        {
            allnovels = new ObservableCollection<Novel>();
            Novels = new ObservableCollection<Novel>();
            Load();
            textboxitem = new Novel();
            SearchCommand = new RelayCommand(Search);
            ViewChaptersCommand = new RelayCommand(ViewChapters);
        }

        private Novel _textboxitem;
        public Novel textboxitem
        {
            get { return _textboxitem; }
            set
            {
                _textboxitem = value;
                OnPropertyChanged(nameof(textboxitem));
            }
        }

        private Novel _selectitem;
        public Novel selectitem
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
                    _textboxitem = JsonConvert.DeserializeObject<Novel>(JsonConvert.SerializeObject(_selectitem, settings), settings);
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
                Novels = new ObservableCollection<Novel>(allnovels);
            }
            else
            {
                var filteredNovels = allnovels
                    .Where(s => s.NovelName.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Novels = new ObservableCollection<Novel>(filteredNovels);
            }

            OnPropertyChanged(nameof(Novels));
        }

        private void Load()
        {
            using (var context = new NovelAppContext())
            {
                var novelList = context.Novels.ToList();
                allnovels = new ObservableCollection<Novel>(novelList);
                Novels = new ObservableCollection<Novel>(allnovels);
            }
        }

        private void ViewChapters(object obj)
        {
            if (_selectitem != null)
            {
                // Mở cửa sổ ChapterView và truyền novelId
                var chapterWindow = new ChapterView(_selectitem.NovelId);
                chapterWindow.Show();
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore;
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
        public ICommand ViewMyNovelCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ToggleViewFavoritesCommand { get; }

        private readonly int _userId;
        private readonly NavigationService _navigationService;
        private bool _isViewingFavorites;
        private string _viewFavoritesButtonText;
        private string _favoriteButtonText;

        public string ViewFavoritesButtonText
        {
            get => _viewFavoritesButtonText;
            set
            {
                _viewFavoritesButtonText = value;
                OnPropertyChanged(nameof(ViewFavoritesButtonText));
            }
        }

        public string FavoriteButtonText
        {
            get => _favoriteButtonText;
            set
            {
                _favoriteButtonText = value;
                OnPropertyChanged(nameof(FavoriteButtonText));
            }
        }

        public NovelViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _userId = UserSession.Instance.Account?.UserId ?? 0;
            allnovels = new ObservableCollection<Novel>();
            Novels = new ObservableCollection<Novel>();
            _isViewingFavorites = false;
            ViewFavoritesButtonText = "View Favorites";
            Load();
            textboxitem = new Novel();
            SearchCommand = new RelayCommand(Search);
            ViewChaptersCommand = new RelayCommand(ViewChapters);
            ViewMyNovelCommand = new RelayCommand(ViewMyNovel);
            ToggleFavoriteCommand = new RelayCommand(ToggleFavorite);
            ToggleViewFavoritesCommand = new RelayCommand(ToggleViewFavorites);
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
                if (_selectitem != null)
                {
                    // Không cần sao chép bằng JsonConvert, chỉ cần gán trực tiếp
                    _textboxitem = _selectitem;
                    OnPropertyChanged(nameof(textboxitem));

                    // Cập nhật văn bản của nút Favorite
                    UpdateFavoriteButtonText();
                }
                else
                {
                    FavoriteButtonText = "Add to Favorites";
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
                Search(null);
            }
        }

        private bool IsNovelFavorite(Novel novel)
        {
            if (_userId == 0)
            {
                return false;
            }
            var favorite = novel.Favorites.FirstOrDefault(f => f.UserId == _userId);
            return favorite != null && favorite.IsFavorite == true;
        }

        private void UpdateFavoriteButtonText()
        {
            if (_selectitem != null && IsNovelFavorite(_selectitem))
            {
                FavoriteButtonText = "Remove from Favorites";
            }
            else
            {
                FavoriteButtonText = "Add to Favorites";
            }
        }

        private void Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchName))
            {
                if (_isViewingFavorites)
                {
                    Novels = new ObservableCollection<Novel>(allnovels.Where(n => IsNovelFavorite(n)));
                }
                else
                {
                    Novels = new ObservableCollection<Novel>(allnovels);
                }
            }
            else
            {
                var filteredNovels = _isViewingFavorites
                    ? allnovels.Where(n => IsNovelFavorite(n) && n.NovelName.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                    : allnovels.Where(n => n.NovelName.Contains(SearchName, StringComparison.OrdinalIgnoreCase));
                Novels = new ObservableCollection<Novel>(filteredNovels);
            }

            OnPropertyChanged(nameof(Novels));
        }

        private void Load()
        {
            using (var context = new NovelAppContext())
            {
                var novelList = context.Novels
                    .Include(n => n.Genres)
                    .Include(n => n.Favorites)
                    .ToList();

                allnovels = new ObservableCollection<Novel>(novelList);
                Novels = new ObservableCollection<Novel>(allnovels);

                if (_isViewingFavorites)
                {
                    Novels = new ObservableCollection<Novel>(allnovels.Where(n => IsNovelFavorite(n)));
                }
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

        private void ViewMyNovel(object obj)
        {
            {
                var MyNovel = new UserHomePageView();
                MyNovel.Show();
                Application.Current.Windows[0]?.Close();
            }
        }

        private void ToggleFavorite(object obj)
        {
            if (_selectitem != null)
            {
                if (_userId == 0)
                {
                    MessageBox.Show("Please log in to favorite a novel.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new NovelAppContext())
                {
                    var favorite = context.Favorites
                        .FirstOrDefault(f => f.UserId == _userId && f.NovelId == _selectitem.NovelId);

                    if (favorite == null)
                    {
                        favorite = new Favorite
                        {
                            UserId = _userId,
                            NovelId = _selectitem.NovelId,
                            IsFavorite = true
                        };
                        context.Favorites.Add(favorite);
                    }
                    else
                    {
                        favorite.IsFavorite = !favorite.IsFavorite;
                    }

                    context.SaveChanges();

                    // Tải lại selectitem để cập nhật Favorites và Genres
                    var updatedNovel = context.Novels
                        .Include(n => n.Favorites)
                        .Include(n => n.Genres)
                        .FirstOrDefault(n => n.NovelId == _selectitem.NovelId);

                    if (updatedNovel != null)
                    {
                        // Cập nhật selectitem
                        _selectitem = updatedNovel;

                        // Cập nhật mục tương ứng trong allnovels
                        var index = allnovels.IndexOf(allnovels.FirstOrDefault(n => n.NovelId == _selectitem.NovelId));
                        if (index >= 0)
                        {
                            allnovels[index] = updatedNovel;
                        }

                        // Cập nhật giao diện
                        OnPropertyChanged(nameof(selectitem));
                        UpdateFavoriteButtonText();

                        // Cập nhật danh sách Novels
                        Search(null);
                    }
                }
            }
        }

        private void ToggleViewFavorites(object obj)
        {
            if (_userId == 0)
            {
                MessageBox.Show("Please log in to view your favorite novels.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _isViewingFavorites = !_isViewingFavorites;
            ViewFavoritesButtonText = _isViewingFavorites ? "View All Novels" : "View Favorites";
            Search(null);
        }
    }
}
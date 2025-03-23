using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Novel_App.Utilities;
using System.Windows.Input;
using System.Windows;
using Novel_App.Model;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Novel_App.ViewModel.User
{
    public class MyNovelViewModel : BaseViewModel
    {
        public ObservableCollection<Novel> All { get; set; }
        public ObservableCollection<Novel> NovelList { get; set; }
        public ObservableCollection<String> Genrename { get; set; }



        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ViewChapterCommand { get; }
        public ICommand ViewDetailCommand { get; }
        public ICommand ClearCommand { get; }


        public MyNovelViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            ViewChapterCommand = new RelayCommand(ViewChapter);
            ViewDetailCommand = new RelayCommand(ViewDetail);
            ClearCommand = new RelayCommand(Clear);
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

        private Novel _selecteditem;
        public Novel selecteditem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                OnPropertyChanged(nameof(selecteditem));

                if (_selecteditem != null)
                {
                    _textboxitem = JsonConvert.DeserializeObject<Novel>(JsonConvert.SerializeObject(_selecteditem));
                    OnPropertyChanged(nameof(textboxitem));

                    int GenreID = textboxitem.Genres.FirstOrDefault()?.GenreId ?? 0;
                    _selectedGenre = GenreList.FirstOrDefault(g => g.GenreId == GenreID);
                    OnPropertyChanged(nameof(selectedGenre));
                }
            }
        }

        private String _searchitem;
        public String searchitem
        {
            get { return _searchitem; }
            set
            {
                _searchitem = value;
                OnPropertyChanged(nameof(searchitem));
            }
        }

        private Genre _selectedGenre;
        public Genre selectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                OnPropertyChanged(nameof(selectedGenre));
            }
        }
        private ObservableCollection<Genre> _genreList;
        public ObservableCollection<Genre> GenreList
        {
            get { return _genreList; }
            set
            {
                _genreList = value;
                OnPropertyChanged(nameof(GenreList));

                if (selectedGenre == null)
                {
                    selectedGenre = _genreList[0];
                }
            }
        }
        private void Clear(object obj)
        {
            textboxitem = new Novel();
            searchitem = "";
            selectedGenre = _genreList[0];
            NovelList = new ObservableCollection<Novel>(All);
            OnPropertyChanged(nameof(NovelList));
        }

        private void ViewChapter(object obj)
        {
            if (selecteditem != null)
            {

            }
        }

        private Boolean _isListVisible;
        public Boolean IsListVisible
        {
            get { return _isListVisible; }
            set
            {
                _isListVisible = value;
                OnPropertyChanged(nameof(IsListVisible));
            }
        }

        private void ViewDetail(object obj)
        {
            if (selecteditem != null)
            {
                IsListVisible = !IsListVisible;
            }
        }

        private void Load()
        {
            using (var context = new NovelAppContext())
            {
                var list = context.Novels
                    .Include(n => n.Genres)
                    .ToList();
                NovelList = new ObservableCollection<Novel>(list);
            }
            All = new ObservableCollection<Novel>(NovelList);

            using (var context = new NovelAppContext())
            {
                var list = context.Genres.ToList();
                GenreList = new ObservableCollection<Genre>(list);
                Genrename = new ObservableCollection<String>(list.Select(g => g.GenreName).ToList());
            }
            textboxitem = new Novel();
            OnPropertyChanged(nameof(textboxitem));

        }

        public Boolean Check()
        {
            return !String.IsNullOrWhiteSpace(_textboxitem.NovelName) &&
                !String.IsNullOrWhiteSpace(_textboxitem.NovelDescription) &&
                (int.TryParse(_textboxitem.TotalChapter?.ToString(), out int number) && number > 0);

        }

        private void Add(object obj)
        {
            if (Check())
            {
                var newitem = new Novel()
                {
                    NovelName = _textboxitem.NovelName,
                    NovelDescription = _textboxitem.NovelDescription,
                    TotalChapter = _textboxitem.TotalChapter,
                    UserId = UserSession.Instance.Account.UserId,
                };

                using (var context = new NovelAppContext())
                {
                    //var selectedGenre = new ObservableCollection<String>(textboxitem.Genres.Select(g => g.GenreName).ToList());
                    newitem.Genres = context.Genres
                        .Where(g => selectedGenre.Equals(g))
                        .ToList();
                    context.Novels.Add(newitem);
                    context.SaveChanges();
                }

                All.Add(newitem);
                NovelList = new ObservableCollection<Novel>(All);
                OnPropertyChanged(nameof(NovelList));
                _textboxitem = new Novel();
                OnPropertyChanged(nameof(textboxitem));
            } else
            {
                MessageBox.Show("Invalid input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Update(object obj)
        {
            if (_selecteditem != null && Check())
            {
                using (var context = new NovelAppContext())
                {
                    //var existNovel = context.Novels
                    //    .Include(n => n.Genres)
                    //    .FirstOrDefault(n => n.NovelId == textboxitem.NovelId);
                    //existNovel.Genres.Clear();

                    //textboxitem.Genres = context.Genres
                    //.Where(g => selectedGenre.Equals(g))
                    //.ToList();
                    //context.Novels.Update(existNovel);
                    //context.SaveChanges();

                    var existNovel = context.Novels
                        .Include(n => n.Genres)
                        .FirstOrDefault(n => n.NovelId == textboxitem.NovelId);

                    existNovel.Genres = context.Genres
                    .Where(g => selectedGenre.Equals(g))
                    .ToList();
                    existNovel.NovelName = textboxitem.NovelName;
                    existNovel.TotalChapter = textboxitem.TotalChapter;
                    existNovel.NovelDescription = textboxitem.NovelDescription;
                    textboxitem.Genres = existNovel.Genres;

                    context.Novels.Update(existNovel);
                    context.SaveChanges();
                }

                int index = All.IndexOf(selecteditem);
                if (index >= 0)
                {
                    All[index] = textboxitem;
                }
                NovelList = new ObservableCollection<Novel>(All);
                OnPropertyChanged(nameof(NovelList));
                _textboxitem = new Novel();
                OnPropertyChanged(nameof(textboxitem));
            } else
            {
                if (selecteditem == null) MessageBox.Show("Please select a novel", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else MessageBox.Show("Invalid input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete(object obj)
        {
            if (_selecteditem != null)
            {
                using (var context = new NovelAppContext())
                {
                    var existNovel = context.Novels
                        .Include(n => n.Genres)
                        .FirstOrDefault(n => n.NovelId == textboxitem.NovelId);
                    existNovel.Genres.Clear();

                    context.Novels.Remove(existNovel);
                    context.SaveChanges();
                }

                All.Remove(_selecteditem);
                NovelList = new ObservableCollection<Novel>(All);
                OnPropertyChanged(nameof(NovelList));
                _textboxitem = new Novel();
                OnPropertyChanged(nameof(textboxitem));
            }
        }

        private void Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(_searchitem))
            {
                NovelList = new ObservableCollection<Novel>(All);
            }
            else
            {
                var result = All.Where(a => a.NovelName.ToLower().Contains(_searchitem.ToLower())).ToList();
                NovelList.Clear();
                result.ForEach(a => NovelList.Add(a));

            }
            OnPropertyChanged(nameof(NovelList));
        }

    }
}

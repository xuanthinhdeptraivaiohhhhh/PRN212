using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Novel_App.Model;
using Novel_App.Utilities;

using Microsoft.EntityFrameworkCore;


namespace Novel_App.ViewModel.Staff
{
    public class GenreViewModel : BaseViewModel
    {
        private readonly NovelAppContext _context = new NovelAppContext();

        public ObservableCollection<Genre> Genres { get; set; }

        private Genre _selectedGenre = new Genre(); 

        private Genre _selectedGenre = new Genre(); // ✅ Fix lỗi CS8618


        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged(nameof(SelectedGenre));
            }
        }

        public GenreViewModel()
        {
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());

            AddCommand = new RelayCommand(_ => AddGenre());
            EditCommand = new RelayCommand(_ => EditGenre(), _ => SelectedGenre != null);
            DeleteCommand = new RelayCommand(_ => DeleteGenre(), _ => SelectedGenre != null);
            RefreshCommand = new RelayCommand(_ => RefreshData());
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        private void AddGenre()
        {
            if (SelectedGenre == null || string.IsNullOrWhiteSpace(SelectedGenre.GenreName))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại trước khi thêm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newGenre = new Genre { GenreName = SelectedGenre.GenreName };
            _context.Genres.Add(newGenre);
            _context.SaveChanges();

            Genres.Add(newGenre);
            SelectedGenre = new Genre(); // Reset sau khi thêm

            CommandManager.InvalidateRequerySuggested(); // 🔄 Cập nhật lệnh
        }


        private void EditGenre()
        {
            if (SelectedGenre == null) return;

            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == SelectedGenre.GenreId);
            if (genre != null)
            {
                genre.GenreName = SelectedGenre.GenreName;
                _context.SaveChanges();

                // Cập nhật danh sách hiển thị
                RefreshData();
            }
        }

        private void DeleteGenre()
        {
            if (SelectedGenre == null) return;

<<<<<<< HEAD
            var genre = _context.Genres
           .Include(g => g.Novels)
          .FirstOrDefault(g => g.GenreId == SelectedGenre.GenreId);
            genre.Novels.Clear();
=======
            var genre = _context.Genres.FirstOrDefault(g => g.GenreId == SelectedGenre.GenreId);
>>>>>>> parent of 9a5f11b (Revert "View all Genre , add , edit ,delete")
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                Genres.Remove(genre);

                SelectedGenre = new Genre(); // Reset SelectedGenre sau khi xóa
                CommandManager.InvalidateRequerySuggested(); // 🔄 Cập nhật UI
            }
        }


        private void RefreshData()
        {
            Genres.Clear();
            foreach (var genre in _context.Genres.ToList())
            {
                Genres.Add(genre);
            }
        }
    }
}
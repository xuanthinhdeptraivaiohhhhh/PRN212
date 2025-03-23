using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Novel_App.Utilities;

namespace Novel_App.ViewModel.User
{
    public class ChapterContentViewModel : BaseViewModel
    {
        private string _chapterContent;
        public string ChapterContent
        {
            get => _chapterContent;
            set
            {
                _chapterContent = value;
                OnPropertyChanged(nameof(ChapterContent));
            }
        }

        private int _chapterNumber;
        public int ChapterNumber
        {
            get => _chapterNumber;
            set
            {
                _chapterNumber = value;
                OnPropertyChanged(nameof(ChapterNumber));
                UpdateChapterTitle();
            }
        }

        private string _chapterName;
        public string ChapterName
        {
            get => _chapterName;
            set
            {
                _chapterName = value;
                OnPropertyChanged(nameof(ChapterName));
                UpdateChapterTitle();
            }
        }

        private string _chapterTitle;
        public string ChapterTitle
        {
            get => _chapterTitle;
            set
            {
                _chapterTitle = value;
                OnPropertyChanged(nameof(ChapterTitle));
            }
        }

        public ICommand BackCommand { get; }

        private readonly string _contentUrl;

        public ChapterContentViewModel(string contentUrl, int chapterNumber, string chapterName)
        {
            _contentUrl = contentUrl;
            ChapterNumber = chapterNumber;
            ChapterName = chapterName;
            BackCommand = new RelayCommand(Back);
            UpdateChapterTitle();
            LoadContent();
        }

        private void UpdateChapterTitle()
        {
            ChapterTitle = $"Chapter {ChapterNumber}: {(string.IsNullOrEmpty(ChapterName) ? "Untitled" : ChapterName)}";
        }

        private async void LoadContent()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string content = await client.GetStringAsync(_contentUrl);
                    ChapterContent = content;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chapter content: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ChapterContent = "Failed to load content.";
            }
        }

        private void Back(object obj)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}
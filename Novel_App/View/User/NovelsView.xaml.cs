using System.Windows.Controls;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User
{
    public partial class NovelsView : Page
    {
        private readonly NovelViewModel _viewModel;

        public NovelsView()
        {
            InitializeComponent();
            _viewModel = new NovelViewModel(this.NavigationService);
            DataContext = _viewModel;
        }
    }
}
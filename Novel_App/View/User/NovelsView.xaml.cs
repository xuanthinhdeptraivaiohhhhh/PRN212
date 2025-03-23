using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User
{
    /// <summary>
    /// Interaction logic for NovelsView.xaml
    /// </summary>
    public partial class NovelsView : Page
    {
        private readonly NovelViewModel _viewModel;
        public NovelsView()
        {
            InitializeComponent();
            _viewModel = new NovelViewModel();
            DataContext = _viewModel;
        }
    }
}

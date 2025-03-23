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
using System.Windows.Shapes;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User
{
    public partial class NovelView : Window
    {
        private readonly NovelViewModel _viewModel;

        public NovelView()
        {
            InitializeComponent();
            _viewModel = new NovelViewModel();
            DataContext = _viewModel;
        }
    }
}

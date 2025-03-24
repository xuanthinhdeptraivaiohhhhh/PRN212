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

namespace Novel_App.ViewModel.User
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private readonly RegisterViewModel _viewModel;
        public Register()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel();
            DataContext = _viewModel;
        }
        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.ConfirmPassword = ((PasswordBox)sender).Password;
        }
    }
}

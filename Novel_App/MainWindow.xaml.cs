using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Novel_App.View.Staff;
using Novel_App.ViewModel.Staff;
using Novel_App.View;
using Novel_App.ViewModel.User;
using Novel_App.View.User;


namespace Novel_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GenreViewModel();

        }
        private void ViewGenres_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new GenreView());
        }
        
    }
}
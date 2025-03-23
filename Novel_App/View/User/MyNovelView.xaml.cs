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
using Novel_App.Model;
using Novel_App.View.User.MyChapter;
using Novel_App.ViewModel;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User
{
    /// <summary>
    /// Interaction logic for MyNovelView.xaml
    /// </summary>
    public partial class MyNovelView : Page
    {
        public Novel selecteditem {  get; set; }
        public MyNovelView()
        {
            InitializeComponent();
            DataContext = new MyNovelViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selecteditem = (Novel)ListView.SelectedItem;
            if (selecteditem != null)
            {
                NavigationService?.Navigate(new MyListChapterView(selecteditem.NovelId));
            }
        }
    }
}
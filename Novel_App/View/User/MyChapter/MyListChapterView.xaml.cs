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
using Novel_App.Utilities;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User.MyChapter
{
    /// <summary>
    /// Interaction logic for MyListChapterView.xaml
    /// </summary>
    public partial class MyListChapterView : Page
    {
        public Chapter SelectedChapter { get; set; }
        public MyListChapterView(int novelId)
        {
            InitializeComponent();
            int userID = UserSession.Instance.Account.UserId;
            int novelID = novelId;
            DataContext = new MychapterViewModel(userID, novelID);
        }


        private void AddChapter_Click(object sender, RoutedEventArgs e)
        {
            SelectedChapter = (Chapter)listView_Chapters.SelectedItem;

            if (SelectedChapter == null)
            {
                MessageBox.Show("Please select a chapter before adding!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

          
            int userID = UserSession.Instance.Account.UserId; // Lấy userID từ nguồn phù hợp, nếu có hệ thống đăng nhập, lấy từ UserSession
            NavigationService?.Navigate(new MychapterView(SelectedChapter.Novel.NovelId, SelectedChapter.Novel.NovelName, userID));
            
        }



        private void UpdateChapter_Click(object sender, RoutedEventArgs e)
        {
            SelectedChapter = (Chapter)listView_Chapters.SelectedItem;

            if (SelectedChapter == null)
            {
                MessageBox.Show("Please select a chapter before adding!", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            int userID = UserSession.Instance.Account.UserId; // Lấy userID từ nguồn phù hợp, nếu có hệ thống đăng nhập, lấy từ UserSession
            NavigationService?.Navigate(new UpdateMyChapterView(SelectedChapter.ChapterId, SelectedChapter.Novel.NovelId, SelectedChapter.Novel.NovelName, userID));
        }
    }
}

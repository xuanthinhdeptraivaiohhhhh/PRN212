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

namespace Novel_App.View.User.MyChapter
{
    /// <summary>
    /// Interaction logic for UpdateMyChapterView.xaml
    /// </summary>
    public partial class UpdateMyChapterView : Page
    {
        public UpdateMyChapterView(int chapterID, int novelID, string novelName, int userID)
        {
           
        
            InitializeComponent();
            DataContext = new MychapterViewModel(chapterID, novelID, novelName, userID); // Truyền dữ liệu vào ViewModel
        
    }
    }
}

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
using Novel_App.Model;
using Novel_App.ViewModel.User;

namespace Novel_App.View.User
{
    /// <summary>
    /// Interaction logic for ChapterView.xaml
    /// </summary>
    public partial class ChapterView : Window
    {
        public ChapterView(int novelId)
        {
            InitializeComponent();
            DataContext = new ChapterViewModel(novelId);
        }
    }
}

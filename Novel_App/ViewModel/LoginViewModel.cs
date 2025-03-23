using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novel_App.Utilities;
using System.Windows.Input;
using System.Windows;
using Novel_App.Model;
using Novel_App.View.User;
using Novel_App.View.Admin;

namespace Novel_App.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object obj)
        {
            using (var context = new NovelAppContext())
            {
                if (isadmin)
                {
                    var account = context.ManagerAccounts.FirstOrDefault(a => a.Username == _username && a.Password == _password);
                    if (account != null)
                    {
                        AdminSession.Instance.SetUser(account);

                        var View = new AdminDashboardView();
                        View.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var account = context.UserAccounts.FirstOrDefault(a => a.UserName == _username && a.Password == _password);
                    if (account != null)
                    {
                        UserSession.Instance.SetUser(account);

                        var UserView = new UserHomePageView();
                        UserView.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private String _username;
        public String username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(username));
            }
        }

        private String _password;
        public String password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(password));
            }
        }

        private Boolean _isadmin;
        public Boolean isadmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                OnPropertyChanged(nameof(isadmin));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novel_App.Model;

namespace Novel_App.Utilities
{
    public class UserSession
    {
        private static UserSession _instance;
        private static readonly object _lock = new object();

        public UserAccount Account { get; set; }

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserSession();
                    }
                    return _instance;
                }
            }
        }

        public void SetUser(UserAccount account)
        {
            Account = account;
        }

        public void Logout()
        {
            Account = null!;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novel_App.Model;

namespace Novel_App.Utilities
{
    public class AdminSession
    {
        private static AdminSession _instance;
        private static readonly object _lock = new object();

        public ManagerAccount Account { get; set; }

        private AdminSession() { }

        public static AdminSession Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AdminSession();
                    }
                    return _instance;
                }
            }
        }

        public void SetUser(ManagerAccount account)
        {
            Account = account;
        }

        public void Logout()
        {
            Account = null!;
        }
    }
}

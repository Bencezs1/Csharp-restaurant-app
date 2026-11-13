using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class UserSession : IUserSession
    {
        public SessionUser? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;


        public void SignIn(SessionUser user)
        {
            CurrentUser = user;
        }

        public void SignOut()
        {
            CurrentUser = null;
        }
    }
}

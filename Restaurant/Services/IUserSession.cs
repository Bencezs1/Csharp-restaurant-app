using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IUserSession
    {
        User? CurrentUser { get; }
        bool IsLoggedIn { get; }

        void SignIn(User user);
        void SignOut();

    }
}

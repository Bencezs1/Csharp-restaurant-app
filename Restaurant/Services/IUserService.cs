using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IUserService
    {
        Task<User?> LoginUserAsync(string username, string password);

        Task<bool> RegisterUserAsync(
            string firstName,
            string lastName,
            string username,
            string email,
            string passwordm);

        Task<User> UpdateUserAsync(User updated);

        Task<bool> DeleteUserAsync(User user);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> EmailExistsAsync(string email);
    }
}

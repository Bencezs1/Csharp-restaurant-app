using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginUserAsync(string username, string password)
        {
            ValidateLogin(username, password);

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public async Task<User> UpdateUserAsync(User updated)
        {
            try
            {
                _context.Users.Update(updated);
                await _context.SaveChangesAsync();
                return updated;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Could not update user in database.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Could not delete user from database.", ex);
            }
        }


        public async Task<bool> RegisterUserAsync(
                string firstName,
                string lastName,
                string username,
                string email,
                string password)
        {
            ValidateRegistration(
                firstName,
                lastName,
                username,
                email,
                password);


            if (await UsernameExistsAsync(username))
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }


            if (await EmailExistsAsync(email))
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            var cart = new Cart();

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
                Password = PasswordHasher.HashPassword(password)
            };

            user.Cart = cart;


            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(
                    "Could not save user to database.", ex);
             
            }
        }


        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(x => x.Username == username);
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(x => x.Email == email);
        }



        private static void ValidateLogin(
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(
                    "Username is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Password is required.");
        }


        private static void ValidateRegistration(
            string firstName,
            string lastName,
            string username,
            string email,
            string password)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException(
                    "First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException(
                    "Last name is required.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(
                    "Username is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Email is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Password is required.");
        }
    }
}

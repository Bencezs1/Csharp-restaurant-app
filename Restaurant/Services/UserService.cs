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
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public UserService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<SessionUser?> LoginUserAsync(string username, string password)
        {
            ValidateLogin(username, password);

            await using var context = await _contextFactory.CreateDbContextAsync();

            // The hash is read only to verify the password here; it never leaves this method.
            var user = await context.Users
                .Where(x => x.Username == username)
                .Select(x => new
                {
                    x.Id,
                    x.Username,
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    x.Password,
                    CartId = (int?)x.Cart.Id
                })
                .FirstOrDefaultAsync();

            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                return null;

            // Every user should have a cart; create one if the row is missing.
            var cartId = user.CartId ?? await CreateCartAsync(context, user.Id);

            return new SessionUser(
                user.Id,
                cartId,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Email);
        }

        private static async Task<int> CreateCartAsync(ApplicationDbContext context, int userId)
        {
            var cart = new Cart { UserId = userId };

            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            return cart.Id;
        }

        public async Task<User> UpdateUserAsync(User updated)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                context.Users.Update(updated);
                await context.SaveChangesAsync();
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
                await using var context = await _contextFactory.CreateDbContextAsync();

                context.Users.Remove(user);
                await context.SaveChangesAsync();

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
                await using var context = await _contextFactory.CreateDbContextAsync();

                context.Users.Add(user);
                await context.SaveChangesAsync();

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
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Users
                .AnyAsync(x => x.Username == username);
        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Users
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

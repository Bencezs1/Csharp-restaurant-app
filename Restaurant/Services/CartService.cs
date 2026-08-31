using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCart(User user)
        {
            try
            {
                _context.Carts.Add(new Cart
                {
                    UserId = user.Id,
                    User = user,
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(
                    "Could not save user to database.", ex);
            }
        }

        public async Task<Cart?> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts.Include(x=>x.Items).Include(x=>x.User).FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                return null;
            }
            return cart;
        }
    }
}

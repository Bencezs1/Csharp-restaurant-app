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
    public class CartItemService : ICartItemService
    {
        private readonly ApplicationDbContext _context;

        public CartItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemToCart(CartItem item, int userId)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                return false;
            }

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.MealId == item.MealId);

            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                item.CartId = cart.Id;
                item.Cart = cart;

                cart.Items.Add(item);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemFromCart(CartItem item, int userId)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
                return false;


            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}

using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ApplicationDbContext _context;

        public CartItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemToCart(
            CartItem item,
            int cartId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
                return false;

            var existing = cart.Items.FirstOrDefault(x => x.MealId == item.MealId);

            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                item.CartId = cart.Id;
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemFromCart(
            CartItem item,
            int cartId)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart == null)
                return false;

            cart.Items.Remove(item);

            await _context.SaveChangesAsync();  

            return true;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CartItemService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> AddItemToCart(
            int cartId,
            int mealId,
            int quantity)
        {
            if (quantity <= 0)
                return false;

            await using var context = await _contextFactory.CreateDbContextAsync();

            if (!await context.Carts.AnyAsync(x => x.Id == cartId))
                return false;

            // Ask the database whether this meal is already in the cart,
            // instead of relying on whatever happens to be loaded in memory.
            var existing = await context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.MealId == mealId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                context.CartItems.Add(new CartItem
                {
                    CartId = cartId,
                    MealId = mealId,
                    Quantity = quantity
                });
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemFromCart(
            int cartItemId,
            int cartId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // The CartId filter makes sure the item really belongs to this cart.
            var deleted = await context.CartItems
                .Where(x => x.Id == cartItemId && x.CartId == cartId)
                .ExecuteDeleteAsync();

            return deleted > 0;
        }
    }
}

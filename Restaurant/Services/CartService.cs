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
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CartService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Cart?> GetCartByUserId(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // Meal is needed for Cart.Price and for the name column in the cart grid.
            // The User is not loaded: nothing uses it, and it carries the password hash.
            return await context.Carts
                .AsNoTracking()
                .Include(x => x.Items)
                    .ThenInclude(x => x.Meal)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}

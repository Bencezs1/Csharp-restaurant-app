using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface ICartItemService
    {
        Task<bool> AddItemToCart(CartItem item, int userId);
        Task<bool> DeleteItemFromCart(CartItem item, int userId);
    }
}

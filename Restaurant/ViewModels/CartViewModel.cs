using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class CartViewModel : ViewModelBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly SessionUser _user;

        [ObservableProperty]
        private ObservableCollection<CartItem> _cart = new ObservableCollection<CartItem>();

        [ObservableProperty]
        private int _price;

        public CartViewModel(ICartItemService cartItemService, IUserSession session, ICartService cartService)
        {
            _user = session.CurrentUser
                ?? throw new InvalidOperationException("No user is signed in.");
            _cartItemService = cartItemService;
            _cartService = cartService;

            // Load without blocking the UI thread; errors are handled inside.
            _ = LoadCartAsync();
        }

        private async Task LoadCartAsync()
        {
            try
            {
                var cart = await _cartService.GetCartByUserId(_user.Id);

                if (cart == null)
                {
                    Cart = new ObservableCollection<CartItem>();
                    Price = 0;
                    return;
                }

                Cart = new ObservableCollection<CartItem>(cart.Items);
                Price = cart.Price;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Could not load the cart.");
            }
        }

        [RelayCommand]
        private async Task RemoveFromCart(CartItem item)
        {
            var success = await _cartItemService.DeleteItemFromCart(item.Id, _user.CartId);

            if (success)
            {

                MessageBox.Show("Good");
                await LoadCartAsync();
            }
            else
            {
                MessageBox.Show("Bad");
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class CartViewModel : ViewModelBase
    {
        public ICartService CartService { get; set; }
        public ICartItemService CartItemService { get; set; }
        public IUserSession Session { get; set; }
        public User CurrentUser { get; set; }

        [ObservableProperty]
        private ObservableCollection<CartItem> _cart;

        [ObservableProperty]
        private int _price;
        public CartViewModel(ICartItemService cartItemService, IUserSession session, ICartService cartService)
        {
            CartItemService = cartItemService;
            Session = session;
            CartService = cartService;
            CurrentUser = Session.CurrentUser;

            _ = LoadCartAsync();
        }

        private async Task LoadCartAsync()
        {
            var cart = await CartService.GetCartByUserId(CurrentUser.Id);
            if(cart != null)
            {
                Cart = new ObservableCollection<CartItem>(cart.Items);
                Price = cart.Price;
            }
            
        }

        [RelayCommand]
        private async Task RemoveFromCart(CartItem item)
        {
            var success = await CartItemService.DeleteItemFromCart(item, CurrentUser.Id);

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

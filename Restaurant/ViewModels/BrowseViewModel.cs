using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class BrowseViewModel : ViewModelBase
    {
        public IUserSession Session { get; set; }
        public ICartItemService CartItemService { get; set; }
        public IMealService MealService { get; set; }
        public ICartService CartService { get; set; }

        public ObservableCollection<Meal> Meals { get; set; }

        [ObservableProperty]
        private User _currentUser;


        private int quantity = 5;

        public BrowseViewModel(IUserSession session, ICartItemService cartItemService, IMealService mealService, ICartService cartService)
        {
            Session = session;
            CartItemService = cartItemService;
            CurrentUser = Session.CurrentUser;
            MealService = mealService;
            CartService = cartService;
            Meals = new ObservableCollection<Meal>(MealService.GetAll());

        }

        [RelayCommand]
        private async Task AddToCart(Meal meal)
        {
            var item = new CartItem
            {
                Meal = meal,
                MealId = meal.Id,
                Quantity = quantity
            };

            var success = await CartItemService.AddItemToCart(item, CurrentUser.Id);

            if (success)
            {

                MessageBox.Show("Good");
            }
            else
            {
                MessageBox.Show("Bad");
            }
        }


    }
}

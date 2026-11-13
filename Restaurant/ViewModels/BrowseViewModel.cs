using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class BrowseViewModel : ViewModelBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly IMealService _mealService;
        private readonly SessionUser _user;

        public ObservableCollection<Meal> Meals { get; } = new ObservableCollection<Meal>();

        private int quantity = 5;

        public BrowseViewModel(IUserSession session, ICartItemService cartItemService, IMealService mealService)
        {
            _user = session.CurrentUser
                ?? throw new InvalidOperationException("No user is signed in.");
            _cartItemService = cartItemService;
            _mealService = mealService;

            // Load without blocking the UI thread; errors are handled inside.
            _ = LoadMealsAsync();
        }

        private async Task LoadMealsAsync()
        {
            try
            {
                var meals = await _mealService.GetAllAsync();

                Meals.Clear();
                foreach (var meal in meals)
                {
                    Meals.Add(meal);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Could not load the menu.");
            }
        }

        [RelayCommand]
        private async Task AddToCart(Meal meal)
        {
            var success = await _cartItemService.AddItemToCart(_user.CartId, meal.Id, quantity);

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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private IHomeNavigationService _homeNavigation;

        public HomeViewModel(IHomeNavigationService homeNavigation)
        {
            HomeNavigation = homeNavigation;
            HomeNavigation.NavigateTo<BrowseViewModel>(); 
        }

        [RelayCommand]
        private void NavigateToBrowse() => HomeNavigation.NavigateTo<BrowseViewModel>();

        [RelayCommand]
        private void NavigateToCart() => HomeNavigation.NavigateTo<CartViewModel>();


    }
}

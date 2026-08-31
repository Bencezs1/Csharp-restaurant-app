using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Restaurant.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        [ObservableProperty]
        private INavigationService _navigation;

        public MainWindowViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            Navigation.NavigateTo<LoginViewModel>();
        }

        [RelayCommand]
        private void NavigateToHome() => Navigation.NavigateTo<HomeViewModel>();
        [RelayCommand]
        private void NavigateToLogin() => Navigation.NavigateTo<LoginViewModel>();
    }
}

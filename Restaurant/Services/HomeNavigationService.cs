using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public partial class HomeNavigationService : ObservableObject, IHomeNavigationService
    {
        [ObservableProperty]
        private ViewModelBase _currentView;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        public HomeNavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<T>() where T : ViewModelBase
        {
            CurrentView = _viewModelFactory.Invoke(typeof(T));
        }
    }
}

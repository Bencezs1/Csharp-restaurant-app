using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _firstName = string.Empty;
        [ObservableProperty]
        private string _lastName = string.Empty;
        [ObservableProperty]
        [Required(ErrorMessage = "Username is required")]
        [NotifyCanExecuteChangedFor(nameof(NavigatetToLoginCommand))]
        private string _username = string.Empty;
        [ObservableProperty]
        [Required(ErrorMessage = "Email is required")]
        [NotifyCanExecuteChangedFor(nameof(NavigatetToLoginCommand))]
        private string _email = string.Empty;
        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        [NotifyCanExecuteChangedFor(nameof(NavigatetToLoginCommand))]
        private string _password = string.Empty;

        [ObservableProperty]
        private INavigationService _navigationService;

        public IUserService UserService { get; set; }

        public RegisterViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            UserService = userService;
        }

        [RelayCommand(CanExecute = nameof(CanValidate))]
        private async Task NavigatetToLogin()
        {
            try
            {
                var succes = await UserService.RegisterUserAsync(FirstName, LastName, Username, Email, Password);

                if(succes)
                {
                    NavigationService.NavigateTo<LoginViewModel>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanValidate()
        {
            ValidateAllProperties();
            return !HasErrors;
        }
    }
}

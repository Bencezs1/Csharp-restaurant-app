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
    public partial class LoginViewModel : ViewModelBase
    {



        [ObservableProperty]
        [Required(ErrorMessage = "Username is required")]
        [NotifyCanExecuteChangedFor(nameof(NavigateToHomeCommand))]
        private string _username = String.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        [NotifyCanExecuteChangedFor(nameof(NavigateToHomeCommand))]
        private string _password = String.Empty;


        [ObservableProperty]
        private INavigationService _navigationService;

        public IUserService UserService { get; set; }
        public IUserSession Session { get; set; }

        public LoginViewModel(INavigationService navigationService, IUserService userService, IUserSession session)
        {
            NavigationService = navigationService;
            UserService = userService;
            Session = session;
        }

        [RelayCommand(CanExecute = nameof(CanValidate))]
        private async Task NavigateToHome() 
        {
            try
            {
                var user = await UserService.LoginUserAsync(Username, Password);
                if (user == null)
                {
                    throw new Exception("Username or password is incorrect");
                }
                Session.SignIn(user);
                NavigationService.NavigateTo<HomeViewModel>(); 
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

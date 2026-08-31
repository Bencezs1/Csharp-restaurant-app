using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Data;
using Restaurant.Services;
using Restaurant.ViewModels;
using Restaurant.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Restaurant
{
        public partial class App : Application
        {
            private readonly ServiceProvider _serviceProvider;

            public App()
            {
                IServiceCollection services = new ServiceCollection();

                string connectionString ="Server=localhost;Database=resta;Uid=root;Pwd=;";

                services.AddDbContextFactory<ApplicationDbContext>(options =>
                {
                    options.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString)
                    );
                });

            services.AddScoped<IUserService, UserService>();

                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IHomeNavigationService, HomeNavigationService>();
                services.AddSingleton<IUserSession, UserSession>();
                services.AddSingleton<ICartService, CartService>();
                services.AddSingleton<ICartItemService, CartItemService>();
                services.AddSingleton<IMealService, MealService>();


                services.AddSingleton<MainWindowViewModel>();
                services.AddTransient<LoginViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<BrowseViewModel>();
                services.AddTransient<CartViewModel>();


                services.AddSingleton<MainWindow>(provider =>
                new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>()
                });


                services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
                    viewModelType =>
                        (ViewModelBase)provider.GetRequiredService(viewModelType)
                );


                _serviceProvider = services.BuildServiceProvider();
            }


            protected override void OnStartup(StartupEventArgs e)
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

                mainWindow.Show();

                base.OnStartup(e);
            }
        }

}

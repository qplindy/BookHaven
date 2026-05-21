using BookHaven.Views;
using System.Windows;
using System.Windows.Navigation;

namespace BookHaven
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new NavigationWindow();
            window.Title = "BookHaven";
            window.Width = 480;
            window.Height = 560;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowsNavigationUI = false;
            window.Navigate(new AuthPage());
            window.Show();
        }
    }
}
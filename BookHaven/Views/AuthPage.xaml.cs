using BookHaven.Services;
using System.Windows;
using System.Windows.Controls;

namespace BookHaven.Views
{
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Заполните все поля";
                return;
            }

            bool success = AuthService.Login(login, password);
            if (success)
            {
                // Открываем главное окно
                MainWindow main = new MainWindow();
                main.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                ErrorText.Text = "Неверный логин или пароль";
            }e
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
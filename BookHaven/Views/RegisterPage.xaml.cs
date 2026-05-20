using BookHaven.Services;
using System.Windows;
using System.Windows.Controls;

namespace BookHaven.Views
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string displayName = DisplayNameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(password))
            {
                ErrorText.Text = "Заполните все поля";
                return;
            }

            bool success = AuthService.Register(login, password, email, displayName);
            if (success)
            {
                // После регистрации — сразу входим и открываем главное окно
                AuthService.Login(login, password);
                MainWindow main = new MainWindow();
                main.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                ErrorText.Text = "Пользователь с таким логином или email уже существует";
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}
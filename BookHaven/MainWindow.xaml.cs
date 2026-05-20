using BookHaven.Services;
using BookHaven.Views;
using System.Windows;

namespace BookHaven
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetupSidebar();
            MainFrame.Navigate(new CatalogPage());
        }

        // Показываем/скрываем кнопки sidebar в зависимости от роли
        private void SetupSidebar()
        {
            if (AuthService.IsAdmin())
                BtnAdmin.Visibility = Visibility.Visible;

            if (AuthService.IsAuthor())
                BtnAuthor.Visibility = Visibility.Visible;

            if (AuthService.IsFrozen())
                BtnFrozen.Visibility = Visibility.Visible;
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new CatalogPage());

        private void BtnLists_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new ReadingListPage());

        private void BtnProfile_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new ProfilePage());

        private void BtnAuthor_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new AuthorPage());

        private void BtnAdmin_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new AdminPage());

        private void BtnFrozen_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new ProfilePage());
    }
}
using BookHaven.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookHaven.Views
{
    public partial class CatalogPage : Page
    {
        private List<Books> _allBooks;
        private bool _searchPlaceholder = true;

        public CatalogPage()
        {
            InitializeComponent();
            LoadGenres();
            LoadBooks();
        }

        private void LoadGenres()
        {
            GenreBox.Items.Clear();
            GenreBox.Items.Add(new ComboBoxItem
            { Content = "Все жанры", IsSelected = true });

            var genres = Core.Context.Genres.ToList();
            foreach (var g in genres)
                GenreBox.Items.Add(new ComboBoxItem { Content = g.GenreName, Tag = g.GenreId });
        }

        private void LoadBooks()
        {
            _allBooks = BookService.GetAll();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var books = _allBooks.ToList();

            if (!_searchPlaceholder && !string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string q = SearchBox.Text.ToLower();
                books = books.Where(b =>
                    b.Title.ToLower().Contains(q) ||
                    b.Users.DisplayName.ToLower().Contains(q)).ToList();
            }

            if (GenreBox.SelectedItem is ComboBoxItem gi && gi.Tag is int genreId)
                books = books.Where(b => b.Genres.Any(g => g.GenreId == genreId)).ToList();

            if (SortBox.SelectedIndex == 1)
                books = books.OrderByDescending(b =>
                    b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0).ToList();
            else
                books = books.OrderBy(b => b.Title).ToList();

            RenderBooks(books);
        }

        private void RenderBooks(List<Books> books)
        {
            BooksPanel.Children.Clear();

            foreach (var book in books)
            {
                double avgRating = book.Reviews.Any()
                    ? book.Reviews.Average(r => r.Rating) : 0;

                var card = new Border
                {
                    Width = 180,
                    Height = 260,
                    Margin = new Thickness(8),
                    Background = new SolidColorBrush(Color.FromRgb(42, 42, 62)),
                    CornerRadius = new CornerRadius(8),
                    Cursor = Cursors.Hand
                };

                var stack = new StackPanel { Margin = new Thickness(12) };

                var cover = new Border
                {
                    Height = 120,
                    Background = new SolidColorBrush(Color.FromRgb(124, 58, 237)),
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(0, 0, 0, 8)
                };
                cover.Child = new TextBlock
                {
                    Text = "📖",
                    FontSize = 40,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                stack.Children.Add(cover);
                stack.Children.Add(new TextBlock
                {
                    Text = book.Title,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 13,
                    TextWrapping = TextWrapping.Wrap,
                    MaxHeight = 40
                });
                stack.Children.Add(new TextBlock
                {
                    Text = book.Users.DisplayName,
                    Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170)),
                    FontSize = 11,
                    Margin = new Thickness(0, 4, 0, 0)
                });
                stack.Children.Add(new TextBlock
                {
                    Text = avgRating > 0 ? $"⭐ {avgRating:F1}" : "Нет оценок",
                    Foreground = new SolidColorBrush(Color.FromRgb(250, 204, 21)),
                    FontSize = 12,
                    Margin = new Thickness(0, 4, 0, 0)
                });

                card.Child = stack;

                var bookId = book.BookId;
                card.MouseLeftButtonUp += (s, e) =>
                    NavigationService.Navigate(new BookPage(bookId));

                BooksPanel.Children.Add(card);
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_searchPlaceholder)
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.White;
                _searchPlaceholder = false;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Поиск по названию или автору...";
                SearchBox.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                _searchPlaceholder = true;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_searchPlaceholder) ApplyFilters();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            ApplyFilters();

        private void GenreBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            ApplyFilters();
    }
}
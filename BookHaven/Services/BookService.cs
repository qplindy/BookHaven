using System.Collections.Generic;
using System.Linq;

namespace BookHaven.Services
{
    internal class BookService
    {
        // Все незамороженные книги
        public static List<Books> GetAll() =>
            Core.Context.Books
                .Include("Users")
                .Include("Genres")
                .Where(b => b.IsFrozen == false)
                .ToList();

        // Поиск по названию или автору
        public static List<Books> Search(string query) =>
            Core.Context.Books
                .Include("Users")
                .Include("Genres")
                .Where(b => b.IsFrozen == false &&
                       (b.Title.Contains(query) ||
                        b.Users.DisplayName.Contains(query)))
                .ToList();

        // Книги конкретного автора
        public static List<Books> GetByAuthor(int authorId) =>
            Core.Context.Books
                .Include("Genres")
                .Where(b => b.AuthorId == authorId)
                .ToList();

        // Добавить книгу
        public static void Add(string title, string description, string content)
        {
            Core.Context.Books.Add(new Books
            {
                Title = title,
                Description = description,
                Content = content,
                AuthorId = AppSession.CurrentUser.UserId,
                IsFrozen = false,
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }

        // Заморозить книгу
        public static void Freeze(int bookId)
        {
            var book = Core.Context.Books.Find(bookId);
            if (book == null) return;
            book.IsFrozen = true;
            Core.Context.SaveChanges();
        }

        // Разморозить книгу
        public static void Unfreeze(int bookId)
        {
            var book = Core.Context.Books.Find(bookId);
            if (book == null) return;
            book.IsFrozen = false;
            Core.Context.SaveChanges();
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace BookHaven.Services
{
    internal class ReviewService
    {
        // Отзывы на книгу
        public static List<Reviews> GetByBook(int bookId) =>
            Core.Context.Reviews
                .Include("Users")
                .Where(r => r.BookId == bookId)
                .ToList();

        // Добавить отзыв
        public static void Add(int bookId, string text, int rating)
        {
            Core.Context.Reviews.Add(new Reviews
            {
                UserId = AppSession.CurrentUser.UserId,
                BookId = bookId,
                ReviewText = text,
                Rating = rating,
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }

        // Отзывы текущего пользователя
        public static List<Reviews> GetByCurrentUser() =>
            Core.Context.Reviews
                .Include("Books")
                .Where(r => r.UserId == AppSession.CurrentUser.UserId)
                .ToList();
    }
}
using System.Collections.Generic;
using System.Linq;

namespace BookHaven.Services
{
    internal class ReadingListService
    {
        // Книги по статусу (1-Заброшено, 2-В планах, 3-Читаю, 4-Прочитано)
        public static List<ReadingLists> GetByStatus(int statusId) =>
            Core.Context.ReadingLists
                .Include("Books")
                .Include("Books.Genres")
                .Where(r => r.UserId == AppSession.CurrentUser.UserId
                         && r.StatusId == statusId)
                .ToList();

        // Добавить или переместить книгу в списке
        public static void AddOrMove(int bookId, int statusId)
        {
            var existing = Core.Context.ReadingLists
                .FirstOrDefault(r => r.UserId == AppSession.CurrentUser.UserId
                                  && r.BookId == bookId);
            if (existing != null)
                existing.StatusId = statusId;
            else
                Core.Context.ReadingLists.Add(new ReadingLists
                {
                    UserId = AppSession.CurrentUser.UserId,
                    BookId = bookId,
                    StatusId = statusId,
                    AddedAt = System.DateTime.Now
                });
            Core.Context.SaveChanges();
        }

        // Удалить книгу из списка
        public static void Remove(int bookId)
        {
            var item = Core.Context.ReadingLists
                .FirstOrDefault(r => r.UserId == AppSession.CurrentUser.UserId
                                  && r.BookId == bookId);
            if (item == null) return;
            Core.Context.ReadingLists.Remove(item);
            Core.Context.SaveChanges();
        }

        // Текущий статус книги у пользователя
        public static int? GetStatus(int bookId) =>
            Core.Context.ReadingLists
                .FirstOrDefault(r => r.UserId == AppSession.CurrentUser.UserId
                                  && r.BookId == bookId)?.StatusId;
    }
}
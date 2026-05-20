using System.Collections.Generic;
using System.Linq;

namespace BookHaven.Services
{
    internal class UserService
    {
        // Все пользователи (для администратора)
        public static List<Users> GetAll() =>
            Core.Context.Users.Include("Roles").ToList();

        // Заморозить пользователя
        public static void Freeze(int userId)
        {
            var user = Core.Context.Users.Find(userId);
            if (user == null) return;
            user.IsFrozen = true;
            Core.Context.SaveChanges();
        }

        // Сменить роль
        public static void SetRole(int userId, int roleId)
        {
            var user = Core.Context.Users.Find(userId);
            if (user == null) return;
            user.RoleId = roleId;
            Core.Context.SaveChanges();
        }

        // Подать заявку на роль автора
        public static void ApplyForAuthor()
        {
            Core.Context.RoleApplications.Add(new RoleApplications
            {
                UserId = AppSession.CurrentUser.UserId,
                Status = "Pending",
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }

        // Оспорить заморозку аккаунта или книги
        public static void ApplyForUnfreeze(string reason, int? bookId = null)
        {
            Core.Context.UnfreezeApplications.Add(new UnfreezeApplications
            {
                UserId = AppSession.CurrentUser.UserId,
                BookId = bookId,
                Reason = reason,
                Status = "Pending",
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }
    }
}   
using System.Collections.Generic;
using System.Linq;

namespace BookHaven.Services
{
    internal class AdminService
    {
        // Все жалобы
        public static List<Complaints> GetComplaints() =>
            Core.Context.Complaints
                .Include("Users")
                .Include("Books")
                .Include("Reviews")
                .ToList();

        // Удалить жалобу (принять/отклонить — жалоба закрывается)
        public static void RemoveComplaint(int complaintId)
        {
            var c = Core.Context.Complaints.Find(complaintId);
            if (c == null) return;
            Core.Context.Complaints.Remove(c);
            Core.Context.SaveChanges();
        }

        // Заявки на роль автора
        public static List<RoleApplications> GetRoleApplications() =>
            Core.Context.RoleApplications
                .Include("Users")
                .Where(a => a.Status == "Pending")
                .ToList();

        // Одобрить заявку на автора
        public static void ApproveRoleApplication(int appId)
        {
            var app = Core.Context.RoleApplications.Find(appId);
            if (app == null) return;
            app.Status = "Approved";
            var user = Core.Context.Users.Find(app.UserId);
            if (user != null) user.RoleId = 2; // Автор
            Core.Context.SaveChanges();
        }

        // Отклонить заявку на автора
        public static void RejectRoleApplication(int appId)
        {
            var app = Core.Context.RoleApplications.Find(appId);
            if (app == null) return;
            app.Status = "Rejected";
            Core.Context.SaveChanges();
        }

        // Заявки на разморозку
        public static List<UnfreezeApplications> GetUnfreezeApplications() =>
            Core.Context.UnfreezeApplications
                .Include("Users")
                .Include("Books")
                .Where(a => a.Status == "Pending")
                .ToList();

        // Одобрить разморозку
        public static void ApproveUnfreeze(int appId)
        {
            var app = Core.Context.UnfreezeApplications.Find(appId);
            if (app == null) return;
            app.Status = "Approved";
            if (app.BookId != null)
                BookService.Unfreeze(app.BookId.Value);
            else
                UserService.Freeze(app.UserId); // точнее — разморозить
            Core.Context.SaveChanges();
        }

        // Отклонить разморозку
        public static void RejectUnfreeze(int appId)
        {
            var app = Core.Context.UnfreezeApplications.Find(appId);
            if (app == null) return;
            app.Status = "Rejected";
            Core.Context.SaveChanges();
        }
    }
}   
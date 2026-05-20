using System.Linq;

namespace BookHaven.Services
{
    internal class AuthService
    {
        // Авторизация
        public static bool Login(string login, string password)
        {
            var user = Core.Context.Users
                .FirstOrDefault(u => u.Login == login
                             && u.PasswordHash == password);
            if (user == null) return false;
            AppSession.CurrentUser = user;
            return true;
        }

        // Регистрация
        public static bool Register(string login, string password,
                                    string email, string displayName)
        {
            bool exists = Core.Context.Users
                .Any(u => u.Login == login || u.Email == email);
            if (exists) return false;

            Core.Context.Users.Add(new Users
            {
                Login = login,
                PasswordHash = password,
                Email = email,
                DisplayName = displayName,
                RoleId = 1,        // Читатель
                IsFrozen = false,
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
            return true;
        }

        // Проверки роли
        public static bool IsAdmin() => AppSession.CurrentUser?.RoleId == 3;
        public static bool IsAuthor() => AppSession.CurrentUser?.RoleId == 2;
        public static bool IsFrozen() => AppSession.CurrentUser?.IsFrozen == true;
    }
}
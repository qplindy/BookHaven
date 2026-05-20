namespace BookHaven.Services
{
    internal class ComplaintService
    {
        // Жалоба на книгу
        public static void OnBook(int bookId, string reason)
        {
            Core.Context.Complaints.Add(new Complaints
            {
                UserId = AppSession.CurrentUser.UserId,
                BookId = bookId,
                Reason = reason,
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }

        // Жалоба на отзыв
        public static void OnReview(int reviewId, string reason)
        {
            Core.Context.Complaints.Add(new Complaints
            {
                UserId = AppSession.CurrentUser.UserId,
                ReviewId = reviewId,
                Reason = reason,
                CreatedAt = System.DateTime.Now
            });
            Core.Context.SaveChanges();
        }
    }
}
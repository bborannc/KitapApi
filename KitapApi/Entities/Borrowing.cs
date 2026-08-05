namespace KitapApi.Entities
{
    public class Borrowing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public DateTime BorrowedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedDate { get; set; }
    }
}

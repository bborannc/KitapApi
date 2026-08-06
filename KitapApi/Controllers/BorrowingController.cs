using System.Security.Claims;
using KitapApi.Data;
using KitapApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitapApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Member,Admin")]
    public class BorrowingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BorrowingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("borrow/{bookId}")]
        public async Task<IActionResult> BorrowBook(int bookId)
        {
            // Claim'den User ID okuma (Her iki claim tipi için esnek kontrol)
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı." });

            int userId = int.Parse(userIdStr);

            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound(new { Message = "Kitap bulunamadı." });
            if (book.IsBorrowed) return BadRequest(new { Message = "Bu kitap zaten ödünç alınmış." });

            book.IsBorrowed = true;

            var borrowing = new Borrowing
            {
                BookId = bookId,
                UserId = userId,
                BorrowedDate = DateTime.UtcNow
            };

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Kitap başarıyla ödünç alındı." });
        }

        [HttpPost("return/{bookId}")]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı." });

            int userId = int.Parse(userIdStr);

            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.ReturnedDate == null);

            if (borrowing == null)
            {
                return NotFound(new { Message = "Bu kitaba ait aktif bir ödünç kaydı bulunamadı." });
            }

            borrowing.ReturnedDate = DateTime.UtcNow;

            var book = await _context.Books.FindAsync(bookId);
            if (book != null) book.IsBorrowed = false;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Kitap kütüphaneye iade edildi." });
        }
    }
}

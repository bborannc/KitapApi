using AutoMapper;
using KitapApi.Data;
using KitapApi.Dtos;
using KitapApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitapApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Books (Herkes veya Giriş Yapmış Kullanıcılar)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BookResponseDto>>(books));
        }

        // POST: api/Books (Sadece Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] BookCreateDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<BookResponseDto>(book);
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, response);
        }

        // DELETE: api/Books/5 (Sadece Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound(new { Message = "Kitap bulunamadı." });

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Kitap başarıyla silindi." });
        }
    }
}

using KoneProject.Datas;
using KoneProject.DTOs;
using KoneProject.Models;
using Microsoft.EntityFrameworkCore;

namespace KoneProject.Repository
{
    public class BookRepository : IBookRepository
    {
        // Constructor
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookModel>> GetAllAsync() => await _context.Books.ToListAsync();

        public async Task<BookModel?> GetByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book;
        }

        public async Task<BookModel> CreateAsync(BookModel bookModel)
        {
           _context.Books.Add(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

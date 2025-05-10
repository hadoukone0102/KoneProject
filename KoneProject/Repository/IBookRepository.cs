using KoneProject.Models;

namespace KoneProject.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetAllAsync();
        Task<BookModel?> GetByIdAsync(int id);
        Task<BookModel> CreateAsync(BookModel book);
        Task<bool> DeleteAsync(int id);
    }
}

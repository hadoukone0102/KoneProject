using KoneProject.DTOs;

namespace KoneProject.Interfaces
{
    public interface IBookServices
    {
        Task<IEnumerable<BooksDto>> GetAllAsync();
        Task<BooksDto> GetByIdAsync(int id);
        Task<BooksDto> CreateAsync(CreateBooksDto book);
        Task<bool> DeleteAsync(int id);
    }
}

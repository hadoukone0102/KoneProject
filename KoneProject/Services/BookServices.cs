using AutoMapper;
using KoneProject.DTOs;
using KoneProject.Interfaces;
using KoneProject.Models;
using KoneProject.Repository;

namespace KoneProject.Services
{
    public class BookServices: IBookServices
    {
        // constructor
        private readonly IMapper _mapper;
        private readonly IBookRepository _repo;

        public BookServices (IMapper mapper, IBookRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<IEnumerable<BooksDto>> GetAllAsync()
        {
            var books = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BooksDto>>(books);
        }

        public async Task<BooksDto> GetByIdAsync(int id)
        {
            var books = await _repo.GetByIdAsync(id);
            return _mapper.Map<BooksDto>(books);
        }

        public async Task <BooksDto> CreateAsync(CreateBooksDto book)
        {
            var books = _mapper.Map<BookModel>(book);
            var created = await _repo.CreateAsync(books);
            return _mapper.Map<BooksDto>(created);
        }

        public async Task <bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}

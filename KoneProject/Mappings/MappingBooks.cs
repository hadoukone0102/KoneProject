using AutoMapper;
using KoneProject.DTOs;
using KoneProject.Models;

namespace KoneProject.Mappings
{
    public class MappingBooks : Profile
    {
        public MappingBooks() 
        {
            CreateMap<BookModel, BooksDto>();
            CreateMap<CreateBooksDto, BookModel>();
        }
    }
}

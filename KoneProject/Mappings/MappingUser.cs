using AutoMapper;
using KoneProject.DTOs;
using KoneProject.Models;

namespace KoneProject.Mappings
{
    public class MappingUser : Profile
    {
        public MappingUser() 
        {
            {
                CreateMap<UserModel, UserDto>();
            }
        }
    }
}

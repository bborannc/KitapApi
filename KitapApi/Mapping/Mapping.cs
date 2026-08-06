using AutoMapper;
using KitapApi.Dtos;
using KitapApi.Entities;

namespace KitapApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookCreateDto, Book>();
            CreateMap<Book, BookResponseDto>();
        }
    }
}

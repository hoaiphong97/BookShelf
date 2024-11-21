using AutoMapper;
using CoreInfrastructure.Request;
using CoreInfrastructure.Responses;
using Domains.Entities;
using Service.Models.Requests;
using Service.Models.Responses;

namespace BookShelfKimHa.Behavior
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<GetListBookDbResponse, GetListBookResponse>();
            CreateMap<BookDbResponse, BookResponse>();
            CreateMap<GetListBookDbRequest, GetListBookRequest>();

            CreateMap<BookByIdResponse, Book>();
            CreateMap<UpdateBookRequest, Book>();
        }
    }
}

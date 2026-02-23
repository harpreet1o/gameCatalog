using AutoMapper;
using  GamecatalogAPI.Models.Domain;
using GamecatalogAPI.Models.DTO;

namespace GamecatalogAPI.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameDto>().ReverseMap();
            CreateMap<AddGameRequestDto, Game>().ReverseMap();
            CreateMap<UpdateGameRequestDto, Game>().ReverseMap();

        }
    }
     
}

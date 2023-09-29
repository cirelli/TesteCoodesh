using API.DTO;
using API.ViewModels;
using AutoMapper;
using Data.Models;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PokemonMaster, PokemonMasterViewModel>();
            CreateMap<PokemonMasterDTO, PokemonMaster>();

            CreateMap<PokemonCaught, PokemonCaughtViewModel>();
            CreateMap<PokemonCaughtDTO, PokemonCaught>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(p => p.Date ?? DateOnly.FromDateTime(DateTime.Now)));
        }
    }
}

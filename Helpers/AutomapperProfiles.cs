using System;
using System.Linq;
using AutoMapper;
using RestApiDating.Dtos;
using RestApiDating.Models;

namespace RestApiDating.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.FotoUrl, opt =>
                {
                    // MapFrom para especificar la propiedad desde el source
                    opt.MapFrom(src => src.Fotos.FirstOrDefault(f => f.EsPrincipal).Url);
                })
                .ForMember(dest => dest.Edad, opt =>
                {
                    // ResolveUsing para propiedades calculadas
                    opt.ResolveUsing(src => src.FechaNacimiento.CalcularEdad());
                });
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.FotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Fotos.FirstOrDefault(f => f.EsPrincipal).Url);
                })
                .ForMember(dest => dest.Edad, opt =>
                {
                    opt.ResolveUsing(src => src.FechaNacimiento.CalcularEdad());
                });
            CreateMap<Foto, FotoDto>();
        }
    }
}
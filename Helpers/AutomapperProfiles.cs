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
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => PrincipalFotoUrl(src)))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => CalcularEdad(src)));
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => PrincipalFotoUrl(src)))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => CalcularEdad(src)));
            CreateMap<Foto, FotoDto>();
        }

        #region custom mappings
        private string PrincipalFotoUrl(User user)
        {
            return user.Fotos.FirstOrDefault(f => f.EsPrincipal).Url;
        }

        private int CalcularEdad(User user)
        {
            var fechaNacimiento = user.FechaNacimiento;
            if (fechaNacimiento != null)
            {
                var edad = DateTime.Today.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value.AddYears(edad) > DateTime.Today)
                {
                    edad--;
                } 
                return edad;
            }

            return 0;
        }
        #endregion
    }
}
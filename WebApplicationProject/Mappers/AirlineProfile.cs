using AutoMapper;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Mappers
{
    public class AirlineProfile : Profile
    {
        static Dictionary<string, long> map_name_to_countryid = new Dictionary<string, long>();
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
        public AirlineProfile()
        {
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                map_name_to_countryid.Add(country.Name, country.Id);
            }

            CreateMap<AirlineDTO, AirlineCompany>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Country_Id, opt => opt.MapFrom(src => map_name_to_countryid[src.Country_Name]))
                .ForMember(dest => dest.User_Id, opt => opt.MapFrom(src => src.User_Id))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
    }
}

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
        static Dictionary<long, Country> map_countryid_to_name = new Dictionary<long, Country>();
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
        public AirlineProfile()
        {
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                map_name_to_countryid.Add(country.Name, country.Id);
            }
            foreach (Country country in countries)
            {
                map_countryid_to_name.Add(country.Id, country);
            }

            CreateMap<AirlineDTO, AirlineCompany>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Country_Id, opt => opt.MapFrom(src => map_name_to_countryid[src.Country_Name]))
                .ForMember(dest => dest.User_Id, opt => opt.MapFrom(src => src.User_Id))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ReverseMap()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.Country_Name, opt => opt.MapFrom(src => map_countryid_to_name[src.Country_Id].Name))
                .ForPath(dest => dest.User_Id, opt => opt.MapFrom(src => src.User_Id))
                .ForPath(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
    }
}

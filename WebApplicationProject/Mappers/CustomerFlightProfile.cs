using AutoMapper;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Mappers
{
    public class CustomerFlightProfile : Profile
    {
        static Dictionary<long, Country> map_countryid_to_name = new Dictionary<long, Country>();
        static Dictionary<string, long> map_name_to_countryid = new Dictionary<string, long>();
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
        public CustomerFlightProfile()
        {
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                map_countryid_to_name.Add(country.Id, country);
            }
            foreach (Country country in countries)
            {
                map_name_to_countryid.Add(country.Name, country.Id);
            }

            CreateMap<Flight,CustomerFlightDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AirlineCompanyName, opt => opt.MapFrom(src => src.Airline_Company.Name))
                .ForMember(dest => dest.OrigionCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Origin_Country_Id].Name))
                .ForMember(dest => dest.DestinationCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Destination_Country_Id].Name))
                .ForMember(dest => dest.Departure_Time, opt => opt.MapFrom(src => src.Departure_Time))
                .ForMember(dest => dest.Landing_Time, opt => opt.MapFrom(src => src.Landing_Time))
                .ReverseMap();
        }
    }
}

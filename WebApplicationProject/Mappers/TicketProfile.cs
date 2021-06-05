using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Mappers
{
    public class TicketProfile : Profile
    {
        static Dictionary<long, Country> map_countryid_to_name = new Dictionary<long, Country>();
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
        public TicketProfile()
        {
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                map_countryid_to_name.Add(country.Id, country);
            }

            CreateMap<Ticket, TicketDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AirlineCompanyName, opt => opt.MapFrom(src => src.Flight.Airline_Company.Name))
                .ForMember(dest => dest.OrigionCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Flight.Origin_Country_Id]))
                .ForMember(dest => dest.DestinationCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Flight.Destination_Country_Id]))
                .ForMember(dest => dest.Departure_Time, opt => opt.MapFrom(src => src.Flight.Departure_Time))
                .ForMember(dest => dest.Landing_Time, opt => opt.MapFrom(src => src.Flight.Landing_Time));
        }
    }
}

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
        FlightDAOPGSQL flightDAOPGSQL = new FlightDAOPGSQL();
        public TicketProfile()
        {
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                map_countryid_to_name.Add(country.Id, country);
            }

            CreateMap<Ticket, TicketDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Flight_Id, opt => opt.MapFrom(src => src.Flight_Id))
                .ForMember(dest => dest.AirlineCompanyName, opt => opt.MapFrom(src => src.Flight.Airline_Company.Name))
                .ForMember(dest => dest.OrigionCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Flight.Origin_Country_Id].Name))
                .ForMember(dest => dest.DestinationCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Flight.Destination_Country_Id].Name))
                .ForMember(dest => dest.Departure_Time, opt => opt.MapFrom(src => src.Flight.Departure_Time))
                .ForMember(dest => dest.Landing_Time, opt => opt.MapFrom(src => src.Flight.Landing_Time))
                .ForMember(dest => dest.First_Name, opt => opt.MapFrom(src => src.Customer.First_Name))
                .ForMember(dest => dest.Last_Name, opt => opt.MapFrom(src => src.Customer.Last_Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Customer.Address))
                .ForMember(dest => dest.Phone_No, opt => opt.MapFrom(src => src.Customer.Phone_No))
                .ReverseMap();
        }
    }
}

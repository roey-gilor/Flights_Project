using AutoMapper;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Mappers
{
    public class AirlineFlightProfile : Profile
    {
        static Dictionary<long, Country> map_countryid_to_name = new Dictionary<long, Country>();
        static Dictionary<string, long> map_name_to_countryid = new Dictionary<string, long>();
        static Dictionary<string, AirlineCompany> map_company_name_to_company = new Dictionary<string, AirlineCompany>();
        AirlineDAOPGSQL airlineDAOPGSQL = new AirlineDAOPGSQL();
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
        public AirlineFlightProfile()
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

            IList<AirlineCompany> airlines = airlineDAOPGSQL.GetAll();
            foreach (AirlineCompany airline in airlines)
            {
                map_company_name_to_company.Add(airline.Name, airline);
            }

            CreateMap<Flight, AirlineFlightDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AirlineCompanyName, opt => opt.MapFrom(src => src.Airline_Company.Name))
                .ForMember(dest => dest.OrigionCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Origin_Country_Id].Name))
                .ForMember(dest => dest.DestinationCountry, opt => opt.MapFrom(src => map_countryid_to_name[src.Destination_Country_Id].Name))
                .ForMember(dest => dest.Departure_Time, opt => opt.MapFrom(src => src.Departure_Time))
                .ForMember(dest => dest.Landing_Time, opt => opt.MapFrom(src => src.Landing_Time))
                .ForMember(dest => dest.RemainingTickets, opt => opt.MapFrom(src => src.Remaining_Tickets))
                .ReverseMap()
                .ForPath(dest => dest.Airline_Company_Id, opt => opt.MapFrom(src => map_company_name_to_company[src.AirlineCompanyName].Id))
                .ForPath(dest => dest.Origin_Country_Id, opt => opt.MapFrom(src => map_name_to_countryid[src.OrigionCountry]))
                .ForPath(dest => dest.Destination_Country_Id, opt => opt.MapFrom(src => map_name_to_countryid[src.DestinationCountry]))
                .ForPath(dest => dest.Airline_Company, opt => opt.MapFrom(src => map_company_name_to_company[src.AirlineCompanyName]));
        }
    }
}

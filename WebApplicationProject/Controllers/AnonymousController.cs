using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnonymousController : ControllerBase
    {
        private IAnonymousUserFacade m_facade;
        private readonly IMapper m_mapper;
        private readonly ILogger<AnonymousController> _logger;
        public AnonymousController(IAnonymousUserFacade anonymousFacade, IMapper mapper, ILogger<AnonymousController> logger)
        {
            m_facade = anonymousFacade;
            m_mapper = mapper;
            _logger = logger;
        }
        [HttpPost("CreateNewCustomer")]
        public async Task<ActionResult<Customer>> CreateNewCustomer([FromBody] Customer customer)
        {
            long id;
            try
            {
                id = await Task.Run(() => m_facade.AddNewCustomer(customer));
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            customer.Id = id;
            return Created($"api/Anonymous/CreateNewCustomer/{id}", JsonConvert.SerializeObject(customer));
        }
        [HttpPost("CreateNewWaitingAirline")]
        public async Task<ActionResult<Customer>> CreateNewWaitingAirline([FromBody] AirlineDTO airline)
        {
            long id;
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            try
            {
                id = await Task.Run(() => m_facade.AddNewWaitingAirline(airlineCompany));
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
            airlineCompany.Id = id;
            return Created($"api/Anonymous/CreateNewWaitingAirline/{id}", JsonConvert.SerializeObject(airlineCompany));
        }
        [HttpPost("CreateNewWaitingAdmin")]
        public async Task<ActionResult<Customer>> CreateNewWaitingAdmin([FromBody] Administrator administrator)
        {
            long id;
            try
            {
                id = await Task.Run(() => m_facade.AddNewWaitingAdmin(administrator));
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
            administrator.Id = id;
            return Created($"api/Anonymous/CreateNewWaitingAdmin/{id}", JsonConvert.SerializeObject(administrator));
        }
        [HttpGet("GetAllAirlineCompanies")]
        public async Task<ActionResult<List<AirlineReducedDTO>>> GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlines = await Task.Run(() => m_facade.GetAllAirlineCompanies());
            List<AirlineReducedDTO> airlineDTOs = new List<AirlineReducedDTO>();
            foreach (AirlineCompany airline in airlines)
            {
                AirlineReducedDTO airlineDTO = m_mapper.Map<AirlineReducedDTO>(airline);
                airlineDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineDTOs));
        }
        [HttpGet("GetAllFlights")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetAllFlights()
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetAllFlights());
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetAllFlightsVacancy")]
        public async Task<ActionResult<Dictionary<CustomerFlightDTO, int>>> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> flightsVacancy = await Task.Run(() => m_facade.GetAllFlightsVacancy());
            Dictionary<CustomerFlightDTO, int> customerFlightsDTOs = new Dictionary<CustomerFlightDTO, int>();
            foreach (KeyValuePair<Flight, int> flight in flightsVacancy)
            {
                CustomerFlightDTO flightDTO = m_mapper.Map<CustomerFlightDTO>(flight.Key);
                customerFlightsDTOs.Add(flightDTO, flight.Value);
            }
            return Ok(JsonConvert.SerializeObject(customerFlightsDTOs));
        }
        [HttpGet("GetFlightsByDepatrureDate")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByDepatrureDate(DateTime date)
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByDepatrureDate(date));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByLandingDate")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByLandingDate(DateTime date)
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByLandingDate(date));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByDestinationCountry")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByDestinationCountry(string countryName)
        {
            long id = CountryDTO.ReturnIdFromName(countryName);
            if (id == 0)
                return StatusCode(400, $"{{ error: \"{"Country name does not exists"}\" }}");
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByDestinationCountry((int)id));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByOriginCountry")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByOriginCountry(string countryName)
        {
            long id = CountryDTO.ReturnIdFromName(countryName);
            if (id == 0)
                return StatusCode(400, $"{{ error: \"{"Country name does not exists"}\" }}");
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByOriginCountry((int)id));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetAllCountries")]
        public async Task<ActionResult<IList<Country>>> GetAllCountries()
        {
            IList<Country> countries = new List<Country>();
            countries = await Task.Run(() => m_facade.GetAllCountries());
            return Ok(JsonConvert.SerializeObject(countries));
        }
    }
}

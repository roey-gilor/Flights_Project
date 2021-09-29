using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "Administrator")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdministratorController : FlightControllerBase<Administrator>
    {
        private ILoggedInAdministratorFacade m_facade;
        private readonly IMapper m_mapper;
        public AdministratorController(ILoggedInAdministratorFacade adminFacade, IMapper mapper)
        {
            m_facade = adminFacade;
            m_mapper = mapper;
        }
        [HttpPut("ChangeAdminPassword")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] UserDetailsDTO userDetails)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.ChangeMyPassword(token, token.User.Password, userDetails.Password));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPost("CreateAdmin")]
        public async Task<ActionResult<Administrator>> CreateAdmin ([FromBody] Administrator administrator)
        {
            LoginToken<Administrator> token = GetLoginToken();
            long id = 0;
            try
            {
                id = await Task.Run(() => { return m_facade.CreateAdmin(token, administrator); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            administrator.Id = id;
            return Created($"api/Administrator/CreateAdmin/{id}", JsonConvert.SerializeObject(administrator));
        }
        [HttpPost("CreateCountry")]
        public async Task<ActionResult> CreateCountry([FromBody] Country country)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.CreateCountry(token, country));
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Created("api/Administrator/CreateCountry", JsonConvert.SerializeObject(country.Name));
        }
        [HttpPost("CreateAirlineCompany")]
        public async Task<ActionResult<AirlineCompany>> CreateAirlineCompany([FromBody] AirlineDTO airline)
        {
            LoginToken<Administrator> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            long id = 0;
            try
            {
                id = await Task.Run(() => { return m_facade.CreateNewAirline(token, airlineCompany); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            airline.Id = id;
            airline.User_Id = airline.User.Id;
            return Created($"api/Administrator/CreateAirlineCompany/{id}", JsonConvert.SerializeObject(airline));
        }
        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<AirlineCompany>> CreateCustomer([FromBody] Customer customer)
        {
            LoginToken<Administrator> token = GetLoginToken();
            long id = 0;
            try
            {
                id = await Task.Run(() => { return m_facade.CreateNewCustomer(token, customer); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (DuplicateDetailsException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            customer.Id = id;
            return Created($"api/Administrator/CreateCustomer/{id}", JsonConvert.SerializeObject(customer));
        }
        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<IList<Customer>>> GetAllCustomers()
        {
            LoginToken<Administrator> token = GetLoginToken();
            IList<Customer> customers = null;
            try
            {
                customers = await Task.Run(() => { return m_facade.GetAllCustomers(token); });
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            if (customers.Count == 0)
                return StatusCode(204, "{ }");
            return Ok(JsonConvert.SerializeObject(customers));
        }
        [HttpPut("UpdateMyDetails")]
        public async Task<ActionResult> UpdateMyDetails([FromBody] Administrator admin)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.ModifyMyAdminUser(token, admin));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPut("UpdateAdmin")]
        public async Task<ActionResult> UpdateAdmin([FromBody] Administrator admin)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.UpdateAdmin(token, admin));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPut("UpdateAirlineDetails")]
        public async Task<ActionResult> UpdateAirlineDetails ([FromBody] AirlineDTO airline)
        {
            LoginToken<Administrator> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            try
            {
                await Task.Run(() => m_facade.UpdateAirlineDetails(token, airlineCompany));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPut("UpdateCountry")]
        public async Task<ActionResult> UpdateCountry ([FromBody] Country country)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.UpdateCountry(token, country));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPut("UpdateCustomerDetails")]
        public async Task<ActionResult> UpdateCustomerDetails ([FromBody] Customer customer)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.UpdateCustomerDetails(token, customer));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpDelete("RemoveAdmin")]
        public async Task<ActionResult> RemoveAdmin ([FromBody] Administrator administrator)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => { m_facade.RemoveAdmin(token, administrator); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpDelete("RemoveAirline")]
        public async Task<ActionResult> RemoveAirline([FromBody] AirlineDTO airlineDTO)
        {
            LoginToken<Administrator> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airlineDTO);
            try
            {
                await Task.Run(() => { m_facade.RemoveAirline(token, airlineCompany); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpDelete("RemoveCountry/{countryId}")]
        public async Task<ActionResult> RemoveCountry(long countryId)
        {
            LoginToken<Administrator> token = GetLoginToken();
            string countryName = CountryDTO.returnNameFromId(countryId);
            if (countryName == null)
                return StatusCode(400, $"{{ error: \"{"Country id does not exists"}\" }}");
            Country country = new Country { Id = countryId, Name = countryName };
            try
            {
                await Task.Run(() => { m_facade.RemoveCountry(token, country); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpDelete("RemoveCustomer")]
        public async Task<ActionResult> RemoveCustomer([FromBody] Customer customer)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => { m_facade.RemoveCustomer(token, customer); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpGet("GetAllWaitingAirlines")]
        public async Task<ActionResult<IList<AirlineDTO>>> GetAllWaitingAirlines()
        {
            LoginToken<Administrator> token = GetLoginToken();
            IList<AirlineCompany> airlineCompanies = null;
            try
            {
                airlineCompanies = await Task.Run(() => { return m_facade.GetWaitingAirlines(token); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            if (airlineCompanies.Count == 0)
                return StatusCode(204, "{ }");
            IList<AirlineDTO> airlineDTOs = new List<AirlineDTO>();
            foreach (AirlineCompany airline in airlineCompanies)
            {
                AirlineDTO airlineDTO = m_mapper.Map<AirlineDTO>(airline);
                airlineDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineDTOs));
        }
        [HttpPost("ApproveAirline")]
        public async Task<ActionResult<AirlineCompany>> ApproveAirline([FromBody] AirlineDTO airline)
        {
            LoginToken<Administrator> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            long id = 0;
            try
            {
                id = await Task.Run(() => { return m_facade.AcceptWaitingAirline(token, airlineCompany); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            airline.Id = id;
            airline.User.User_Role = 2;
            return Created($"api/Administrator/ApproveAirline/{id}", JsonConvert.SerializeObject(airline));
        }
        [HttpDelete("RejectAirline")]
        public async Task<ActionResult<AirlineCompany>> RejectAirline([FromBody] AirlineDTO airline)
        {
            LoginToken<Administrator> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            try
            {
                await Task.Run(() => m_facade.RejectWaitingAirline(token, airlineCompany));
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpGet("GetAllWaitingAdmins")]
        public async Task<ActionResult<IList<AirlineDTO>>> GetAllWaitingAdmins()
        {
            LoginToken<Administrator> token = GetLoginToken();
            IList<Administrator> administrators = null;
            try
            {
                administrators = await Task.Run(() => { return m_facade.GetWaitingAdmins(token); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            if (administrators.Count == 0)
                return StatusCode(204, "{ }");
            return Ok(JsonConvert.SerializeObject(administrators));
        }
        [HttpPost("ApproveAdmin")]
        public async Task<ActionResult<AirlineCompany>> ApproveAdmin([FromBody] Administrator administrator)
        {
            LoginToken<Administrator> token = GetLoginToken();
            long id = 0;
            try
            {
                id = await Task.Run(() => { return m_facade.AcceptWaitingAdmin(token, administrator); });
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            administrator.Id = id;
            administrator.User.User_Role = 1;
            return Created($"api/Administrator/ApproveAdmin/{id}", JsonConvert.SerializeObject(administrator));
        }
        [HttpDelete("RejectAdmin")]
        public async Task<ActionResult<AirlineCompany>> RejectAdmin([FromBody] Administrator administrator)
        {
            LoginToken<Administrator> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.RejectWaitingAdmin(token, administrator));
            }
            catch (AdministratorDoesntHaveSanctionException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByAdministratorException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
    }
}

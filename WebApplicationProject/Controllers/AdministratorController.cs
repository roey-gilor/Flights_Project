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
            catch (WrongCredentialsException ex)
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
            catch (WrongCredentialsException ex)
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
            catch (WrongCredentialsException ex)
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
    }
}

using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : FlightControllerBase<Customer>
    {
        private ILoggedInCustomerFacade m_facade;
        private readonly IMapper m_mapper;
        public CustomersController(ILoggedInCustomerFacade customerFacade, IMapper mapper)
        {
            m_facade = customerFacade;
            m_mapper = mapper;
        }
        private void AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token_customer, out LoggedInCustomerFacade facade)
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            token_customer = (LoginToken<Customer>)token;
            facade = (LoggedInCustomerFacade)facadeBase;
        }

        // GET: api/<CustomersController>
        [HttpGet("GetAllMyFlights")]
        public async Task<ActionResult<IList<Flight>>> GetAllCustomerFlights()
        {
            //AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token, out LoggedInCustomerFacade facade);
            LoginToken<Customer> token = GetLoginToken();
            IList<Flight> flights = null;
            try
            {
                flights = await Task.Run(() => m_facade.GetAllMyFlights(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            if (flights == null)
            {
                return StatusCode(204, "{ }");
            }
            return Ok(flights);

        }

        // GET api/<CustomersController>/5
        [HttpPut("UpdateUserDetails")]
        public async Task<ActionResult> UpdateUserDetails([FromBody] Customer customer)
        {
            AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token, out LoggedInCustomerFacade facade);
            try
            {
                await Task.Run(() => facade.UpdateUserDetails(token, customer));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }

        // POST api/<CustomersController>
        [HttpPost("PurchaseTicket")]
        public async Task<ActionResult<TicketDTO>> PurchaseTicket([FromBody] Flight flight)
        {
            AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token, out LoggedInCustomerFacade facade);
            Ticket ticket = null;
            try
            {
                 ticket = await Task.Run(() => { return facade.PurchaseTicket(token, flight); });                          
            }
            catch (CustomerAlreadyBoughtTicketException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (OutOfTicketsException ex)
            {
                return StatusCode(406, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            TicketDTO ticketDTO = m_mapper.Map<TicketDTO>(ticket);
            return Created($"api/Customer/buyTicket/{ticketDTO.Id}", ticketDTO);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("ChangeMyPassword")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] object newPassword)
        {
            AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token, out LoggedInCustomerFacade facade);
            try
            {
                await Task.Run(() => facade.ChangeMyPassword(token, token.User.User.Password, newPassword.ToString()));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("CancelTicketPurchase")]
        public async Task<ActionResult> CancelTicketPurchase([FromBody] Ticket ticket)
        {
            AuthenticateAndGetTokenAndGetFacade(out LoginToken<Customer> token, out LoggedInCustomerFacade facade);
            try
            {
                await Task.Run(() => facade.CancelTicket(token, ticket));
            }
            catch (Exception ex)
            {
                 return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
    }
}

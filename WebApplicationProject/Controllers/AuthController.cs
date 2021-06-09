using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplicationProject.DTO;
using DAO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public async Task<ActionResult> GetToken([FromBody] UserDetailsDTO userDetails)
        {
            ILoginToken login;
            try
            {
                login = await Task.Run(() =>
                {
                    FlightCenterSystem.Instance.Login(out FacadeBase facade, out ILoginToken loginToken, userDetails.Name, userDetails.Password);
                    return loginToken;
                });

            }
            catch (WrongCredentialsException)
            {
                return Unauthorized("Login Failed");
            }
            User user = GetUser(login);
            string role = GetUserRole(user);

            string securityKey =
       "this_is_our_supper_long_security_key_for_token_validation_project_2018_09_07$smesk.in";

            // symmetric security key
            var symmetricSecurityKey = new
                SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            // signing credentials
            var signingCredentials = new
                  SigningCredentials(symmetricSecurityKey,
                  SecurityAlgorithms.HmacSha256Signature);

            // 3) create claim for specific role
            // add claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Role, role)); // --> here use the role from the login result
            claims.Add(new Claim("userid", user.Id.ToString())); // --> here use the user_id from the result
            claims.Add(new Claim("username", user.User_Name)); // --> here use the name from the login result
            claims.Add(new Claim("mainUserId", GetRealUserId(login).ToString()));

            // 4) create token
            var token = new JwtSecurityToken(
            issuer: "smesk.in", // change to something better
            audience: "readers", // change to something better
            expires: DateTime.Now.AddHours(1), // should be configurable
            signingCredentials: signingCredentials,
            claims: claims);

            // 5) return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private User GetUser(ILoginToken token)
        {
            LoginToken<Administrator> administrator = null;
            LoginToken<AirlineCompany> airline = null;
            LoginToken<Customer> customer = null;
            User user;

            try
            {
                administrator = (LoginToken<Administrator>)token;
                user = administrator.User.User;
            }
            catch (Exception)
            {
                try
                {
                    airline = (LoginToken<AirlineCompany>)token;
                    user = airline.User.User;
                }
                catch (Exception)
                {
                    customer = (LoginToken<Customer>)token;
                    user = customer.User.User;
                }
            }
            return user;
        }
        private string GetUserRole(User user)
        {
            if (user.User_Role == 1)
                return "Administrator";
            if (user.User_Role == 2)
                return "Airline Company";
            return "Customer";
        }
        private long GetRealUserId(ILoginToken token)
        {
            LoginToken<Administrator> administrator = null;
            LoginToken<AirlineCompany> airline = null;
            LoginToken<Customer> customer = null;
            long id = 0;

            try
            {
                administrator = (LoginToken<Administrator>)token;
                id = administrator.User.Id;
            }
            catch (Exception)
            {
                try
                {
                    airline = (LoginToken<AirlineCompany>)token;
                    id = airline.User.Id;
                }
                catch (Exception)
                {
                    customer = (LoginToken<Customer>)token;
                    id = customer.User.Id;
                }
            }
            return id;
        }
    }
}

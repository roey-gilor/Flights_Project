using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace WebApplicationProject.Controllers
{
    public abstract class FlightControllerBase<T> : ControllerBase where T : IUser, new()
    {
        protected LoginToken<T> GetLoginToken()
        {
            string jwtToken = Request.Headers["Authorization"].ToString();

            jwtToken = jwtToken.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken);
            var decodedJwt = jsonToken as JwtSecurityToken;

            string userName = decodedJwt.Claims.First(_ => _.Type == "username").Value;
            long id = Convert.ToInt64(decodedJwt.Claims.First(_ => _.Type == "mainUserId").Value);
            long user_id = Convert.ToInt64(decodedJwt.Claims.First(_ => _.Type == "userid").Value);

            LoginToken<T> login_token = new LoginToken<T>()
            {
                User = new T()
                {
                    Id = id,
                    User_Id = user_id,
                    Name = userName,
                    Password = "no password. created from JWT"
                }
            };
            return login_token;
        }
    }

}
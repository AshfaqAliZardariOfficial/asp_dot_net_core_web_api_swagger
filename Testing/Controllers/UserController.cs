using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.DAOs;
using Testing.Models;
using Testing.Services;
using Testing.Utils;

namespace Testing.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DataProtectionUtils DataProtection;

        public UserController(IDataProtectionProvider provider)
        {
            DataProtection = new DataProtectionUtils(provider);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }
            user.Password = DataProtection.Protect(user.Password);
            // If user not inserted in database,
            // It return The HTTP 409 status code (Conflict).
            if (!new UserService().Insert(user))
            {
                return Conflict(user);
            }

            return Ok(user);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(user);
            }

            user.Password = DataProtection.Protect(user.Password);
            // If user not updated in database,
            // It return The HTTP 409 status code (Conflict).
            if (!new UserService().Update(user))
            {
                return Conflict(user);
            }

            return Ok(user);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] int? id)
        {

            // If user not deleted in database,
            // It return The HTTP 409 status code (Conflict).
            if (!new UserService().Delete(id))
            {
                return Conflict(id);
            }

            return Ok(id);
        }

        [HttpGet("Get")]
        public IActionResult Get([FromQuery] int? ID, [FromQuery] string Username, [FromQuery] bool? OnlineStatus, [FromQuery] DateTime? LastLogin, [FromQuery] DateTime? CreatedAt)
        {

            // If users couldn't getting from database,
            // It return The HTTP 409 status code (Conflict).
            List<UserModel> users = new UserService().Get(ID, Username, OnlineStatus, LastLogin, CreatedAt);
            if (users == null)
            {
                return Conflict(users);
            }

            return Ok(users);
        }
    }
}

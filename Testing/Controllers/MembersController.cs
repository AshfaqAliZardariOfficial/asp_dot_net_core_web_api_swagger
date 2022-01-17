using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.Interfaces;
using Testing.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Testing.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;

        private readonly List<MemberModel> lstMember = new List<MemberModel>()
        {
            new MemberModel{Id=1, Name="SirAhmed" },
            new MemberModel {Id=2, Name="Ashfaq" },
            new MemberModel{Id=3, Name="Ali"}
        };

        public MembersController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }

        // GET: api/<MembersController>
        [HttpGet]
        public IEnumerable<MemberModel> AllMembers()
        {
            return lstMember;
        }

        // GET api/<MembersController>/5
        [HttpGet("{id}")]
        public MemberModel MemberByid(int id)
        {
            return lstMember.Find(x => x.Id == id);
        }

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("authentication")]
        public IActionResult Authentication([FromBody] UserCredentialModel userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        string AdminPassword = "TheBestMessengerInTheW0rld!";

        // POST api/<AdminController>
        [HttpPost]
        public int Post([FromBody] UserData userData)
        {
            if (userData.password == AdminPassword)
            {
                Program.AdminSessons.Add(0, userData.login);
                Console.WriteLine($"{userData.login} got admin permission");
                return 1;
            }
            else
                 return 0;
        }
    }
}

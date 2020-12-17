using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpPost]
        public int Post([FromBody] UserData userData)
        {
            for(int i = 0; i < Program.Allusers.users.Count; i++)
            {
                if(Program.Allusers.users[i].login == userData.login)
                {
                    return -1;
                }
            }
            Program.Allusers.Add(userData.login, userData.password);
            Console.WriteLine($"Added new user {userData.login}");
            return 1;
        }
    }
}

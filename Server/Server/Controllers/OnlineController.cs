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
    public class OnlineController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] Session session )
        {
            for(int i = 0; i< Program.AllSessions.sessions.Count; i++)
            {
                if ((Program.AllSessions.sessions[i].login == session.login) && (Program.AllSessions.sessions[i].token == session.token))
                    Program.AllSessions.sessions[i].online = 1;
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public string Get()
        {
            string all = "";
            string json;
            for (int i = Program.AllSessions.sessions.Count - 1; i >= 0; i--)
            {
                json = JsonConvert.SerializeObject(Program.AllSessions.sessions.ElementAt(i));
                all = all + json.ToString() + "\n";
            }
            return all;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(int id)
        {
            string json = "not found";
            if ((id < Program.AllSessions.sessions.Count) && (id >= 0))
            {
                json = JsonConvert.SerializeObject(Program.AllSessions.sessions.ElementAt(id));
            }
            return json.ToString();
        }

        [HttpPost]
        public void Post([FromBody] Session session)
        {
            Program.AllSessions.Add(session.token, session.login, session.password);
            Console.WriteLine($"Added new user {session.login}");
        }
    }
}

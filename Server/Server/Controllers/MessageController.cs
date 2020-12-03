using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        static Messages messagesBox = new Messages();

        // GET api/<ChatController>/
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(int id)
        {
            string json = "not found";
            if ((id < messagesBox.messages.Count) && (id >= 0))
            {
                json = JsonSerializer.Serialize(messagesBox.messages.ElementAt(id));
            }
            return json.ToString();
        }

        // POST api/<ChatController>
        [HttpPost]
        public void Post([FromBody] Message message)
        {
            messagesBox.Add(message.username, message.text);
            Console.WriteLine($"{(messagesBox.messages.Count)-1}.{message.username}: {message.text} ");
        }
    }
}
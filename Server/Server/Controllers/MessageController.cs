﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        [HttpGet]
        public string Get()
        {
            string all = "";
            string json;
            for (int i = Program.AllMessages.messages.Count - 1; i >= 0; i--)
            {
                json = JsonSerializer.Serialize(Program.AllMessages.messages.ElementAt(i));
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
            if ((id < Program.AllMessages.messages.Count) && (id >= 0))
            {
                json = JsonSerializer.Serialize(Program.AllMessages.messages.ElementAt(id));
            }
            return json.ToString();
        }

        [HttpPost]
        public void Post([FromBody] Message message)
        {
            for (int j = 0; j < Program.AllSessions.sessions.Count; j++)
            {
                if (Program.AllSessions.sessions[j].token == message.token)
                    if(Program.AllSessions.sessions[j].login == message.username)
                    {
                        Program.AllMessages.Add(message.username, message.token, message.text);
                        Console.WriteLine($"{message.username} sent message: '{message.text}' ");
                        return;
                    }
            }
        }
    }
}
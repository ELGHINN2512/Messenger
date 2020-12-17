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
        [HttpPost]
        public int Post([FromBody] UserData userData)
        {
            for (int i = 0; i < Program.Allusers.users.Count; i++)
            {
                if (Program.Allusers.users[i].login == userData.login)
                    if (Program.Allusers.users[i].password == userData.password)
                    {
                        int token = Program.AllSessions.GenerateToken();
                        for(int j = 0; j< Program.AllSessions.sessions.Count; j++)
                        {
                            if(Program.AllSessions.sessions[j].login == userData.login)
                            {
                                Program.AllSessions.sessions[j].token = token;
                                Console.WriteLine($"User {userData.login} logged in. Token: {token}");
                                return token;
                            }
                        }
                        Program.AllSessions.Add(token, userData.login);
                        Console.WriteLine($"User {userData.login} logged in. Token: {token}");
                        return token;
                    }
                    else return -2; // Неверный пароль
            }
            return -1;  // Пользователь не найден
        }
    }
}

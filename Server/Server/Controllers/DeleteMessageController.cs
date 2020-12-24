using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteMessageController : ControllerBase
    {
        [HttpGet("{id}")]
        public int Get(int id)
        {
            if ((id < Program.AllDeletedMessages.Count) && (id > -1))
                return Program.AllDeletedMessages[id];
            else
                return -1;
        }

        [HttpPost]
        public void Post([FromBody] DeleteMessageData deleteMessageData)
        {
            for (int i = 0; i < Program.AdminSessons.sessions.Count; i++)
            {
                if (Program.AdminSessons.sessions[i].login == deleteMessageData.login)
                {
                    Program.AllDeletedMessages.Add(deleteMessageData.messageID);
                    Console.WriteLine($"Admin {deleteMessageData.login} delete message ID = {deleteMessageData.messageID}");

                    Message OldMessage = new Message();
                    OldMessage.username = Program.AllMessages.messages[deleteMessageData.messageID].username;
                    OldMessage.text = Program.AllMessages.messages[deleteMessageData.messageID].text;
                    OldMessage.token = Program.AllMessages.messages[deleteMessageData.messageID].token;
                    OldMessage.time = Program.AllMessages.messages[deleteMessageData.messageID].time;


                    Program.AllMessages.messages[deleteMessageData.messageID].username = "Server";
                    Program.AllMessages.messages[deleteMessageData.messageID].text = "\t\t\tСообщение было удалено администратором";
                    Program.AllMessages.messages[deleteMessageData.messageID].token = 0;

                    string strAllMessages = System.IO.File.ReadAllText("SavedMessages.txt");
                    strAllMessages = strAllMessages.Replace(JsonConvert.SerializeObject(OldMessage).ToString(), JsonConvert.SerializeObject(Program.AllMessages.messages[deleteMessageData.messageID]).ToString());
                    System.IO.File.WriteAllText("SavedMessages.txt", strAllMessages);
                }
            }
        }

    }
}

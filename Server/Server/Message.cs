using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Server.Controllers;
using System.Net;
using System.Text;

namespace Server
{
    [Serializable]
    public class Message
    {
        public string username { get; set; }
        public string text { get; set; }
        public DateTime time { get; set; }
        // Добавить время сообщения
        public Message()
        {
                this.username = "Server";
                this.text = "Server is running";
                this.time = DateTime.UtcNow;
        }

        public Message(string _username, string _text)
        {
            this.username = _username;
            this.text = _text;
            this.time = DateTime.UtcNow;
        }
    }

    [Serializable]
    public class Messages
    {
        public List<Message> messages = new List<Message>();

        public Messages()
        {
            if(File.Exists("SavedMessages.txt"))
            {
                Message message;
                string line;
                StreamReader file = new StreamReader("SavedMessages.txt");
                while((line = file.ReadLine()) != null)
                {
                    message = JsonConvert.DeserializeObject<Message>(line);
                    messages.Add(message);
                    Console.WriteLine($"Message: '{message.text}' from {message.username} has been loaded");
                }
                message = new Message();
                messages.Add(message);
                file.Close();
            }
            else
            {
                Message message = new Message();
                messages.Add(message);
            }
        }

        public void Add(string username, string text)
        {
            Message message = new Message(username, text);
            messages.Add(message);
            File.AppendAllText("SavedMessages.txt", JsonConvert.SerializeObject(message).ToString()+ "\n");
        }
    }
}


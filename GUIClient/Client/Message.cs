using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Server
{
    [Serializable]
    public class Message
    {
        public string username { get; set; }
        public int token { get; set; }
        public string text { get; set; }
        public DateTime time { get; set; }
        // Добавить время сообщения
        public Message()
        {
                this.username = "Server";
                this.token = 0;
                this.text = "Server is running";
                this.time = DateTime.UtcNow;
        }

        public Message(string _username,int _token, string _text)
        {
            this.username = _username;
            this.token = token;
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
        }

        public void Add(string username,int token, string text)
        {
            Message message = new Message(username, token, text);
            messages.Add(message);
        }


    }
}


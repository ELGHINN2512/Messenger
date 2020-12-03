using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            messages.Clear();
            Message message = new Message();
            messages.Add(message);
        }

        //public Messages(List<Message> messages)
        //{
        //    messages.Clear();
        //    Message message = new Message();
        //    messages.Add(message);
        //}
        //public void Add(Message message)
        //{
        //    messages.Add(message);
        //}

        public void Add(string username, string text)
        {
            Message message = new Message(username, text);
            messages.Add(message);
        }

        public override string ToString()
        {
            return messages.ToString();
        }
    }
}
﻿using Server;
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
        public int token { get; set; }
        public string text { get; set; }
        public DateTime time { get; set; }
        public Message()
        {
            this.username = "Server";
            this.token = 0;
            this.text = "\t\t\tServer is running";
            this.time = DateTime.UtcNow;
        }

        public Message(string _username, int _token, string _text)
        {
            this.username = _username;
            this.token = _token;
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
            if (File.Exists("SavedMessages.txt"))
            {
                Message message;
                string line;
                StreamReader file = new StreamReader("SavedMessages.txt");
                while ((line = file.ReadLine()) != null)
                {
                    message = JsonConvert.DeserializeObject<Message>(line);
                    messages.Add(message);
                    Console.WriteLine($"Message: '{message.text}' from {message.username} has been loaded");
                }
                file.Close();
            }
        }

        public void Add(string username,int token, string text)
        {
            Message message = new Message(username,token, text);
            messages.Add(message);
            File.AppendAllText("SavedMessages.txt", JsonConvert.SerializeObject(message).ToString()+ "\n");
        }

        void Add(Message message)
        {
            messages.Add(message);
            File.AppendAllText("SavedMessages.txt", JsonConvert.SerializeObject(message).ToString() + "\n");
        }
    }
}


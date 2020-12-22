using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Session
    {
        public int token { get; set; }
        public string login { get; set; }
        public int online { get; set; }


        public Session(int _token, string _login)
        {
            this.token = _token;
            this.login = _login;
            this.online = 1;
        }
    }

    [Serializable]
    public class UserData
    {
        public string login { get; set; }
        public string password { get; set; }

        public UserData() { }
        public UserData(string _login, string _password)
        {
            login = _login;
            password = _password;
        }
    }

    [Serializable]
    public class DeleteMessageData
    {
        public string login { get; set; }
        public int token { get; set; }
        public int messageID { get; set; }

        public DeleteMessageData() { }

        public DeleteMessageData(string _login,int _token, int _messageID)
        {
            login = _login;
            token = _token;
            messageID = _messageID;
        }
    }
}

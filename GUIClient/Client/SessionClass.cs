using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class SessionClass
    {
        public List<Session> sessions = new List<Session>();

        public SessionClass()
        {
        }

        public void Add(int token,string login, string password)
        {
            Session session = new Session(token, login, password);
            sessions.Add(session);
        }

    }

    [Serializable]
    public class Session
    {
        public int token { get; set; }
        public string login { get; set; }

        public Session(int _token, string _login, string _password)
        {
            this.token = _token;
            this.login = _login;
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
}

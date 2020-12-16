using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//test
namespace Server
{
    [Serializable]
    public class SessionClass
    {
        public List<Session> sessions = new List<Session>();

        public SessionClass()
        {
            if (File.Exists("SavedSessions.txt"))
            {
                Session session;
                string line;
                StreamReader file = new StreamReader("SavedSessions.txt");
                while ((line = file.ReadLine()) != null)
                {
                    session = JsonConvert.DeserializeObject<Session>(line);
                    sessions.Add(session);
                    Console.WriteLine($"User {session.login} has been loaded");
                }
                file.Close();
            }
        }

        public void Add(int token,string login, string password)
        {
            Session session = new Session(token, login, password);
            sessions.Add(session);
            File.AppendAllText("SavedSessions.txt", JsonConvert.SerializeObject(session).ToString() + "\n");
        }

    }

    [Serializable]
    public class Session
    {
        public int token { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        
        public Session()
        {
        }

        public Session(int _token, string _login, string _password)
        {
            this.token = _token;
            this.login = _login;
            this.password = _password;
        }
    }

}

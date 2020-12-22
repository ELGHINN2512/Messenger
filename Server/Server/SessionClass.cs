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

        public void Add(int token,string login)
        {
            Session session = new Session(token, login);
            sessions.Add(session);
        }

        public int GenerateToken()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999);
        }

        public int CheckData(int token, string login)
        {
            for (int i = 0; i < sessions.Count; i++)
            {
                if (sessions[i].login == login)
                    if (sessions[i].token == token)
                        return 1;
                    else
                        return -1;
            }
            return -1;
        }
    }

    [Serializable]
    public class Session
    {
        public int token { get; set; }
        public string login { get; set; }
        public int online { get; set; }
        
        public Session()
        {
        }

        public Session(int _token, string _login)
        {
            this.token = _token;
            this.login = _login;
            this.online = 1;
        }
    }

    [Serializable]
    public class Users
    {
        public List<UserData> users = new List<UserData>();

        public Users()
        {
            if (File.Exists("SavedUsers.txt"))
            {
                UserData userData;
                string line;
                StreamReader file = new StreamReader("SavedUsers.txt");
                while ((line = file.ReadLine()) != null)
                {
                    userData = JsonConvert.DeserializeObject<UserData>(line);
                    users.Add(userData);
                    Console.WriteLine($"User {userData.login} has been loaded");
                }
                file.Close();
            }
        }

        public void Add(string login, string password)
        {
            UserData userData = new UserData(login, password);
            users.Add(userData);
            File.AppendAllText("SavedUsers.txt", JsonConvert.SerializeObject(userData).ToString() + "\n");
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
        public  string login { get; set; }
        public  int token { get; set; }
        public  int messageID { get; set; }

        public DeleteMessageData() { }

        public DeleteMessageData(string _login, int _token, int _messageID)
        {
            login = _login;
            token = _token;
            messageID = _messageID;
        }
    }

}

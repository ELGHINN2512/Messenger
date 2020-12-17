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
        }

        public void Add(int token,string login, string password)
        {
            Session session = new Session(token, login, password);
            sessions.Add(session);
        }

        public int GenerateToken()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999);
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

}

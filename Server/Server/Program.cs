using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Controllers;
using System.Threading;
using System.Windows;



namespace Server
{

    public class Program
    {

        public static Messages AllMessages = new Messages();
        public static SessionClass AllSessions = new SessionClass();
        public static SessionClass AdminSessons = new SessionClass();
        public static Users Allusers = new Users();
        public static List<int> AllDeletedMessages = new List<int>();

        public static void Main(string[] args)
        {
            TimerCallback tm = new TimerCallback(OnlineControl);
            Timer timer = new Timer(tm, 1, 0, 20000);
            CreateHostBuilder(args).Build().Run();
        }

        public static void OnlineControl(object obj)
        {
            for(int i=0;i<AllSessions.sessions.Count;i++)
            {
                if(AllSessions.sessions[i].online == 0)
                {
                    AllMessages.Add("Server", 0, $"\t\t\tПользователь {AllSessions.sessions[i].login} покинул чат");
                    Console.WriteLine($"User {AllSessions.sessions[i].login} logged out.");
                    for(int j=0;j<AdminSessons.sessions.Count; j++)
                    {
                        if(AllSessions.sessions[i].login == AdminSessons.sessions[j].login)
                            AdminSessons.sessions.RemoveAt(j);
                    }
                    AllSessions.sessions.RemoveAt(i);
                    i--;
                    continue;
                }
                AllSessions.sessions[i].online = 0;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

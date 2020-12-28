using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Timers;
using ConsoleClient;
using Newtonsoft.Json;
using Server;
using Terminal.Gui;



namespace ConsoleClient
{



    class Program
    {
        private static MenuBar menu;
        private static Window winMain;
        private static Window winMessages;
        private static Label labelUsername;
        private static TextField fieldUsername;
        private static Label labelMessage;
        private static TextField fieldMessage;
        private static Button buttonSend;

        private static Messages allMessages = new Messages();
        static int choise;
        static string login;
        static string password;
        static int token;


        static void Main(string[] args)
        {

            Console.Write("Добро Пожаловать!\n1.Авторизация\n2.Регистрация\n");
            do
            {
                Console.Write("Ваш выбор: ");
                choise = Convert.ToInt32(Console.ReadLine());
            } while ((choise != 1) && (choise != 2));

            if (choise == 2)
            {
                Console.Clear();
                Console.WriteLine("РЕГИСТРАЦИЯ");

                do
                {
                    Console.Write("Логин: ");
                    login = Console.ReadLine();
                    Console.Write("Пароль:");
                    password = Console.ReadLine();
                    token = Registration(login, password);
                    Console.WriteLine("Попытка регистрации...");
                } while (token != 1);
                choise = 1;
            }

            if (choise == 1)
            {
                Console.Clear();
                Console.WriteLine("АВТОРИЗАЦИЯ");
                do
                {
                    Console.Write("Логин: ");
                    login = Console.ReadLine();
                    Console.Write("Пароль:");
                    password = Console.ReadLine();
                    token = Authorization(login, password);
                    Console.WriteLine("Попытка авторизации...");
                } while (token < 0);
            }
            Console.Clear();

            Application.Init();

            ColorScheme mainColor = new ColorScheme();
            mainColor.Normal = new Terminal.Gui.Attribute(Color.White,Color.Black);
            mainColor.Focus = new Terminal.Gui.Attribute(Color.White, Color.Black);
            mainColor.Disabled = new Terminal.Gui.Attribute(Color.White, Color.Black);
            mainColor.HotFocus = new Terminal.Gui.Attribute(Color.White, Color.Black);
            mainColor.HotNormal = new Terminal.Gui.Attribute(Color.White, Color.Black); 

            var top = Application.Top;
            menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("_App", new MenuItem[]
                {
                    new MenuItem("_Quit", "Close the app", Application.RequestStop),
                }),
            });
            top.Add(menu);

            // Компонент главной формы
            winMain = new Window()
            {
                // Позиция начала окна
                X = 0,
                Y = 1, //< Место под меню
                // Авто растягивание по размерам терминала
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = mainColor,
            };
            top.Add(winMain);

            // Компонент сообщений
            winMessages = new Window()
            {
                X = 0,
                Y = 0,
                Width = winMain.Width,
                Height = winMain.Height - 1,
                ColorScheme = mainColor,
            };
            winMain.Add(winMessages);


            labelMessage = new Label()
            {
                X = 8,
                Y = Pos.Bottom(winMain) - 4,
                Width = 15,
                Height = 1,
                Text = "Message:",
            };
            winMain.Add(labelMessage);

            fieldMessage = new TextField()
            {
                X = 18,
                Y = Pos.Bottom(winMain) - 4,
                Width = winMain.Width - 10,
                Height = 1,
            };
            winMain.Add(fieldMessage);

            buttonSend = new Button()
            {
                X = Pos.Right(winMain) - 11,
                Y = Pos.Bottom(winMain) - 4,
                Width = 5,
                Height = 1,
                Text = "SEND",
            };
            buttonSend.Clicked += ButtonSendClick;
            winMain.Add(buttonSend);

            int lastMsgID = 0;
            bool msgSent = true;
            System.Timers.Timer updateLoop = new System.Timers.Timer();
            updateLoop.Interval = 1000;
            updateLoop.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Message message = GetMessage(lastMsgID);
                if(message != null && msgSent == true)
                {
                    msgSent = false;
                    allMessages.Add(message);
                    MessageUpdate();
                    lastMsgID++;
                    msgSent = true;
                }
            };

            System.Timers.Timer updateOnline = new System.Timers.Timer();
            updateOnline.Interval = 10000;
            updateOnline.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                SendOnline(new Session(token, login));
            };

            updateOnline.Start();
            updateLoop.Start();

            Application.Run();

            static void ButtonSendClick()
            {
                if (fieldMessage.Text.Length != 0)
                {
                    Message message = new Message(login,token, fieldMessage.Text.ToString());
                    SendMassage(message);
                    fieldMessage.Text = "";
                }
            }

            static void MessageUpdate()
            {
                winMessages.RemoveAll();
                int offset = 0;
                for (var i = allMessages.messages.Count - 1; i>0; i-- )
                {
                    var message = allMessages.messages[i];
                    View msg = new View()
                    {
                        X = 0,
                        Y = offset,
                        Width = winMessages.Width,
                        Height = 1,
                        Text = $"[{message.time}] {message.username}: {message.text}",
                };
                    winMessages.Add(msg);
                    offset++;
                }

                Application.Refresh();
            }

            static void SendMassage(Message message)
            {
                WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/chat");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                string postData = JsonConvert.SerializeObject(message);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentLength = bytes.Length;
                Stream reqStream = httpWebRequest.GetRequestStream();
                reqStream.Write(bytes,0, bytes.Length);
                reqStream.Close();

                httpWebRequest.GetResponse();
            };

            static Message GetMessage(int id)
            {
                WebRequest httpWebRequest = WebRequest.Create($"http://localhost:5000/api/chat/{id}");
                WebResponse httpWebResponse = httpWebRequest.GetResponse();
                string smsg = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
                if (smsg == "not found") 
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<Message>(smsg);
            }

            int Authorization(string login, string password)
            {
                UserData userData = new UserData(login, password);
                WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/session");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                string postData = JsonConvert.SerializeObject(userData);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentLength = bytes.Length;
                Stream reqStream = httpWebRequest.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                int resp;
                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    resp = Convert.ToInt32(new StreamReader(response.GetResponseStream()).ReadToEnd());
                reqStream.Close();
                return resp;
            }

            static int Registration(string login, string password)
            {
                UserData userData = new UserData(login, password);
                WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/registration");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                string postData = JsonConvert.SerializeObject(userData);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentLength = bytes.Length;
                Stream reqStream = httpWebRequest.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                int resp;
                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                    resp = Convert.ToInt32(new StreamReader(response.GetResponseStream()).ReadToEnd());
                reqStream.Close();
                return resp;
            }

            static void SendOnline(Session session)
            {
                WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/online");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                string postData = JsonConvert.SerializeObject(session);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentLength = bytes.Length;
                Stream reqStream = httpWebRequest.GetRequestStream();
                reqStream.Write(bytes, 0, bytes.Length);
                reqStream.Close();

                httpWebRequest.GetResponse();
            }


        }
    }
}


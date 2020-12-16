using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Timers;
using ConsoleClient;
using Newtonsoft.Json;
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


        static void Main(string[] args)
        {
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
                Height = winMain.Height - 3,
                ColorScheme = mainColor,
            };
            winMain.Add(winMessages);

            // Текст "Пользователь"
            labelUsername = new Label()
            {
                X = 7,
                Y = Pos.Bottom(winMain)-6,
                Width = 17,
                Height = 1,
                Text = "Username:",
            };
            winMain.Add(labelUsername);

            fieldUsername = new TextField()
            {
                X = 18,
                Y = Pos.Bottom(winMain) - 6,
                Width = 30,
                Height = 1,
                ColorScheme = mainColor,
            };
            winMain.Add(fieldUsername);

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
            updateLoop.Interval = 100;
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
            updateLoop.Start();

            Application.Run();

            static void ButtonSendClick()
            {
                if (fieldMessage.Text.Length != 0 && fieldUsername.Text.Length != 0)
                {
                    Message message = new Message(fieldUsername.Text.ToString(), fieldMessage.Text.ToString());
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
        }
    }
}
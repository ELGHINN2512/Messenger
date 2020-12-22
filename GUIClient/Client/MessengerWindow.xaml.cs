using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MessengerWindow.xaml
    /// </summary>
    public partial class MessengerWindow : Window
    {
        int lastMsgID = 0;
        int lastMsgDeleteID = 0;
        public string login;
        public int token;
        static public bool flagCloseMainWindow = true;
        Messages AllMessages = new Messages();
        string StrAllMesages = "";
        bool ViewMessageID = false;


        public MessengerWindow()
        {
            DispatcherTimer timerMsg = new DispatcherTimer();
            timerMsg.Tick += new EventHandler(timer_Tick_Msg);
            timerMsg.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerMsg.Start();

            DispatcherTimer timerOnline = new DispatcherTimer();
            timerOnline.Tick += new EventHandler(timer_Tick_Online);
            timerOnline.Interval = new TimeSpan(0, 0, 0, 10, 0);
            timerOnline.Start();

            DispatcherTimer timerDeleteMsg = new DispatcherTimer();
            timerOnline.Tick += new EventHandler(timer_Tick_DeleteMsg);
            timerOnline.Interval = new TimeSpan(0, 0, 0, 5, 0);
            timerOnline.Start();

            InitializeComponent();
        }

        private void timer_Tick_DeleteMsg(object sender, EventArgs e)
        {
            int IDdeleteMsg = GetDeleteMessage(lastMsgDeleteID);
            {
                if(IDdeleteMsg != -1)
                {
                    string newStrAllMessages = "";
                    for(int i = 0; i<AllMessages.messages.Count; i++)
                    {
                        if(IDdeleteMsg == i)
                        {
                            Message message = GetMessage(IDdeleteMsg);
                            AllMessages.messages[i].text = message.text;
                            AllMessages.messages[i].username = message.username;
                            AllMessages.messages[i].token = message.token;
                        }
                        if (ViewMessageID == false)
                        {
                            if (AllMessages.messages[i].username == "Server")
                                newStrAllMessages = newStrAllMessages + $"\n {((AllMessages.messages[i].time.Hour) + 3) % 24}:{AllMessages.messages[i].time.Minute} {AllMessages.messages[i].text}\n";
                            else
                                newStrAllMessages = newStrAllMessages + $"\n {((AllMessages.messages[i].time.Hour) + 3) % 24}:{AllMessages.messages[i].time.Minute}     {AllMessages.messages[i].username}\n {AllMessages.messages[i].text}\n";
                        }
                        else
                        {
                            if (AllMessages.messages[i].username == "Server")
                                newStrAllMessages = newStrAllMessages + $"\n[ ID:{i} ] {((AllMessages.messages[i].time.Hour) + 3) % 24}:{AllMessages.messages[i].time.Minute} {AllMessages.messages[i].text}\n";
                            else
                                newStrAllMessages = newStrAllMessages + $"\n[ ID:{i} ] {((AllMessages.messages[i].time.Hour) + 3) % 24}:{AllMessages.messages[i].time.Minute}     {AllMessages.messages[i].username}\n {AllMessages.messages[i].text}\n";
                        }
                    }
                    StrAllMesages = newStrAllMessages;
                    chat.Text = StrAllMesages;
                    lastMsgDeleteID++;
                }
            }
        }

        private void timer_Tick_Online(object sender, EventArgs e)
        {
            SendOnline(new Session(token, login));
        }

        private void timer_Tick_Msg(object sender, EventArgs e)
        {
            Message msg = GetMessage(lastMsgID);
            if (msg != null)
            {
                AllMessages.Add(msg);
                lastMsgID++;
                if (ViewMessageID == false)
                {
                    if (msg.username == "Server")
                    {
                        StrAllMesages = StrAllMesages + $"\n {((msg.time.Hour) + 3) % 24}:{msg.time.Minute}\t {msg.text}\n";
                        chat.Text = StrAllMesages;
                    }
                    else
                    {
                        StrAllMesages = StrAllMesages + $"\n {((msg.time.Hour) + 3) % 24}:{msg.time.Minute}     {msg.username}\n {msg.text}\n";
                        chat.Text = StrAllMesages;
                    }
                }
                else
                {
                    if (msg.username == "Server")
                    {
                        StrAllMesages = StrAllMesages + $"\n[ ID:{lastMsgID - 1} ] {((msg.time.Hour) + 3) % 24}:{msg.time.Minute}\t {msg.text}\n";
                        chat.Text = StrAllMesages;
                    }
                    else
                    {
                        StrAllMesages = StrAllMesages + $"\n[ ID:{lastMsgID - 1} ] {((msg.time.Hour) + 3) % 24}:{msg.time.Minute}     {msg.username}\n {msg.text}\n";
                        chat.Text = StrAllMesages;
                    }
                }
            }
        }


        private void MessagerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (flagCloseMainWindow == true)
            {
                (Application.Current.MainWindow as MainWindow).Close();
            }
        }

        private void OpenPanel(object sender, RoutedEventArgs e)
        {
            ShowName.Text = login;
            LeftPanel.Visibility = Visibility.Visible;
            ButtonOpenPanel.Visibility = Visibility.Hidden;
            InputPanel.Margin = new Thickness(240, 0, 0, 0);
            ChatPannel.Margin = new Thickness(240, 1, 0, 55);
        }

        private void ClosePannel(object sender, RoutedEventArgs e)
        {
            LeftPanel.Visibility = Visibility.Hidden;
            ButtonOpenPanel.Visibility = Visibility.Visible;
            if (SettingPanel.Visibility == Visibility.Visible)
            {
                SettingPanel.Visibility = Visibility.Hidden;
                AdminPanel.Visibility = Visibility.Hidden;
            }
            InputPanel.Margin = new Thickness(0, 0, 0, 0);
            ChatPannel.Margin = new Thickness(24, 1, 0, 55);
        }


        static void SendMessage(Message message)
        {
            WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/chat");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            string postData = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = bytes.Length;
            Stream reqStream = httpWebRequest.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);
            reqStream.Close();

            httpWebRequest.GetResponse();
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

        static void SendDeleteMessage(DeleteMessageData deleteMessageData)
        {
            WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/DeleteMessage");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            string postData = JsonConvert.SerializeObject(deleteMessageData);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = bytes.Length;
            Stream reqStream = httpWebRequest.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);
            reqStream.Close();

            httpWebRequest.GetResponse();
        }

        static int SendAdminPassword(UserData userData)
        {
            WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/Admin");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            string postData = JsonConvert.SerializeObject(userData);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = bytes.Length;
            Stream reqStream = httpWebRequest.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);
            reqStream.Close();

            using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                 return Convert.ToInt32(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }



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

        static int GetDeleteMessage(int id)
        {
            WebRequest httpWebRequest = WebRequest.Create($"http://localhost:5000/api/DeleteMessage/{id}");
            WebResponse httpWebResponse = httpWebRequest.GetResponse();
            string smsg = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
            int IDdeleteMsg = Convert.ToInt32(smsg);
            return IDdeleteMsg;
        }


        private void ChoiceEmoji(object sender, RoutedEventArgs e)
        {
            MessageBox.Text = MessageBox.Text + (sender as Button).Content;
        }

        private void ClickEmoji(object sender, RoutedEventArgs e)
        {
            if (emoji.Visibility == Visibility.Hidden)
                emoji.Visibility = Visibility.Visible;
            else
                emoji.Visibility = Visibility.Hidden;
        }

        private void ClickAdminPanelButton(object sender, RoutedEventArgs e)
        {
            if (AdminPanel.Visibility == Visibility.Visible)
                AdminPanel.Visibility = Visibility.Hidden;
            else
                AdminPanel.Visibility = Visibility.Visible;
        }

        private void Click_SendAdminPassword(object sender, RoutedEventArgs e)
        {
            if(SendAdminPassword(new UserData(login, AdminBox.Password)) == 1)
            {
                ViewMessageID = true;
                lastMsgID = 0;
                lastMsgDeleteID = 0;
                StrAllMesages = "";
            }
        }

        private void ClickSendMessage(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text.Contains("/delete"))
            {
                int MessageID;
                int.TryParse(string.Join("", MessageBox.Text.Where(c => char.IsDigit(c))), out MessageID);
                DeleteMessageData deleteMessageData = new DeleteMessageData(login, token, MessageID);
                SendDeleteMessage(deleteMessageData);
                MessageBox.Text = "";
            }
            else
            {
                SendMessage(new Message(login, token, MessageBox.Text));
                MessageBox.Text = "";
            }

        }

        private void ClickButtonExit(object sender, RoutedEventArgs e)
        {
            flagCloseMainWindow = false;
            login = "";
            token = 0;
            Close();
            (Application.Current.MainWindow as MainWindow).Show();
            flagCloseMainWindow = true;
        }

        private void ClickButtonSettings(object sender, RoutedEventArgs e)
        {
            if (SettingPanel.Visibility == Visibility.Hidden)
                SettingPanel.Visibility = Visibility.Visible;
            else
            {
                SettingPanel.Visibility = Visibility.Hidden;
                AdminPanel.Visibility = Visibility.Hidden;
            }
        }

        private void MessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
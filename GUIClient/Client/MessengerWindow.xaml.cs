using Newtonsoft.Json;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string login;
        public int token;
        static public bool flagCloseMainWindow = true;

        public MessengerWindow()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Message msg = GetMessage(lastMsgID);
            if (msg != null)
            {
                chat.Text = chat.Text + $"\n {((msg.time.Hour)+3)%24}:{msg.time.Minute} | \t{msg.username}\n {msg.text}\n";
                lastMsgID++;
            }
        }
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            SendMassage(new Message(login, token, MessageBox.Text));
            MessageBox.Text = "";
        }
        private void MessagerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (flagCloseMainWindow == true)
            {
                (Application.Current.MainWindow as MainWindow).Close();
            }
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
            reqStream.Write(bytes, 0, bytes.Length);
            reqStream.Close();

            httpWebRequest.GetResponse();
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

        private void OpenPanel(object sender, RoutedEventArgs e)
        {
            ShowName.Text = login;
            LeftPanel.Visibility = Visibility.Visible;
            ButtonOpenPanel.Visibility = Visibility.Hidden;
            InputPanel.Margin = new Thickness(240,0,0,0);
            ChatPannel.Margin = new Thickness(240, 1, 0, 55);
        }

        private void ClosePannel(object sender, RoutedEventArgs e)
        {
            LeftPanel.Visibility = Visibility.Hidden;
            ButtonOpenPanel.Visibility = Visibility.Visible;
            InputPanel.Margin = new Thickness(0, 0, 0, 0);
            ChatPannel.Margin = new Thickness(24, 1, 0, 55);
        }

        private void ClickButtonExit(object sender, RoutedEventArgs e)
        {
            flagCloseMainWindow = false;
            this.Close();
            (Application.Current.MainWindow as MainWindow).Show();
            flagCloseMainWindow = true;
        }

        private void ClickButtonSettings(object sender, RoutedEventArgs e)
        {

        }

        private void MessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ClickEmoji(object sender, RoutedEventArgs e)
        {
            if (emoji.Visibility == Visibility.Hidden)
                emoji.Visibility = Visibility.Visible;
            else
                emoji.Visibility = Visibility.Hidden;
        }

        private void ChoiceEmoji(object sender, RoutedEventArgs e)
        {
            MessageBox.Text = MessageBox.Text + (sender as Button).Content;
        }
    }
}

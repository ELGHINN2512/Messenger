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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pass1.Password != pass2.Password)
            {
                warn.Visibility = Visibility.Visible;
            }
            else
            {
                Session session = new Session(login.Text,pass1.Password);
                Registration(session);
                warn.Visibility = Visibility.Hidden;
                (Application.Current.MainWindow as MainWindow).transitionToAuthorization();
            }
        }

        private void Button_BackToAuthorization(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).transitionToAuthorization();
        }

        static void Registration(Session session)
        {
            WebRequest httpWebRequest = WebRequest.Create("http://localhost:5000/api/session");
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

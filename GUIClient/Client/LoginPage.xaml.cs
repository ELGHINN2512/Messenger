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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public void Button_Autorization(object sender, RoutedEventArgs e)
        {
            int token = Authorization(login.Text, password.Password);
            if (token == -1)
            {
                warnWrongPassword.Visibility = Visibility.Hidden;
                warnUserNotFound.Visibility = Visibility.Visible;
                password.Password = "";
                return;
            }
            if (token == -2)
            {
                warnUserNotFound.Visibility = Visibility.Hidden;
                warnWrongPassword.Visibility = Visibility.Visible;
                password.Password = "";
                return;
            }
            warnUserNotFound.Visibility = Visibility.Hidden;
            warnWrongPassword.Visibility = Visibility.Hidden;
            password.Password = "";
            MainWindow.token = token;
            (Application.Current.MainWindow as MainWindow).transitionMessanger();
        }

        private void Button_Registration(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).transitionToRegistration();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        static int Authorization(string login, string password)
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
    }
}

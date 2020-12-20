using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginPage loginPage = new LoginPage();
        RegistrationPage registrationPage = new RegistrationPage();

        public MainWindow()
        {
            InitializeComponent();

            transitionToAuthorization();
        }

        public void frame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        public void transitionToAuthorization()
        {
            frame.Height = 342;
            frame.Width = 366;
            MinHeight = frame.Height;
            MinWidth = frame.Width;
            frame.Navigate(loginPage);
        }

        public void transitionToRegistration()
        {
            frame.Height = 417;
            frame.Width = 366;
            MinHeight = frame.Height;
            MinWidth = frame.Width;
            frame.Navigate(registrationPage);
        }
        
        public void transitionMessanger(string _login, int _token)
        {
            MessengerWindow messengerWindow = new MessengerWindow();
            messengerWindow.login = _login;
            messengerWindow.token = _token;
            messengerWindow.Show();
            Hide();
        }


        private void Button_Registration(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}

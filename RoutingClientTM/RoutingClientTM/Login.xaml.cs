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
using System.Windows.Shapes;

namespace RoutingClientTM
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }


        private void LoginBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            loginImg.Height = 35;
            loginImg.Width = 35;
        }

        private void LoginBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            loginImg.Height = 40;
            loginImg.Width = 40;
        }

        private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            closeBtn.Height = 27;
            closeBtn.Width = 27;
        }

        private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            closeBtn.Height = 30;
            closeBtn.Width = 30;
        }

        private void CloseBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }

        private void LoginBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/" + usernameTxt.Text + "/" + passTxt.Password;
            string response = restClient.makeRequest();

            if (response == "true")
            {
                AdminGui gui = new AdminGui();
                gui.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("user not found");
            }
        }

        private void RectMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutingClientTM.Classes;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoutingClientTM
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
        }

        string response;
        #region objects initialised       
        AdminGui Window;
        List<User> users = new List<User>();
        List<Job> jobs = new List<Job>();
        #endregion

        public Users(AdminGui window)
        {
            InitializeComponent();
            Window = window;
            ControlsOff();
            ListUsers();
            userJobsDatagrid.ItemsSource = jobs;
        }
       
        public void ControlsOff()
        {
            this.userIdTxt.IsEnabled = false;
            this.userTypeBox.IsEnabled = false;
            this.driverTypeBox.IsEnabled = false;
            this.licenseTypeBox.IsEnabled = false;
            this.cpcTypeBox.IsEnabled = false;
            this.fNameTxt.IsEnabled = false;
            this.sNameTxt.IsEnabled = false;
            this.emailTxt.IsEnabled = false;
        }

        public void ControlsOn()
        {
            this.userTypeBox.IsEnabled = true;
            this.driverTypeBox.IsEnabled = true;
            this.licenseTypeBox.IsEnabled = true;
            this.cpcTypeBox.IsEnabled = true;
            this.fNameTxt.IsEnabled = true;
            this.sNameTxt.IsEnabled = true;
            this.emailTxt.IsEnabled = true;
        }

        #region  changes state of close image
        private void closeBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Image uie = closeBtn;
            uie.Effect =
            new DropShadowEffect
            {
                Color = new Color { A = 0, R = 0, G = 0, B = 0 },
                Direction = 225,
                ShadowDepth = 5,
                BlurRadius = 11
            };
        }

        private void closeBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Image uie = closeBtn;
            uie.Effect =
            new DropShadowEffect
            {
                Color = new Color { A = 0, R = 0, G = 0, B = 0 },
                Direction = 225,
                ShadowDepth = 5,
                BlurRadius = 14
            };
        }
        #endregion

        private void closeBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            #region resets controls on user page and admin gui
            var selPage = Window.categoryViewer.NavigationService.Content.GetType().Name.ToString();

            //resets window controls if page cancel btn clicked
            if (selPage.ToString() == "Users")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close? all unsaved changes will be lost", "cancel shift", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ControlsOff();
                    //newBtn
                    Window.newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
                    Window.newSave.Text = "New";
                    //editBtn
                    Window.editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
                    Window.editTxt.Text = "Edit";
                    //deleteBtn
                    Window.deleteImg.Source = new BitmapImage(new Uri("Images/delete.png", UriKind.Relative));
                    Window.deleteTxt.Text = "Delete";
                    //refreshBtn
                    Window.refreshImg.Source = new BitmapImage(new Uri("Images/refresh.png", UriKind.Relative));
                    Window.refreshTxt.Text = "Refresh";
                    //resets the counter
                    Window.counter = 0;

                    //resets buttons in window
                    Window.editBtn.Visibility = Visibility.Visible;
                    Window.deleteBtn.Visibility = Visibility.Visible;
                    Window.refreshBtn.Visibility = Visibility.Visible;
                    Window.printBtn.Visibility = Visibility.Visible;

                    //clears Txtboxes
                    driverTypeBox.SelectedIndex = 0;
                    cpcTypeBox.SelectedIndex = 0;
                    licenseTypeBox.SelectedIndex = 0;
                    userTypeBox.SelectedIndex = 0;
                    sNameTxt.Clear();
                    fNameTxt.Clear();
                    userIdTxt.Clear();
                }
            }
            #endregion
        }

        public void ListUsers()
        {
            //calls rest api and processes the Json in user objects to display in listbox 
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/allusers";
            response = restClient.makeRequest();         

            try
            {
                JArray a = JArray.Parse(response);

                foreach (var obj in a)
                {
                    User usr = JsonConvert.DeserializeObject<User>(obj.ToString());
                    users.Add(usr);
                    userList.ItemsSource = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            jobs.Clear();
            //removes selected user from listbox casts back to User class and add details to users page controls
            User user = (User)userList.SelectedItem;
            userIdTxt.Text = user.user_id;
            userTypeBox.SelectedIndex = int.Parse(user.user_type);
            driverTypeBox.SelectedIndex = int.Parse(user.driver_type);
            cpcTypeBox.SelectedIndex = int.Parse(user.cpc_type);
            licenseTypeBox.SelectedIndex = int.Parse(user.license_type);
            fNameTxt.Text = user.first_name;
            sNameTxt.Text = user.second_name;
            emailTxt.Text = user.e_mail;

            //add job info logic
            //calls rest api and processes the Json in user objects to display in listbox 
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/jobs";
            restClient.postJson = user.user_id;
            restClient.httpMethod = httpVerb.POST;
            string response = restClient.makeRequest();


            try
            {
                JArray a = JArray.Parse(response);

                if (a.Count == 0)
                {
                    CollectionViewSource.GetDefaultView(userJobsDatagrid.ItemsSource).Refresh();
                }
                else
                {
                    foreach (var obj in a)
                    {
                        Job job = JsonConvert.DeserializeObject<Job>(obj.ToString());
                        jobs.Add(job);
                        CollectionViewSource.GetDefaultView(userJobsDatagrid.ItemsSource).Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

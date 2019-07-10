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
    /// Interaction logic for BusP.xaml
    /// </summary>
    public partial class BusP : Page
    {

        List<Job> jobs = new List<Job>();
        string response;
        List<Bus> buses = new List<Bus>();
        AdminGui adminGui;
        public BusP(AdminGui AdminGui)
        {
            InitializeComponent();
            ControlsOff();
            ListBuses();
            busJobDataGrid.ItemsSource = jobs;
            adminGui = AdminGui;
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
            var selPage = adminGui.categoryViewer.NavigationService.Content.GetType().Name.ToString();

            //resets window controls if page cancel btn clicked
            if (selPage.ToString() == "BusP")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to close? all unsaved changes will be lost", "cancel shift", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ControlsOff();
                    //newBtn
                    adminGui.newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
                    adminGui.newSave.Text = "New";
                    //editBtn
                    adminGui.editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
                    adminGui.editTxt.Text = "Edit";
                    //deleteBtn
                    adminGui.deleteImg.Source = new BitmapImage(new Uri("Images/delete.png", UriKind.Relative));
                    adminGui.deleteTxt.Text = "Delete";
                    //refreshBtn
                    adminGui.refreshImg.Source = new BitmapImage(new Uri("Images/refresh.png", UriKind.Relative));
                    adminGui.refreshTxt.Text = "Refresh";
                    //resets the counter
                    adminGui.counter = 0;

                    //resets buttons in window
                    adminGui.editBtn.Visibility = Visibility.Visible;
                    adminGui.deleteBtn.Visibility = Visibility.Visible;
                    adminGui.refreshBtn.Visibility = Visibility.Visible;
                    adminGui.printBtn.Visibility = Visibility.Visible;

                    //clears Txtboxes
                    busTypeBox.SelectedIndex = 0;
                    tachoTypeBox.SelectedIndex = 0;
                    heightTxt.Clear();
                    widthTxt.Clear();
                    weightTxt.Clear();
                    modelTypeBox.Clear();
                    yearTxt.Clear();
                    capacityTxt.Clear();
                }
            }
            #endregion
        }

        public void ListBuses()
        {
            //calls rest api and processes the Json in user objects to display in listbox 
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/allbuses";
            response = restClient.makeRequest();

            try
            {
                JArray a = JArray.Parse(response);

                foreach (var obj in a)
                {
                    Bus bus = JsonConvert.DeserializeObject<Bus>(obj.ToString());
                    buses.Add(bus);
                    busList.ItemsSource = buses;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void BusList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            jobs.Clear();

            //removes selected user from listbox casts back to User class and add details to users page controls
            Bus bus = (Bus)busList.SelectedItem;
            regTxt.Text = bus.bus_reg;
            modelTypeBox.Text = bus.model;
            yearTxt.Text = bus.year;
            capacityTxt.Text = bus.bus_capacity;
            weightTxt.Text = bus.bus_weight;
            heightTxt.Text = bus.bus_height;
            widthTxt.Text = bus.bus_width;
            busTypeBox.SelectedIndex = int.Parse(bus.bus_type);
            tachoTypeBox.SelectedIndex = int.Parse(bus.tacho);

            //add job info logic
            //calls rest api and processes the Json in user objects to display in listbox 
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/jobs/bus/"+bus.bus_reg;
            string response = restClient.makeRequest();

            
            try
            {
                JArray a = JArray.Parse(response);

                if(a.Count == 0)
                {

                    CollectionViewSource.GetDefaultView(busJobDataGrid.ItemsSource).Refresh();

                }
                else
                {
                    foreach (var obj in a)
                    {

                        Job job = JsonConvert.DeserializeObject<Job>(obj.ToString());
                        jobs.Add(job);
                        CollectionViewSource.GetDefaultView(busJobDataGrid.ItemsSource).Refresh();

                    }
                }              
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ControlsOn()
        {
            regTxt.IsEnabled = true;
            modelTypeBox.IsEnabled = true;
            yearTxt.IsEnabled = true;
            capacityTxt.IsEnabled = true;
            weightTxt.IsEnabled = true;
            heightTxt.IsEnabled = true;
            widthTxt.IsEnabled = true;
            busTypeBox.IsEnabled = true;
            tachoTypeBox.IsEnabled = true;
        }
        public void ControlsOff()
        {
            regTxt.IsEnabled = false;
            modelTypeBox.IsEnabled = false;
            yearTxt.IsEnabled = false;
            capacityTxt.IsEnabled = false;
            heightTxt.IsEnabled = false;
            weightTxt.IsEnabled = false;
            widthTxt.IsEnabled = false;
            busTypeBox.IsEnabled = false;
            tachoTypeBox.IsEnabled = false;
        }
        public void Clear()
        {
            regTxt.Text = "";
            modelTypeBox.Text = "";
            yearTxt.Text = "";
            capacityTxt.Text = "";
            weightTxt.Text = "";
            heightTxt.Text = "";
            widthTxt.Text = "";
            busTypeBox.SelectedIndex = 0;
            tachoTypeBox.SelectedIndex = 0;
        }

       
    }
}

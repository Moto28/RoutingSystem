
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutingClientTM.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace RoutingClientTM
{
    /// <summary>
    /// Interaction logic for Shifts.xaml
    /// </summary>
    public partial class Shifts : Page
    {
        AdminGui adminGui;
        List<User> users = new List<User>();
        List<Job> jobs = new List<Job>();
        string response;
        public Bus bus;
        public Bus busT;
        public Job job;
        public Job tempJob;
        public DriverC driver;
        public DriverC driverT;
        public JRoute jRoute;
        public string jRouteString;
        public MJRoute mjRoute;
        public MJRoute tempRoute;
        public string mjRouteString;
        public JGeoCode start;
        public JGeoCode end;
        public TimeSpan time;
        public List<JGeoCode> cordsAddress;
        public string busTachoType;
        public string routeRes;
        string responseBus;
        string responseDriver;
        public string responseJob;
        public int passNum;
        public string datePicked;

        public Shifts(AdminGui AdminGui)
        {
            InitializeComponent();
            adminGui = AdminGui;
            ControlsOff();
        }       

        private void DriverList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeComponent();
        }

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

        public void ControlsOff()
        {
            busSelectionImg.IsEnabled = false;
            driverSelectionImg.IsEnabled = false;
            routeSelectionImg.IsEnabled = false;
        }
        
        public void ControlsOn()
        {
            busSelectionImg.IsEnabled = true;
            driverSelectionImg.IsEnabled = true;
            routeSelectionImg.IsEnabled = true;
        }

        public void Clear()
        {
            busTxt.Text = "No Bus Selected";
            driverTxt.Text = "No Driver Selected";
            routeTxt.Text = "No Route Selected ";
        }

        private void closeBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Shifts shifts;
            adminGui.categoryViewer.Navigate(shifts = new Shifts(adminGui));
            shifts.ControlsOff();
            adminGui.newBtn.Visibility = Visibility.Visible;
            adminGui.editBtn.Visibility = Visibility.Visible;
            adminGui.deleteBtn.Visibility = Visibility.Visible;
            adminGui.refreshBtn.Visibility = Visibility.Visible;
            adminGui.printBtn.Visibility = Visibility.Visible;
            adminGui.counter = 0;
            adminGui.editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
            adminGui.newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
            adminGui.newSave.Text = "New";
            adminGui.editTxt.Text = "Edit";            

        }

        private void DriverSelection_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //checks if user has selected a bus to job
            if( busTxt.Text == "Bus Selected")
            {
                Driver windowN = new Driver(this,adminGui);
                windowN.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to select a driver, you must select a bus first.");
            }
        }

        private void DriverSelection_MouseEnter(object sender, MouseEventArgs e)
        {
            driverSelectionImg.Height = 37;
            driverSelectionImg.Width = 27;
        }

        private void DriverSelection_MouseLeave(object sender, MouseEventArgs e)
        {
            driverSelectionImg.Height = 40;
            driverSelectionImg.Width = 30;
        }

        private void BusSelection_MouseEnter(object sender, MouseEventArgs e)
        {
            busSelectionImg.Height = 37;
            busSelectionImg.Height = 27;
        }

        private void BusSelection_MouseLeave(object sender, MouseEventArgs e)
        {
            busSelectionImg.Height = 40;
            busSelectionImg.Height = 30;
        }

        private void RouteSelection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //checks if user has selected driver and bus
            if (busTxt.Text == "Bus Selected" && driverTxt.Text == "Driver Selected")
            {
                MapWindow window = new MapWindow(this,adminGui);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to select a route, you must select a bus and driver first.");
            }          
        }

        private void RouteSelection_MouseEnter(object sender, MouseEventArgs e)
        {
            routeSelectionImg.Height = 37;
            routeSelectionImg.Width = 27;
        }

        private void RouteSelection_MouseLeave(object sender, MouseEventArgs e)
        {
            routeSelectionImg.Height = 40;
            routeSelectionImg.Width = 30;
        }

        private void BusSelection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Buses window = new Buses(this,adminGui);
            window.ShowDialog();
        }

        private void BusTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //shows bus information
            try
            {
                busTxtBlk.Text = "\n\n Registration: " + bus.bus_reg + "\n" + " Model: "+bus.model+" \n"+" Capacity: " + bus.bus_capacity +"\n Type: "
                    + bus.bus_type;
            }
            catch(Exception ex)
            {
                ex.ToString();
            }            
        }

        private void DriverTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //shows driver information
            try
            {
                driverTxtBlk.Text = "\n\n User ID: " + driver.user_id + "\n Name: " + driver.first_name + " " + driver.second_name + "\n" + " Driver Type: "
                    + driver.driver_type + "\n" + " CPC Type: " + driver.cpc_type;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void RouteTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //shows route information
            try
            {
                //converts units for km's to mile's
                double kDistance = mjRoute.resourceSets[0].resources[0].travelDistance / 1.609344;
                double mDistance = Math.Round(kDistance, 2);
                time = TimeSpan.FromSeconds(mjRoute.resourceSets[0].resources[0].travelDuration);
                start = cordsAddress[0];
                end = cordsAddress[cordsAddress.Count - 1];

                string startResult = start.resourceSets[0].resources[0].address.formattedAddress;
                string endResult = end.resourceSets[0].resources[0].address.formattedAddress;

                string[] startWords = startResult.Split(',');
                string[] endWords = endResult.Split(',');
                var formattedStartAddress = new System.Text.StringBuilder();
                var formattedEndAddress = new System.Text.StringBuilder();
                //converts start formatted address into atomic elements
                for (int i = 1; i < startWords.Length; i++)
                {
                    formattedStartAddress.AppendLine(startWords[i]);
                }
                formattedStartAddress.AppendLine(startWords[0]);

                //converts end formatted address into atomic elements
                for (int i = 1; i < endWords.Length; i++)
                {
                    formattedEndAddress.AppendLine(endWords[i]);
                }
                formattedEndAddress.AppendLine(endWords[0]);

                startTxtBlk.Text = "\n\n\n\n\n\n" + formattedStartAddress.ToString();

                endTxtBlk.Text = "\n\n\n\n\n\n" + formattedEndAddress.ToString();

                summaryTxtBlk.Text = "\n\n\n\nDistance: " + mDistance + "miles " + "                Estimated Duration: " + time;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
          
        }

        private void JobInfoDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //add job info logic
            //calls rest api and processes the Json in user objects to display in listbox 
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/job/all";
            response = restClient.makeRequest();

            try
            {
                JArray a = JArray.Parse(response);

                foreach (var obj in a)
                {
                    Job job = JsonConvert.DeserializeObject<Job>(obj.ToString());
                    jobs.Add(job);
                   
                }
                jobInfoDataGrid.ItemsSource = jobs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
       

        private void JobInfoDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tempJob = (Job)jobInfoDataGrid.SelectedItem;

            try
            {
                if (tempJob.job_id != "")
                {
                    MessageBoxResult result = MessageBox.Show("Driver: " + tempJob.driver_id + " " + tempJob.bus_reg + " Selected" + ", click yes to continue", "Confirm Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        //creates bus object to display on screen
                        RestClient client = new RestClient();
                        client.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/bus/" + tempJob.bus_reg;
                        responseBus = client.makeRequest();
                        busT = JsonConvert.DeserializeObject<Bus>(responseBus.ToString());
                        busTxtBlk.Text = "\n\n Registration: " + busT.bus_reg.ToUpper() + "\n" + " Model: " + busT.model + " \n" + " Capacity: " + busT.bus_capacity + "\n Type: " + busT.bus_type;

                        //creates driver object to display on screen
                        client.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/get/driver/" + tempJob.driver_id;
                        responseDriver = client.makeRequest();
                        driverT = JsonConvert.DeserializeObject<DriverC>(responseDriver.ToString());
                        driverTxtBlk.Text = "\n\n User ID: " + driverT.user_id + "\n Name: " + driverT.first_name + " " + driverT.second_name + "\n" + " Driver Type: " + driverT.driver_type + "\n" + " CPC Type: " + driverT.cpc_type;

                        ///////////////////////////////////////////////////////////////                     
                        string[] startWords = tempJob.start_location.Split(',');
                        string[] endWords = tempJob.end_location.Split(',');
                        var formattedStartAddress = new System.Text.StringBuilder();
                        var formattedEndAddress = new System.Text.StringBuilder();
                        //converts start formatted address into atomic elements
                        for (int i = 1; i < startWords.Length; i++)
                        {
                            formattedStartAddress.AppendLine(startWords[i]);
                        }
                        formattedStartAddress.AppendLine(startWords[0]);

                        //converts end formatted address into atomic elements
                        for (int i = 1; i < endWords.Length; i++)
                        {
                            formattedEndAddress.AppendLine(endWords[i]);
                        }
                        formattedEndAddress.AppendLine(endWords[0]);

                        startTxtBlk.Text = "\n\n\n\n\n\n" + formattedStartAddress.ToString();

                        endTxtBlk.Text = "\n\n\n\n\n\n" + formattedEndAddress.ToString();

                        //summaryTxtBlk.Text = "\n\n\n\nDistance: " + mDistance + "miles " + "                Estimated Duration: " + time;

                        busTxt.Text = "Bus Selected";
                        driverTxt.Text = "Driver Selected";
                        routeTxt.Text = "Driver Selected"; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("To continue, a job must be selected", "Selection Error");
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// Interaction logic for Driver.xaml
    /// </summary>
    public partial class Driver : Window
    {
        string response;
        List<DriverC> drivers = new List<DriverC>();
        Shifts shift;
        DriverC tempDriver;
        AdminGui window;
        public Driver(Shifts Shift,AdminGui Window)
        {
            InitializeComponent();
            shift = Shift;
            window = Window;
        }

        private void DriverList_Loaded(object sender, RoutedEventArgs e)
        {
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/all/drivers";
             response = restClient.makeRequest();            

            if(window.editTxt.Text == "Save")
            {
                try
                {
                    JArray a = JArray.Parse(response);

                    foreach (var obj in a)
                    {
                        DriverC driver = JsonConvert.DeserializeObject<DriverC>(obj.ToString());
                        if(driver.cpc_type == "Analogue" && shift.busT.tacho == "Digital")
                        {

                        }
                        else
                        {
                            drivers.Add(driver);
                        }

                    }
                    driverList.ItemsSource = drivers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                try
                {
                    JArray a = JArray.Parse(response);

                    foreach (var obj in a)
                    {
                        DriverC driver = JsonConvert.DeserializeObject<DriverC>(obj.ToString());
                        if (shift.busTachoType == "Digital")
                        {
                            if (driver.cpc_type == "Analogue")
                            {

                            }
                            else
                            {
                                drivers.Add(driver);
                            }
                        }
                        else
                        {
                            drivers.Add(driver);
                        }

                    }
                    driverList.ItemsSource = drivers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
           
        }

        private void DriverBtn_Click(object sender, RoutedEventArgs e)
        {
            tempDriver = (DriverC)driverList.SelectedItem;
            shift.driver = tempDriver;

            try
            {
                if (shift.driver.user_id != "")
                {
                    MessageBoxResult result = MessageBox.Show("Driver: " + shift.driver.first_name +" " + shift.driver.second_name + " Selected"+", click yes to continue", "Confirm Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        shift.driverTxt.Text = "Driver Selected";
                        //shows bus information
                        //shows driver information
                        try
                        {
                            shift.driverTxtBlk.Text = "\n\n User ID: " + shift.driver.user_id + "\n Name: " + shift.driver.first_name + " " + shift.driver.second_name + "\n" + " Driver Type: "
                                + shift.driver.driver_type + "\n" + " CPC Type: " + shift.driver.cpc_type;
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("To continue, a driver must be selected", "Selection Error");
            }
        }
    }
}

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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoutingClientTM
{
    /// <summary>
    /// Interaction logic for Buses.xaml
    /// </summary>
    public partial class Buses : Window
    {
        string response;
        List<Bus> buses = new List<Bus>();
        List<Bus> sortedBuses = new List<Bus>();
        public Shifts shift;
        AdminGui window;
        public int passNum;

        public Buses()
        {
        }
        public Buses(Shifts Shift, AdminGui Window)
        {            
            InitializeComponent();           
            shift = Shift;
            window = Window;
            DialogInput pass = new DialogInput(this);
            pass.ShowDialog();
        }     

        public void PopulateDataGrid()
        {
            RestClient restClient = new RestClient();
            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/allbuses/convert";
            response = restClient.makeRequest();            

            if(window.editTxt.Text == "Save")
            {
                try
                {
                   
                    JArray a = JArray.Parse(response);

                    foreach (var obj in a)
                    {
                        Bus bus = JsonConvert.DeserializeObject<Bus>(obj.ToString());
                        int cap = int.Parse(bus.bus_capacity);

                        if(shift.driverT.license_type == "Full" && (shift.busT.bus_type == "Double Deck" 
                            || shift.busT.bus_type == "Single Deck"|| shift.busT.bus_type == "Coach"|| shift.busT.bus_type == "Mini bus"))
                        {
                            if (shift.driverT.cpc_type == "Analogue" && bus.tacho == "Digital")
                            {
                                if (shift.driver.cpc_type == "Digital" && bus.tacho == "Digital")

                                    if (cap >= passNum)
                                    {
                                        buses.Add(bus);
                                    }
                                    else
                                    {

                                    }

                            }
                            else
                            {

                                if (cap >= passNum)
                                {
                                    buses.Add(bus);
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {

                        }
                        
                       
                    }
                    busList.ItemsSource = buses;
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
                        Bus bus = JsonConvert.DeserializeObject<Bus>(obj.ToString());
                        int cap = int.Parse(bus.bus_capacity);

                        if (cap >= shift.passNum)
                        {
                            buses.Add(bus);
                        }
                        else
                        {

                        }
                    }
                    busList.ItemsSource = buses;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }      

        private void BusBtn_Click(object sender, RoutedEventArgs e)
        {
            //checks selected bus and shows user message for them to confirm selection
            Bus tempBus = (Bus)busList.SelectedItem;
            shift.bus = tempBus;       
             
            try
            {
                if (shift.bus.bus_reg != "")
                {
                    MessageBoxResult result = MessageBox.Show("MODEL: "+shift.bus.model +" REG: "+shift.bus.bus_reg+", click yes to continue", "Confirm Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        shift.busTxt.Text = "Bus Selected";
                        shift.busTachoType = tempBus.tacho;
                        shift.passNum = passNum;
                        Close();                        
                    }
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show("To continue, a bus must be selected", "Selection Error");
            }           
        }

        private void BusList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //formats the datagrid view
                busList.RowHeight = 30;
                //reg
                busList.Columns[0].Header = "Registration";
                busList.Columns[0].Width = 80;
                //model
                busList.Columns[1].Header = "Model";
                busList.Columns[1].Width = 160;
                //year
                busList.Columns[2].Header = "Year";
                busList.Columns[2].Width = 40;
                //capacity
                busList.Columns[3].Header = "Capacity";
                busList.Columns[3].Width = 60;
                //type
                busList.Columns[4].Header = "Type";
                busList.Columns[4].Width = 80;
                //width
                busList.Columns[5].Header = "Width(M)";
                busList.Columns[5].Width = 60;
                //height
                busList.Columns[6].Header = "Height(M)";
                busList.Columns[6].Width = 70;
                //weight
                busList.Columns[7].Header = "Weight(Tonne)";
                busList.Columns[7].Width = 90;
                //tacho
                busList.Columns[8].Header = "Tacho";
                busList.Columns[8].Width = 79;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Connection to the server could not be established" + " "+ex.ToString());
            }
           
        }       


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateDataGrid();
        }

       
    }
}


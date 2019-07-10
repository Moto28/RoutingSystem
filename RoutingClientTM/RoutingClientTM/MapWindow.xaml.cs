
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using Microsoft.Maps.MapControl.WPF.Design;
using Microsoft.Maps.MapControl.WPF.Overlays;

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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoutingClientTM
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        Queue<string> GeoCords = new Queue<string>();
        List<JGeoCode> cordsAddress = new List<JGeoCode>();

        string[] waypoint = new string[25];
        string hTemp;
        string mTemp;
        int hcounter = 0;
        int mcounter = 0;
        double lat;
        double lng;
        int I;
        bool calc = false;
        bool edit = false;
        LocationCollection coll;
        int counter = 0;       
        Shifts shift;
        JRoute code;
        TimeSpan time;
        double kDistance;
        double mDistance;
        AdminGui window;
        string response;
       

        public MapWindow()
        {            
           
        }

        public MapWindow(Shifts Shift, AdminGui Window)
        {
            InitializeComponent();
            shift = Shift;
            window = Window;
        }
              
        private void  CalcRouteBtn_Click(object sender, RoutedEventArgs e)
        {
            //takes each geo location taken from array and querys HERE API for route
            foreach (var cord in GeoCords)
            {
                hTemp += "&waypoint." + hcounter + "=" + cord;
                hcounter++;
            }

            RestClient client = new RestClient();
            if (window.editTxt.Text == "Save")
            {
                mapTest.Children.Clear();
                client.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/route/" + hTemp + "?" + "height=" + shift.busT.bus_height + "&width=" + shift.busT.bus_width + "&weight=" + shift.busT.bus_weight;
                response = client.makeRequest();
                
                window.route = response;
              

            }
            else
            {
                client.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/route/" + hTemp + "?" + "height=" + shift.bus.bus_height + "&width=" + shift.bus.bus_width + "&weight=" + shift.bus.bus_weight;
                response = client.makeRequest();
            }

            try
            {
                code = JsonConvert.DeserializeObject<JRoute>(response.ToString());
                //turns api response into JRoute object plots the path and draws to map the lines to map control               
                int via = 0;
                shift.jRoute = code;
                shift.jRouteString = response.ToString();

                //creates Bing Map API Request and add start and end push pins
                for (int i = 0; i < code.resourceSets[0].resources[0].routeLegs[0].itineraryItems.Length; i++)
                {
                    if (i == 0 || i == code.resourceSets[0].resources[0].routeLegs[0].itineraryItems.Length - 1)
                    {
                        double lat = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[0];
                        double lng = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[1];
                        mTemp += "&wp." + i + "=" + lat + "," + lng;
                        Pushpin pin = new Pushpin();
                        pin.Location = new Location(lat, lng);
                        mapTest.Children.Add(pin);
                    }
                    else if (code.resourceSets[0].resources[0].routeLegs[0].itineraryItems.Length >= 25 && via < 9)
                    {
                        double lat = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[0];
                        double lng = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[1];
                        mTemp += "&vwp." + i + "=" + lat + "," + lng;
                        via++;
                    }
                    else
                    {
                        double lat = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[0];
                        double lng = code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[1];
                        mTemp += "&wp." + i + "=" + lat + "," + lng;
                    }
                    I = i;
                }

                mapTest.Center = new Location(code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[0].maneuverPoint.coordinates[0], code.resourceSets[0].resources[0].routeLegs[0].itineraryItems[0].maneuverPoint.coordinates[1]);
                mapTest.ZoomLevel = 13;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't find location, try again");
            }



            try
            {
                MJRoute code2 = JsonConvert.DeserializeObject<MJRoute>(response.ToString());
                shift.mjRoute = code2;
                shift.mjRouteString = response.ToString();

                MapPolyline polyline = new MapPolyline();
                polyline.Stroke = new SolidColorBrush(Colors.Blue);
                polyline.StrokeThickness = 3;
                polyline.Opacity = 0.7;

                coll = new LocationCollection();

                ////full path
                foreach (var item in code2.resourceSets[0].resources[0].routePath.line.coordinates)
                {
                    coll.Add(new Location(item[0], item[1]));
                }
                //non matched 
                //foreach (var item in code2.resourceSets[0].resources[0].routeLegs[0].itineraryItems)
                //{
                //    coll.Add(new Location(item.maneuverPoint.coordinates[0], item.maneuverPoint.coordinates[1]));
                //}

                polyline.Locations = coll;
                mapTest.Children.Add(polyline);
                calc = true;

                //creates summary expander
                if (counter >= 2)
                {
                    JGeoCode sCode = cordsAddress[0];
                    JGeoCode eCode = cordsAddress[cordsAddress.Count - 1];
                    time = TimeSpan.FromSeconds(code2.resourceSets[0].resources[0].travelDuration);
                    kDistance = code2.resourceSets[0].resources[0].travelDistance;
                    mDistance = Math.Round(kDistance, 2);
                    Expander eExpander = new Expander
                    {
                        Header = "Summary",
                        Content = "Start: " + "\nTravel Time: " + time
                         + "\nDistance: " + mDistance + "miles"
                    };
                    waypointsTxt.Items.Add(eExpander);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()+"what what");
                calc = false;
            }
        }    

        private void RAcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            //checks if route has been calculated and a min of 2 check points
            if (calc == true && waypointsTxt.Items.Count >= 2)
            {
                if(datePicker.Value != null)
                {
                    shift.datePicked = datePicker.Value.ToString();
                    Close();
                    shift.cordsAddress = cordsAddress;
                    shift.routeTxt.Text = "Route Selected";
                }               
            } 
            else if (shift.tempJob.route != null)
            {
               MessageBoxResult result = MessageBox.Show("Are you sure you want to keep the same route","Confirm",MessageBoxButton.YesNo);
               if(result == MessageBoxResult.Yes)
               {
                    shift.tempJob.date = datePicker.Value.ToString();
                    Close();
               }                
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {            
            if(edit == true)
            {
                waypointsTxt.Items.Clear();
                edit = false;
            }
            if (routeTypeCmb.Text == waypoint[0] && counter > 0)
            {
                MessageBox.Show("Start waypoint already selected");
                waypoint[counter] = "";
            }
            else
            {
                //creates a request to bing maps api for a geo code
                RestClient client = new RestClient();
                client.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/geo/" + addTxt.Text;
                waypoint[counter] = routeTypeCmb.Text;
                string response = client.makeRequest();                

                try
                {
                    // creates JGeoCode from response
                    JGeoCode code1 = JsonConvert.DeserializeObject<JGeoCode>(response.ToString());

                    //creates expander objects added to listbox on button click
                    Expander expander = new Expander
                    {
                        Header = waypoint[counter],
                        Name = "expander" + counter.ToString(),
                        Content = "Address: " + "\n" + code1.resourceSets[0].resources[0].address.formattedAddress
                    };

                    RegisterName("expander" + counter.ToString(), expander);               
                    waypointsTxt.Items.Add(expander);
                    //keeps geo cords to forward to bing api
                    GeoCords.Enqueue(code1.resourceSets[0].resources[0].point.coordinates[0] + "," + code1.resourceSets[0].resources[0].point.coordinates[1]);
                    ////keeps response 
                    cordsAddress.Add(code1);

                    //ui formatting
                    waypoint[counter] = routeTypeCmb.Text;
                    routeTypeCmb.SelectedIndex = 2;
                    addTxt.Clear();
                    counter++; 

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Whoops something happened, please try again");
                }
            }              
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            counter--;
            Expander tb = (Expander)this.FindName("expander" + counter.ToString());
            try
            {
                tb.UnregisterName("expander" + counter.ToString());
                waypointsTxt.Items.Remove(tb);
                addTxt.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString()+"No way points found");
            }                              
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            if (window.editTxt.Text == "Save" )
            {
                edit = true;
                Job tempJob = (Job)shift.jobInfoDataGrid.SelectedItem;
               
                try
                {
                    datePicker.Value = Convert.ToDateTime(shift.tempJob.date);
                    MJRoute code2 = JsonConvert.DeserializeObject<MJRoute>(tempJob.route.ToString());
                    shift.mjRoute = code2;
                    shift.mjRouteString = tempJob.route.ToString();

                    MapPolyline polyline = new MapPolyline();
                    polyline.Stroke = new SolidColorBrush(Colors.Blue);
                    polyline.StrokeThickness = 3;
                    polyline.Opacity = 0.7;

                    coll = new LocationCollection();
                    int testCount = 0;

                    foreach (var item in code2.resourceSets[0].resources[0].routePath.line.coordinates)
                    {
                        if (testCount == 0)
                        {
                            Pushpin pin = new Pushpin();
                            pin.Location = new Location(item[0], item[1]);
                            mapTest.Children.Add(pin);
                            testCount++;
                        }
                        if (testCount == code2.resourceSets[0].resources[0].routePath.line.coordinates.Length)
                        {
                            Pushpin pin = new Pushpin();
                            pin.Location = new Location(item[0], item[1]);
                            mapTest.Children.Add(pin);
                        }

                        coll.Add(new Location(item[0], item[1]));
                        testCount++;

                    }


                    polyline.Locations = coll;
                    mapTest.Children.Add(polyline);

                    float[] center = code2.resourceSets[0].resources[0].routePath.line.coordinates[1];

                    mapTest.Center = new Location(Convert.ToDouble(center[0]), Convert.ToDouble(center[1]));
                    mapTest.ZoomLevel = 13;
                    calc = true;

                    //creates summary expander
                    if (counter >= 2 || testCount >= 2)
                    {
                        time = TimeSpan.FromSeconds(code2.resourceSets[0].resources[0].travelDuration);
                        kDistance = code2.resourceSets[0].resources[0].travelDistance;
                        mDistance = Math.Round(kDistance, 2);
                        Expander eExpander = new Expander
                        {
                            Header = "Summary",
                            Content = "Start: " + "\nTravel Time: " + time
                             + "\nDistance: " + mDistance + "miles"
                        };
                        waypointsTxt.Items.Add(eExpander);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()+ "Window loaded error");
                    calc = false;
                }
            }
            
        }
    }
}

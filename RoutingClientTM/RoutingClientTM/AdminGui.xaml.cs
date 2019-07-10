using RoutingClientTM.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdminGui.xaml
    /// </summary>
    public partial class AdminGui : Window
    {
        Users userP;
        Shifts shiftP;
        BusP busP;
        public int counter = 0;
        public int otherCounter = 0;
        public string route;      


        public AdminGui()
        {
            InitializeComponent();
            categoryViewer.Navigate(shiftP = new Shifts(this));
            ShowControls();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("ARE YOU SURE, ALL UNSAVED INFORMATION WILL BE LOST", "EXIT SHIFT MANAGER", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void shiftsBtn_Click(object sender, RoutedEventArgs e)
        {
            categoryViewer.Navigate(shiftP = new Shifts(this));
            shiftP.IsEnabled = true;
            ShowControls();
        }

        private void applicationsBtn_Click(object sender, RoutedEventArgs e)
        {
            //categoryViewer.Navigate(appP = new Applications());

            //HideControls();
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            categoryViewer.Navigate(busP = new BusP(this));

            //HideControls();
        }

        private void usersBtn_Click(object sender, RoutedEventArgs e)
        {
            categoryViewer.Navigate(userP = new Users(this));

            ShowControls();
        }

        public void HideControls()
        {
            newBtn.Visibility = Visibility.Hidden;
            editBtn.Visibility = Visibility.Hidden;
            deleteBtn.Visibility = Visibility.Hidden;
            refreshBtn.Visibility = Visibility.Hidden;
            printBtn.Visibility = Visibility.Hidden;
        }

        public void ShowControls()
        {
            newBtn.Visibility = Visibility.Visible;
            editBtn.Visibility = Visibility.Visible;
            deleteBtn.Visibility = Visibility.Visible;
            refreshBtn.Visibility = Visibility.Visible;
            printBtn.Visibility = Visibility.Visible;
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selPage = this.categoryViewer.NavigationService.Content.GetType().Name.ToString();

            switch (counter)
            {
                case 0:
                    if (selPage.ToString() == "Users")
                    {
                        userP.ControlsOn();
                        userP.driverTypeBox.SelectedIndex = 0;
                        userP.cpcTypeBox.SelectedIndex = 0;
                        userP.licenseTypeBox.SelectedIndex = 0;
                        userP.userTypeBox.SelectedIndex = 0;
                        userP.sNameTxt.Clear();
                        userP.fNameTxt.Clear();
                        userP.userIdTxt.Clear();



                        newImage.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        newSave.Text = "Save";

                        editBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "BusP")
                    {
                        busP.ControlsOn();
                        busP.Clear();



                        newImage.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        newSave.Text = "Save";

                        editBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "Shifts")
                    {
                        shiftP.ControlsOn();
                        shiftP.Clear();
                        MessageBoxResult result = MessageBox.Show("Are you sure you want to create a new job", "ALERT", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                            shiftP.IsEnabled = true;

                        newImage.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        newSave.Text = "Save";

                        editBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    break;
                case 1:
                    if (selPage.ToString() == "Users" && counter == 1)
                    {
                        #region testing validation

                        //checks names or size requirements
                        string tempF = userP.fNameTxt.Text;
                        string tempS = userP.sNameTxt.Text;

                        //adds user to database via rest api
                        if (tempF.Length <= 60 && tempS.Length <= 60)
                        {
                            RestClient restClient = new RestClient();
                            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/add/user/" + userP.userTypeBox.SelectedIndex + "/" + userP.fNameTxt.Text +
                                "/" + userP.sNameTxt.Text + "/" + userP.driverTypeBox.SelectedIndex + "/" + userP.licenseTypeBox.SelectedIndex + "/" + userP.cpcTypeBox.SelectedIndex + "/" + userP.emailTxt.Text;
                            string response = restClient.makeRequest();

                            if (response == "true")
                            {
                                MessageBox.Show("user saved");
                                newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
                                newSave.Text = "New";

                                editBtn.Visibility = Visibility.Visible;
                                deleteBtn.Visibility = Visibility.Visible;
                                refreshBtn.Visibility = Visibility.Visible;
                                printBtn.Visibility = Visibility.Visible;

                                userP.ControlsOff();
                                userP.driverTypeBox.SelectedIndex = 0;
                                userP.cpcTypeBox.SelectedIndex = 0;
                                userP.licenseTypeBox.SelectedIndex = 0;
                                userP.userTypeBox.SelectedIndex = 0;
                                userP.sNameTxt.Clear();
                                userP.fNameTxt.Clear();
                                userP.userIdTxt.Clear();
                                userP.emailTxt.Clear();
                                categoryViewer.Navigate(userP = new Users(this));
                                counter = 0;
                            }
                            else
                            {
                                MessageBox.Show("error: user not saved");
                            }

                        }

                        #endregion
                    }
                    else if (selPage.ToString() == "BusP" && counter == 1)
                    {
                        #region testing validation
                        Regex regRegex = new Regex(@"(?<Current>^[A-Z]{2}[0-9]{2}[A-Z]{3}$)|(?<Prefix>^[A-Z][0-9]{1,3}[A-Z]{3}$)|(?<Suffix>^[A-Z]{3}[0-9]{1,3}[A-Z]$)|(?<DatelessLongNumberPrefix>^[0-9]{1,4}[A-Z]{1,2}$)|(?<DatelessShortNumberPrefix>^[0-9]{1,3}[A-Z]{1,3}$)|(?<DatelessLongNumberSuffix>^[A-Z]{1,2}[0-9]{1,4}$)|(?<DatelessShortNumberSufix>^[A-Z]{1,3}[0-9]{1,3}$)");
                        Match match = regRegex.Match(busP.regTxt.Text.ToUpper());
                        if (match.Success)
                        {
                            Regex yearRegex = new Regex(@"(^\d{4}$)");
                            Match match1 = yearRegex.Match(busP.yearTxt.Text.ToUpper());
                            if (match1.Success)
                            {
                                Regex wHwRegex = new Regex(@"^(\.0{1,2})?|[0-9]{0,2}(\.\d{1,2})?$");
                                Match match2 = wHwRegex.Match(busP.weightTxt.Text.ToUpper());
                                if (match2.Success)
                                {
                                    wHwRegex = new Regex(@"^(\.0{1,2})?|[0-9]{0,2}(\.\d{1,2})?$");
                                    match2 = wHwRegex.Match(busP.heightTxt.Text.ToUpper());

                                    if (match2.Success)
                                    {
                                        wHwRegex = new Regex(@"^(\.0{1,2})?|[0-9]{0,2}(\.\d{1,2})?$");
                                        match2 = wHwRegex.Match(busP.widthTxt.Text.ToUpper());
                                        if (match2.Success)
                                        {
                                            //adds bus to database via rest api

                                            RestClient restClient = new RestClient();
                                            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/add/bus/" + busP.regTxt.Text + "/" + busP.busTypeBox.SelectedIndex + "/" + busP.weightTxt.Text + "/" +
                                                busP.heightTxt.Text + "/" + busP.widthTxt.Text + "/" + busP.capacityTxt.Text + "/" + busP.yearTxt.Text + "/" + busP.modelTypeBox.Text + "/" + busP.tachoTypeBox.SelectedIndex;
                                            string response = restClient.makeRequest();

                                            if (response == "true")
                                            {
                                                MessageBox.Show("Bus saved");
                                                newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
                                                newSave.Text = "New";

                                                editBtn.Visibility = Visibility.Visible;
                                                deleteBtn.Visibility = Visibility.Visible;
                                                refreshBtn.Visibility = Visibility.Visible;
                                                printBtn.Visibility = Visibility.Visible;

                                                busP.Clear();
                                                busP.ControlsOff();
                                                counter = 0;
                                            }
                                            else
                                            {
                                                MessageBox.Show("error: user not saved");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("The entered width entered must be in a valid format d.dd e.g 6.78 ");
                                            busP.widthTxt.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("The entered height entered must be in a valid format d.dd e.g 6.78 ");
                                        busP.heightTxt.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("The entered weight entered must be in a valid format d.dd e.g 6.78 ");
                                    busP.widthTxt.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("The entered year entered must be in a valid format YYYY e.g 1999 ");
                                busP.yearTxt.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("The entered registration entered must be in a valid UK format ");
                            busP.regTxt.Focus();
                        }





                        #endregion
                    }
                    else if (selPage.ToString() == "Shifts" && counter == 1)
                    {
                        #region testing validation

                        try
                        {
                            foreach (var item in shiftP.mjRoute.resourceSets[0].resources[0].routePath.line.coordinates)
                            {
                                if (otherCounter == 1)
                                {
                                    route += item[0] + "," + item[1] + " ";
                                }
                                else if (otherCounter == shiftP.mjRoute.resourceSets[0].resources[0].routePath.line.coordinates.Length)
                                {
                                    route += item[0] + "," + item[1];
                                }
                                else
                                {
                                    route += item[0] + "," + item[1] + " ";
                                }

                            }

                            //adds job to database via rest api                          
                            MessageBox.Show(shiftP.datePicked);
                            RestClient restClient = new RestClient();
                            restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/job/add/" + shiftP.driver.user_id + "/" + shiftP.bus.bus_reg +
                                "/" + shiftP.start.resourceSets[0].resources[0].address.formattedAddress + "/" + shiftP.end.resourceSets[0].resources[0].address.formattedAddress + "/" + shiftP.passNum;
                            restClient.httpMethod = httpVerb.POST;
                            restClient.postJson = shiftP.mjRouteString;
                            restClient.postRoute = shiftP.datePicked;

                            string response = restClient.PostRequest();

                            if (response == "true")
                            {
                                MessageBox.Show("Job saved");
                                newImage.Source = new BitmapImage(new Uri("Images/new.png", UriKind.Relative));
                                newSave.Text = "New";

                                editBtn.Visibility = Visibility.Visible;
                                deleteBtn.Visibility = Visibility.Visible;
                                refreshBtn.Visibility = Visibility.Visible;
                                printBtn.Visibility = Visibility.Visible;
                                counter = 0;
                                //creates clean shift page
                                categoryViewer.Navigate(shiftP = new Shifts(this));
                                shiftP.ControlsOff();
                            }
                            else
                            {
                                MessageBox.Show("error: job not saved");
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                        #endregion
                    }
                    break;
            }
        }

        private void EditBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selPage = this.categoryViewer.NavigationService.Content.GetType().Name.ToString();

            switch (counter)
            {
                case 0:
                    if (selPage.ToString() == "Users")
                    {
                        userP.ControlsOn();

                        editImg.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        editTxt.Text = "Save";

                        newBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "BusP")
                    {
                        busP.ControlsOn();

                        editImg.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        editTxt.Text = "Save";

                        newBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "Shifts" && shiftP.jobInfoDataGrid.SelectedIndex == -1)
                    {
                        MessageBox.Show("You Must Select a Job to edit it");
                    }
                    else if (selPage.ToString() == "Shifts" && shiftP.jobInfoDataGrid.SelectedIndex != -1)
                    {
                        editImg.Source = new BitmapImage(new Uri("Images/save.jpg", UriKind.Relative));
                        editTxt.Text = "Save";
                        shiftP.ControlsOn();
                        newBtn.Visibility = Visibility.Hidden;
                        deleteBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    break;
                case 1:
                    if (selPage.ToString() == "Users" && counter == 1)
                    {
                        #region testing validation

                        RestClient restClient = new RestClient();
                        restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/edit/user/" + userP.userIdTxt.Text + "/" + userP.userTypeBox.SelectedIndex + "/" +
                            userP.fNameTxt.Text + "/" + userP.sNameTxt.Text + "/" + userP.driverTypeBox.SelectedIndex + "/" + userP.licenseTypeBox.SelectedIndex + "/" + userP.cpcTypeBox.SelectedIndex + "/" + userP.emailTxt.Text;


                        string response = restClient.makeRequest();

                        if (response == "true")
                        {
                            MessageBox.Show("user succesfully edited");
                            editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
                            newSave.Text = "New";
                            editTxt.Text = "Edit";

                            newBtn.Visibility = Visibility.Visible;
                            deleteBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;

                            userP.ControlsOff();
                            userP.driverTypeBox.SelectedIndex = 0;
                            userP.cpcTypeBox.SelectedIndex = 0;
                            userP.licenseTypeBox.SelectedIndex = 0;
                            userP.userTypeBox.SelectedIndex = 0;
                            userP.sNameTxt.Clear();
                            userP.fNameTxt.Clear();
                            userP.userIdTxt.Clear();
                            userP.emailTxt.Clear();

                            categoryViewer.Navigate(userP = new Users(this));
                            ShowControls();
                            counter = 0;

                        }

                        #endregion
                    }
                    else if (selPage.ToString() == "BusP" && counter == 1)
                    {
                        #region testing validation

                        RestClient restClientT = new RestClient();
                        restClientT.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/edit/bus/" + busP.regTxt.Text + "/" + busP.busTypeBox.SelectedIndex + "/" + busP.weightTxt.Text + "/" +
                                busP.heightTxt.Text + "/" + busP.widthTxt.Text + "/" + busP.capacityTxt.Text + "/" + busP.yearTxt.Text + "/" + busP.modelTypeBox.Text + "/" + busP.tachoTypeBox.SelectedIndex;


                        string response = restClientT.makeRequest();

                        if (response == "true")
                        {
                            MessageBox.Show("Bus succesfully edited");
                            editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
                            newSave.Text = "New";
                            editTxt.Text = "Edit";

                            newBtn.Visibility = Visibility.Visible;
                            deleteBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;

                            busP.Clear();
                            categoryViewer.Navigate(busP = new BusP(this));
                            busP.ControlsOff();
                            counter = 0;
                            ShowControls();
                        }

                        #endregion
                    }
                    else if (selPage.ToString() == "Shifts" && counter == 1)
                    {
                        #region testing validation

                        RestClient restClient = new RestClient();
                        restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/job/edit/" + shiftP.tempJob.job_id + "/" + shiftP.driverT.user_id + "/" + shiftP.busT.bus_reg + "/" +
                                shiftP.tempJob.start_location + "/" + shiftP.tempJob.end_location + "/" + shiftP.tempJob.pass_num;
                        restClient.httpMethod = httpVerb.POST;
                        restClient.postJson = shiftP.tempJob.route;
                        restClient.postRoute = shiftP.tempJob.date;
                        string response = restClient.makeRequest();

                        if (response == "true")
                        {
                            MessageBox.Show("Job succesfully edited");
                            editImg.Source = new BitmapImage(new Uri("Images/edit.png", UriKind.Relative));
                            newSave.Text = "New";
                            editTxt.Text = "Edit";

                            newBtn.Visibility = Visibility.Visible;
                            deleteBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;


                            categoryViewer.Navigate(shiftP = new Shifts(this));

                            counter = 0;
                            ShowControls();
                        }

                        #endregion
                    }
                    break;
            }
        }

        private void DeleteBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selPage = this.categoryViewer.NavigationService.Content.GetType().Name.ToString();

            switch (counter)
            {
                case 0:
                    if (selPage.ToString() == "Users")
                    {
                        userP.ControlsOn();
                        deleteTxt.Text = "Confirm";
                        newBtn.Visibility = Visibility.Hidden;
                        editBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "BusP")
                    {
                        busP.ControlsOn();
                        deleteTxt.Text = "Confirm";
                        newBtn.Visibility = Visibility.Hidden;
                        editBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    else if (selPage.ToString() == "Shifts")
                    {
                        MessageBox.Show("To delete a job click a job from the list and click the delete icon");

                        deleteTxt.Text = "Confirm";
                        newBtn.Visibility = Visibility.Hidden;
                        editBtn.Visibility = Visibility.Hidden;
                        refreshBtn.Visibility = Visibility.Hidden;
                        printBtn.Visibility = Visibility.Hidden;

                        counter = 1;
                    }
                    break;
                case 1:
                    if (selPage.ToString() == "Users" && counter == 1)
                    {
                        #region testing validation

                        RestClient restClient = new RestClient();
                        restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/delete/user/" + userP.userIdTxt.Text;


                        string response = restClient.makeRequest();
                        MessageBox.Show("ENDPOINT: \n" + restClient.endPoint + "RESPONSE: " + response);

                        if (response == "true")
                        {
                            deleteTxt.Text = "Delete";
                            MessageBox.Show("user succesfully deleted");

                            newBtn.Visibility = Visibility.Visible;
                            editBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;

                            userP.ControlsOff();
                            userP.driverTypeBox.SelectedIndex = 0;
                            userP.cpcTypeBox.SelectedIndex = 0;
                            userP.licenseTypeBox.SelectedIndex = 0;
                            userP.userTypeBox.SelectedIndex = 0;
                            userP.sNameTxt.Clear();
                            userP.fNameTxt.Clear();
                            userP.userIdTxt.Clear();
                            userP.emailTxt.Clear();

                            categoryViewer.Navigate(userP = new Users(this));
                            ShowControls();
                            counter = 0;

                        }

                        #endregion
                    }
                    else if (selPage.ToString() == "BusP" && counter == 1)
                    {
                        #region testing validation

                        RestClient restClient = new RestClient();
                        restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/delete/bus/" + busP.regTxt.Text;


                        string response = restClient.makeRequest();
                        MessageBox.Show("ENDPOINT: \n" + restClient.endPoint + "RESPONSE: " + response);

                        if (response == "true")
                        {
                            deleteTxt.Text = "Delete";
                            MessageBox.Show("user succesfully deleted");

                            newBtn.Visibility = Visibility.Visible;
                            editBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;

                            busP.Clear();
                            categoryViewer.Navigate(busP = new BusP(this));
                            busP.ControlsOff();
                            counter = 0;
                            ShowControls();
                        }

                        #endregion
                    }
                    else if (selPage.ToString() == "Shifts" && counter == 1)
                    {
                        #region testing validation

                        Job job = (Job)shiftP.jobInfoDataGrid.SelectedItem;
                        RestClient restClient = new RestClient();
                        restClient.endPoint = "http://localhost:8080/AppServer_war_exploded/logic/job/delete/" + job.job_id;


                        string response = restClient.makeRequest();
                        MessageBox.Show("ENDPOINT: \n" + restClient.endPoint + "RESPONSE: " + response);

                        if (response == "true")
                        {
                            deleteTxt.Text = "Delete";
                            MessageBox.Show("job succesfully deleted");

                            newBtn.Visibility = Visibility.Visible;
                            editBtn.Visibility = Visibility.Visible;
                            refreshBtn.Visibility = Visibility.Visible;
                            printBtn.Visibility = Visibility.Visible;

                            categoryViewer.Navigate(shiftP = new Shifts(this));
                            shiftP.ControlsOff();
                            counter = 0;
                            ShowControls();
                        }
                        #endregion
                    }
                    break;
            }
        }
    }
}

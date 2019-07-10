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
    /// Interaction logic for DialogInput.xaml
    /// </summary>
    public partial class DialogInput : Window
    {
        Buses buses;
        public DialogInput(Buses window)
        {
            InitializeComponent();
            buses = window;
            try
            {
                pasNumTxt.Text = window.shift.tempJob.pass_num.ToString();
            }
            catch(Exception e)
            {

            }
           
        }

        private void PassNumBtn_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"[0-9]{1,2}");
            Match match = regex.Match(pasNumTxt.Text.ToString());

            if (pasNumTxt.Text == "Passenger Number")
            {
                MessageBox.Show("Enter Pasenger number into the below text box to continue");
            }
            else
            {
                try
                {                   
                    if (match.Success)
                    {                       
                        buses.shift.tempJob.pass_num = pasNumTxt.Text.ToString();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Passenger number can only be between 1 and 99");
                    }

                    
                }
                catch(Exception ex)
                {
                    if (match.Success)
                    {
                        buses.shift.passNum = int.Parse(pasNumTxt.Text);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Passenger number can only be between 1 and 99");
                    }
                }      
                
            }
           
        }

        private void PasNumTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            pasNumTxt.Text = "";
        }       
    }
}

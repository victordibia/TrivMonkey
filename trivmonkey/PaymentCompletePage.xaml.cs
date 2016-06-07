using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;

namespace TrivMonkey
{
    public partial class PaymentCompletePage : PhoneApplicationPage
    {


        public PaymentCompletePage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            string error = ""; // (string)PhoneApplicationService.Current.State["error"];
            int coinvalue = 30; // (int)PhoneApplicationService.Current.State["coinvalue"];
            if (error.Equals(""))
            {
                lbltitle.Text = "PAYMENT SUCCESSFUL";
                lblfunny.Text = "ALL RIGHT SPARKY!";
                lblmessage.Text = "Congratulations, you have successfully purchase " + coinvalue + " MONKEY KOINS . Go right ahead and spend it!";
            }
            else {
                lbltitle.Text = "PAYMENT FAILED";
                lblfunny.Text = "I'M SORRY CAPTAIN JIM!";
                lblmessage.Text = "Unfortunately, we could not complete your purchase at this time.\n" + error ;
            
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void btntryagain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        } 
    }
}
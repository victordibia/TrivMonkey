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
using Quickteller.Sdk.Wp8;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace TrivMonkey
{
    public partial class BuyPage : PhoneApplicationPage
    {

        // Put your developer.interswitchng.com client id and secret key here
        // These credentials are only necessary if want your app to support Interswitch payment in West Africa.
        // https://connect.interswitchng.com/documentation/verve-payment-sdk-for-windows-phone-8/
        private const string CLIENT_ID = "";  // Sample, replace with yours ;

        private const string CLIENT_SECRET = ""; // Sample, replace with yours;;
        private const string PAYMENT_CODE = "XXXXX";  // 5 numbers

        private static Popup my_popup_cs =  new Popup();
        long paymentamount = 0;
        int paymentcoinvalue = 0;
        TextBlock txt_blk2 = new TextBlock();
        bool popped = false;



        public BuyPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;


        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            //this.dispatcherTimer.Stop();
            base.OnBackKeyPress(e);
            if (my_popup_cs.IsOpen == true)
            {
                System.Diagnostics.Debug.WriteLine("Backpressed ");
                my_popup_cs.IsOpen = false;
                 e.Cancel = true;
            }

        }



        private void fulfilCoins(int coinvalue)
        {
            GamePage.updateKoins(coinvalue);
            navigateComplete("", coinvalue);
        }

        private void navigateComplete(string error, int coinamount)
        {
           // loadinglabel.Visibility = Visibility.Collapsed;
            System.Diagnostics.Debug.WriteLine("Navigate Called");

            if (error.Equals(""))
            {
                lbltitle.Text = "PAYMENT SUCCESSFUL";
                lblfunny.Text = "ALL RIGHT SPARKY!";
                newbalance.Text = MainPage.gamemonkeycoins;
                btndone.Content = " Oh Yeah!";
                lblmessage.Text = "Congratulations. \nYou have successfully purchased " + coinamount + " MONKEY KOINS . Feel free to purchase more stuff or go right back and spend your koins on some good stuff!";
                btndone.Visibility = Visibility.Visible;
            }
            else
            {
                lbltitle.Text = "PAYMENT FAILED";
                lblfunny.Text = "I'M SORRY CAPTAIN JIM!";
                lblmessage.Text = "Unfortunately, we could not complete your purchase at this time.\n" + error;
                btntryagain.Visibility = Visibility.Visible;


            }

            stckbeforepurchase.Visibility = Visibility.Collapsed;
            stkafterpurchase.Visibility = Visibility.Visible;
            myStoryboard.Begin();
        }

        public async Task display_cspopup()
        {
            await Task.Run(
                () =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {


                        if (popped == false)
                        {

                            my_popup_cs = new Popup();
                            Border border = new Border();                                                     // to create green color border
                            border.BorderBrush = new SolidColorBrush(Colors.Green);
                            border.BorderThickness = new Thickness(2);
                            border.Margin = new Thickness(10, 10, 10, 10);

                            Grid totalgrid = new Grid();

                            SolidColorBrush gridback = new SolidColorBrush(Colors.Black);
                            gridback.Opacity = 0.8;

                            totalgrid.SetValue(Grid.BackgroundProperty, gridback);

                            totalgrid.Height = Application.Current.Host.Content.ActualHeight;
                            totalgrid.Width = Application.Current.Host.Content.ActualWidth;
                            totalgrid.VerticalAlignment = VerticalAlignment.Top;

                            // totalgrid.Opacity = 0.8;


                            Style style = (Style)Application.Current.Resources["OptionButtonStyle"];

                            StackPanel skt_pnl_outter = new StackPanel();                             // stack panel
                            skt_pnl_outter.VerticalAlignment = VerticalAlignment.Center;
                            //skt_pnl_outter.Background = new SolidColorBrush(Colors.LightGray);
                            skt_pnl_outter.Orientation = System.Windows.Controls.Orientation.Vertical;

                            Image img_disclaimer = new Image();                                       // Image
                            img_disclaimer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            img_disclaimer.Height = 50;
                            img_disclaimer.Width = 50;
                            img_disclaimer.Stretch = Stretch.UniformToFill;
                            img_disclaimer.Margin = new Thickness(25, 5, 0, 0);
                            Uri uriR = new Uri("img/coins.png", UriKind.Relative);
                            BitmapImage imgSourceR = new BitmapImage(uriR);
                            img_disclaimer.Source = imgSourceR;

                            StackPanel skt_pnl_mage = new StackPanel();                             // stack panel
                            skt_pnl_mage.VerticalAlignment = VerticalAlignment.Center;
                            //skt_pnl_outter.Background = new SolidColorBrush(Colors.LightGray);
                            skt_pnl_mage.Orientation = System.Windows.Controls.Orientation.Horizontal;




                            TextBlock txt_blk1 = new TextBlock();                                         // Textblock 1
                            txt_blk1.Text = "Select Payment Method ";
                            txt_blk1.TextAlignment = TextAlignment.Left;
                            //txt_blk1.TextAlignment = TextAlignment.Center;
                            txt_blk1.FontSize = 30;
                            txt_blk1.Margin = new Thickness(20, 20, 20, 0);
                            txt_blk1.Foreground = new SolidColorBrush(Colors.White);



                            skt_pnl_mage.Children.Add(img_disclaimer);
                            skt_pnl_mage.Children.Add(txt_blk1);

                            SolidColorBrush brush = (SolidColorBrush)App.Current.Resources["yellowrange"];



                            StackPanel skt_pnl_inner = new StackPanel();
                            skt_pnl_inner.Margin = new Thickness(20, 10, 20, 20);
                            Button btn_continue = new Button();
                            Button btn_cancel = new Button();
                            btn_continue.Foreground = new SolidColorBrush(Colors.White);
                            btn_cancel.Foreground = new SolidColorBrush(Colors.White);

                            btn_continue.Content = "Verve";
                            btn_continue.Style = style;
                            btn_continue.Height = 100;
                            btn_continue.Margin = new Thickness(0, 0, 0, 15);
                            btn_continue.Click += new RoutedEventHandler(btn_verve_Click);

                            // Button cancel
                            btn_cancel.Content = "Paypal , Credit , Debit Card";
                            btn_cancel.Style = style;
                            btn_cancel.Height = 100;
                            btn_cancel.Click += new RoutedEventHandler(btn_paypal_Click);



                            //TextBlock txt_blk2 = new TextBlock();

                            txt_blk2.TextWrapping = TextWrapping.Wrap;
                            txt_blk2.TextAlignment = TextAlignment.Left;
                            txt_blk2.FontSize = 20;
                            txt_blk2.Margin = new Thickness(20, 10, 20, 10);
                            txt_blk2.Foreground = new SolidColorBrush(Colors.White);


                            //Adding control to stack panel
                            totalgrid.Children.Add(skt_pnl_outter);
                            //skt_pnl_outter.Children.Add(img_disclaimer);
                            skt_pnl_outter.Children.Add(skt_pnl_mage);
                            skt_pnl_outter.Children.Add(txt_blk2);


                            skt_pnl_inner.HorizontalAlignment = HorizontalAlignment.Stretch;
                            skt_pnl_inner.Orientation = System.Windows.Controls.Orientation.Vertical;


                            skt_pnl_inner.Children.Add(btn_continue);
                            skt_pnl_inner.Children.Add(btn_cancel);


                            skt_pnl_outter.Children.Add(skt_pnl_inner);

                            // Adding stackpanel  to border
                            //border.Child = skt_pnl_outter;

                            // Adding border to pup-up
                            my_popup_cs.Child = totalgrid;



                            my_popup_cs.IsOpen = true;
                            popped = true;

                        }
                        else
                        {
                            my_popup_cs.IsOpen = true;
                        }


                        txt_blk2.Text = "Select your preferred payment method to purchase " + paymentcoinvalue + " monkey coins.";

                    }
                       );

                });
        }

        private void btn_verve_Click(object sender, RoutedEventArgs e)
        {

            if (my_popup_cs.IsOpen)
            {                //performUnlock(unlockCurrentCategory.mcoins);
                my_popup_cs.IsOpen = false;
                paymentSelector(paymentamount, paymentcoinvalue, "verve");
            }
        }

        private void btn_paypal_Click(object sender, RoutedEventArgs e)
        {

            if (my_popup_cs.IsOpen)
            {                //performUnlock(unlockCurrentCategory.mcoins);
                my_popup_cs.IsOpen = false;
                paymentSelector(paymentamount, paymentcoinvalue, "paypal");
                //display_cspopup(unlockCurrentCategory.title, "congrats", Int32.Parse(unlockCurrentCategory.mcoins));
            }
        }

        private async Task paymentSelector(long amount, int coinamount, string provider)
        {

            if (provider.Equals("verve"))
            {
                PerformPaymentOperation(amount, coinamount);
            }
            else
            {

                ListingInformation li = await Store.CurrentApp.LoadListingInformationAsync();
                foreach (string keyz in li.ProductListings.Keys)
                {
                    ProductListing pListing = li.ProductListings[keyz];
                    System.Diagnostics.Debug.WriteLine(keyz);
                }

                string key = "";
                switch (coinamount)
                {
                    case 50:
                        key = "50_MONKEY_KOINS";
                        break;
                    case 100:
                        key = "100_MONKEY_KOINS";
                        break;
                    case 500:
                        key = "500_MONKEY_KOINS";
                        break;
                    case 1000:
                        key = "1000_MONKEY_KOINS";
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                string pID = li.ProductListings[key].ProductId;




                try
                {
                    string receipt = await Store.CurrentApp.RequestProductPurchaseAsync(pID, true);
                    System.Diagnostics.Debug.WriteLine("Reciept ooo : " + receipt);
                    ProductLicense prod = CurrentApp.LicenseInformation.ProductLicenses[key];
                    if (prod.IsActive)
                    {
                        System.Diagnostics.Debug.WriteLine("Fulfilling now");
                        fulfilCoins(coinamount);
                        Store.CurrentApp.ReportProductFulfillment(pID);

                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Not fulfiling.. ussues");
                  //  loadinglabel.Visibility = Visibility.Collapsed;
                    //navigateComplete("The payment process was cancelled.", coinamount);

                }
                // if (receipt .Equals ("")){

                // }



            }

        }

        private async Task beforePayment(long amount, int coinamount)
        {
            await Task.Run(
               () =>
               {
                   Deployment.Current.Dispatcher.BeginInvoke(() =>
                     {
                         loadinglabel.Text = "  Loading payment interface ...  ";
                     });
                   paymentamount = amount;
                   paymentcoinvalue = coinamount;
                   bool netavail = (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType !=
                   Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);

                   if (NetworkInterface.GetIsNetworkAvailable())
                   {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                       {
                       loadinglabel.Text = "";
                       display_cspopup();
                       }
                       );

                   }
                   else
                   {
                       Deployment.Current.Dispatcher.BeginInvoke(() =>
                       {
                           MessageBox.Show("Oops! It appears you might not have an internet connecting on this device. Please check your network settings.", "Check Internet Settings!", MessageBoxButton.OK);
                           loadinglabel.Text = "";
                       }
                       );

                       }
                   //display_cspopup(long amount, int coinamount);
               });

        }

        private async Task PerformPaymentOperation(long amount, int coinamount)
        {

            await Task.Run(
                () =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            string custID = MainPage.GetWindowsLiveAnonymousID();
                            System.Diagnostics.Debug.WriteLine(custID);

                            var quicktellerPayment = new QuicktellerPayment(this, PAYMENT_CODE, amount, custID, CLIENT_ID, CLIENT_SECRET, false);

                            quicktellerPayment.OnPaymentCompleted += (e) =>
                            {
                                fulfilCoins(coinamount);//where e is a type of PaymentCompletedEventArgs
                                //navigateComplete("", coinamount);
                            };
                            quicktellerPayment.OnPaymentException += (e) =>
                            {
                                navigateComplete(e.PaymentException.Message, coinamount);
                            };

                            quicktellerPayment.DoPayment();
                        });

                });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            beforePayment(100 * 50, 50);

            System.Diagnostics.Debug.WriteLine(" Performpayment");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            beforePayment(100 * 100, 100);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            beforePayment(100 * 300, 500);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            beforePayment(100 * 350, 1000);
        }


        private void btntryagain_Click(object sender, RoutedEventArgs e)
        {
            stkafterpurchase.Opacity = 0;
            lbltitle.Text = "BUY MONKEY KOINS";
            stckbeforepurchase.Visibility = Visibility.Visible;
            stkafterpurchase.Visibility = Visibility.Collapsed;
            stkStoryboard.Begin();
            // NavigationService.Navigate(new Uri("/BuyPage.xaml", UriKind.Relative));
            //this.NavigationService.GoBack();
        }

        private void btndone_Click(object sender, RoutedEventArgs e)
        {
            // this.NavigationService.GoBack();
            stkafterpurchase.Opacity = 0;
            lbltitle.Text = "BUY MONKEY KOINS";
            stckbeforepurchase.Visibility = Visibility.Visible;
            stkafterpurchase.Visibility = Visibility.Collapsed;
            stkStoryboard.Begin();
        }



    }
}

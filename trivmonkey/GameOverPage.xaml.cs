using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace TrivMonkey
{
    public partial class GameOverPage : PhoneApplicationPage
    {
        public GameOverPage()
        {
            InitializeComponent();
            lbl_score.Text = GamePage.score + "";



            if (GamePage.score > 50)
            {
                gameoverdesc.Text = "YOUR SCORE : " + GamePage.score + ". NICE PLAY!";
            }
            else if (GamePage.score > 100)
            {
                gameoverdesc.Text = "YOUR SCORE : " + GamePage.score + ". SUPERB PLAY - YOU ARE ON A WINNING STREAK!!";
            }
            else {
                gameoverdesc.Text = "YOUR SCORE : " + GamePage.score + ". YOU'LL BE BETTER NEXT TIME!";
            }

            BitmapImage bm = new BitmapImage(new Uri(@"/img/" + MainPage.CurrentSubCategory.imagelink, UriKind.RelativeOrAbsolute));
            img_thumb3.Source = bm;
             
        }

       protected override void OnBackKeyPress(CancelEventArgs e) {
           base.OnBackKeyPress(e);
            //var result = MessageBox.Show("Do you want to exit?", "Attention!",
            //                    MessageBoxButton.OKCancel);

            //if (result == MessageBoxResult.OK)
            //{
            //     Do not cancel navigation
            //    return;
            //}
           // e.Cancel = true;
            
           
           //GamePage .co
            //NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));

        }
        private void playbtnclick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void sharebtnclick(object sender, RoutedEventArgs e)
        {
            ShareStatusTask sst = new ShareStatusTask();
            sst.Status = "I just scored " + GamePage .score  
                + " while playing the trivia pack  " 
                + MainPage .CurrentSubCategory .title  
                +  " on Trivia Monkey for Windows Phone! Check it out!" ;
            sst.Show();
        }

        private void menubtnclick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}
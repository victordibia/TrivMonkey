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
using System.Windows.Media;
using System.Threading.Tasks;
using Scoreloop.CoreSocial.API;
using System.Diagnostics;

namespace TrivMonkey
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public string oldname = "";
        public SettingsPage()
        {
            InitializeComponent();


            //Create and add theme buttons
            generateThemeButtons();
           checkSettingPresets();
          // MessageBox.Show(System.Globalization.RegionInfo.CurrentRegion.DisplayName);




        }

        private void checkSettingPresets()
        {
            
                           if (MainPage.gamesound.Equals("1"))
                           {
                               togglesound.IsChecked = true;
                               togglesound.Content = "Sound is On";
                           }
                           else
                           {
                               togglesound.IsChecked = false;
                               togglesound.Content = "Sound is Off";
                           }


                           if (MainPage.gamevibration.Equals("1"))
                           {
                               togglevibration.IsChecked = true;
                               togglevibration.Content = "Vibration is On";
                           }
                           else
                           {
                               togglevibration.IsChecked = false;
                               togglevibration.Content = "Vibration is Off";
                           }


                           if (MainPage.checkTile())
                           {
                               togglelivetile.IsChecked = true;
                               togglelivetile.Content = "Live Tiles in On";
                           }
                           else
                           {
                               togglelivetile.IsChecked = false;
                               togglelivetile.Content = "Live Tiles in Off";
                           }
                       
        }

        private void generateThemeButtons()
        {



            for (int i = 0; i < MainPage.colorjam.Count(); i++)
            {
                Button btn = new Button()
                {
                    Content = MainPage.colorjamnames[i],
                    Background = new SolidColorBrush(MainPage.colorjam[i]),
                    Width = 110,
                    Height = 110,
                    FontSize = 18,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Medium,
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(0, 0, 12, 0),
                    Style = (Style)App.Current.Resources["OptionButtonStyle"]

                };
                btn.Click += btn_Click; ;
                themestack.Children.Add(btn);
            }

        }

        void btn_Click(object sender, RoutedEventArgs e)
        {

            Button thisbut = (Button)sender;


            for (int i = 0; i < MainPage.colorjam.Count(); i++)
            {
                if (thisbut.Content.Equals(MainPage.colorjamnames[i]))
                {

                    SolidColorBrush brush = (SolidColorBrush)App.Current.Resources["yellowrange"];
                    brush.Color = MainPage.colorjam[i];

                    //Update setting themecolor
                    var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                            select settings).FirstOrDefault();
                    if (SettingItemsInDB != null)
                    {
                        SettingItemsInDB.themecolor = i + "";
                        MainPage.toDoDB.SubmitChanges();
                    }
                }
            }

            //throw new NotImplementedException();
        }



        private void togglesound_Click(object sender, RoutedEventArgs e)
        {
            //Update setting sound
            //var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
            //                       select settings).FirstOrDefault();

            //if (togglesound.IsChecked == true)
            //{
            //    SettingItemsInDB.gamesound = 0 + "";
            //    togglesound.Content = "Sound is On";
            //}
            //else {
            //    SettingItemsInDB.gamesound = 1 + "";
            //    togglesound.Content = "Sound is On";
            //}

            //MainPage.toDoDB.SubmitChanges();
        }

        private void togglesound_Checked(object sender, RoutedEventArgs e)
        {
            togglesound.Content = "Sound is On";
            var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                    select settings).FirstOrDefault();
            SettingItemsInDB.gamesound = 1 + "";
            MainPage.toDoDB.SubmitChanges();
            MainPage.gamesound = "1";
        }

        private void togglesound_Unchecked(object sender, RoutedEventArgs e)
        {
            togglesound.Content = "Sound is Off";
            var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                    select settings).FirstOrDefault();
            SettingItemsInDB.gamesound = 0 + "";
            MainPage.toDoDB.SubmitChanges();

            MainPage.gamesound = "0";
        }

        private void togglevibration_Unchecked(object sender, RoutedEventArgs e)
        {
            togglevibration.Content = "Vibration is Off";
            var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                    select settings).FirstOrDefault();
            SettingItemsInDB.vibration = 0 + "";
            MainPage.toDoDB.SubmitChanges();
            MainPage.gamevibration = "0";
        }

        private void togglevibration_Checked(object sender, RoutedEventArgs e)
        {
            togglevibration.Content = "Vibration is On";
            var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                    select settings).FirstOrDefault();
            SettingItemsInDB.vibration = 1 + "";
            MainPage.toDoDB.SubmitChanges();
            MainPage.gamevibration = "1";
        }

        private void togglelocation_Checked(object sender, RoutedEventArgs e)
        {
            togglelocation.Content = "Location is On";
        }

        private void togglelocation_Unchecked(object sender, RoutedEventArgs e)
        {
            togglelocation.Content = "Location is Off";
        }

        private void togglelivetile_Unchecked(object sender, RoutedEventArgs e)
        {
            MainPage.deleteTile();
        }

        private void togglelivetile_Checked(object sender, RoutedEventArgs e)
        {
            MainPage.createTile();
        }

         

    }
}
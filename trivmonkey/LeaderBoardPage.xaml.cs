using System;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.ComponentModel;
using TrivMonkey.ViewModels;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Diagnostics;
using System.Windows.Data;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Info;

using Scoreloop.CoreSocial.API;
using Scoreloop.CoreSocial.API.Model;
using Scoreloop.CoreSocial.API.WebBrowserExtensions;
using Microsoft.Phone.Net.NetworkInformation;
using System.Globalization;

namespace TrivMonkey
{
    public partial class LeaderBoardPage : PhoneApplicationPage
    {
        private IScoresController _scoresController;
        private IScoresController _scoresControllerKP;

        IScoresController rankcon;
        private const int Mode = 0;
        private const int OnPage = 60;
        string oldname = "";
        string oldemail = "";

        bool savemode = false;
        // Constructor
        public LeaderBoardPage()
        {
            InitializeComponent();


            //try
            //{
                Debug.WriteLine("============ " + MainPage.usercountry);
                // System.Globalization.RegionInfo.CurrentRegion.DisplayName;
                updateUserName();
                Loaded += MainPage_Loaded;
                if (NetworkInterface.GetIsNetworkAvailable())
                {

                    authenticateCalls();
                }
                else
                {
                    btnreload.Visibility = Visibility.Visible;
                    MessageBox.Show("Oops! It appears you might not have an internet connecting on this device. Please check your network settings.", "Check Internet Settings!", MessageBoxButton.OK);
                }

            //}
            //catch (Exception ex) { 
            
            //}
        }
        private void updateUserName()
        {

            if (App.slClient.Session.User.Login != null)
            {
                MainPage.userdisplayname = App.slClient.Session.User.Login ?? string.Empty;
                MainPage.useremail = App.slClient.Session.User.Email ?? string.Empty;
                MainPage.usercountry = App.slClient.Session.User.LocationCountry ?? string.Empty;
            }

            //if (MainPage.displayname != null && MainPage.useremail != null && MainPage.usercountry != null)
            //{

             txt_displayname.Text = MainPage.userdisplayname ?? string.Empty ;
             txt_email.Text = MainPage.useremail ?? string.Empty;
             txt_countr.Text = MainPage.usercountry ?? string.Empty;
             
           
        }

        private void authenticateCalls()
        {
            if (!App.slClient.Status.IsSessionAuthenticated)
            {
                var sessionController = App.slClient.CreateSessionController();
                sessionController.RequestFailed += SessionController_RequestFailed;
                sessionController.RequestCancelled += SessionController_RequestFailed;
                // not subscribing to SessionAuthenticated, as this is already monitored
                // via App.slClient.Session.Authenticated event handler...
                sessionController.Authenticate();
                // txtStatus.Text = "Authenticating...";
            }
            loadScores();
            loadScoresKP();
            loadRank();

        }
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.slClient.Session.Authenticated += SessionAuthenticated;
            App.slClient.UnexpectedExceptionUnhandled += UnexpectedExceptionUnhandled;

            UpdateAuthenticatedUser(App.slClient.Status.IsSessionAuthenticated ? App.slClient.Session.User : null);
            //PrepareInfoScreen(App.slClient);
        }
        void SessionAuthenticated(object sender, SessionEventArgs e)
        {
            UpdateAuthenticatedUser(e.Session.User);
            UpdateSessionState(e.Session);

        }

        void UpdateAuthenticatedUser(User user)
        {
            //if (user != null)
            //   // txtStatus.Text = string.Concat("Welcome back ", user.Login, "!");
            //else
            //    //txtStatus.Text = "Not authenticated yet!";
        }

        void UpdateSessionState(Session session)
        {
            // txtSessionStatus.Text = session.IsAuthenticated ? "Authenticated" : "Not authenticated";
            // txtUserStatus.Text = session.IsAuthenticated ? session.User.Login : "unknown";
        }

        void UnexpectedExceptionUnhandled(object sender, UnexpectedExceptionEventArgs e)
        {
            //txtSessionStatus.Text = e.Exception.Message;
            //txtStatus.Text = e.Exception.Message;
            //txtUserStatus.Text = e.Exception.Message;
        }

        void SessionController_RequestFailed(object sender, RequestControllerEventArgs<IRequestController> e)
        {
            //if (e.Error.Status == StatusCode.NotFound)
            //   // txtStatus.Text = "Invalid Internet connection";
            //else
            //   // txtStatus.Text = e.Error.ToString();

            UpdateSessionState(e.Controller.Session);
        }

        public void loadScores()
        {

            CreateScoresControllerIfNeeded();

            if (_scoresController.IsProcessing)
                return;
            _scoresController.LoadScores(_scoresController.GlobalSearchList, new Range(0, OnPage), GamePage.SCORE_MODE_HIGHSCORE);

        }

        //load Koin Power Scores
        public void loadScoresKP()
        {

            CreateScoresControllerIfNeededKP();

            if (_scoresControllerKP.IsProcessing)
                return;

            _scoresControllerKP.LoadScores(_scoresControllerKP.GlobalSearchList, new Range(0, OnPage), GamePage.SCORE_MODE_KOINPOWER);
        }




        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            loadScores();
            loadRank();

        }

        private void loadRank()
        {
            CreateRankconControllerIfNeeded();

            if (rankcon.IsProcessing)
                return;
            // _scoresController.LoadScores(_scoresController.GlobalSearchList, new Range(0, OnPage), GamePage.SCORE_MODE_HIGHSCORE);
            rankcon.LoadScores(rankcon.GlobalSearchList, App.slClient.Session.User, 1, Mode);

        }

        void rankcon_ScoresLoaded(object sender, RequestControllerEventArgs<IScoresController> e)
        {
            highgrid.VerticalAlignment = VerticalAlignment.Top;
            // DataContext = _scoresController.Scores;
            //if (rankcon.Scores != null & rankcon.Scores.Length > 0)
            if (rankcon.Scores != null & rankcon.Scores.Length > 0)
            {

                Debug.WriteLine("Rank is : " + (rankcon.Scores.ElementAt(0)).Rank + App.slClient.Session.User.Email + " ");

                txt_rank.Text = (rankcon.Scores.ElementAt(0)).Rank.ToString(new CultureInfo("en-US")) + "";
                txt_points.Text = (rankcon.Scores.ElementAt(0)).Result.ToString(new CultureInfo("en-US")) + "";
                double rank = rankcon.Scores.ElementAt(0).Rank;
                string badge = "";
                if (rank > 0 && rank < 500)
                {
                    badge = "GOLD";
                }
                else if (rank > 500 && rank < 1500)
                {
                    badge = "SILVER";
                }
                else if (rank > 1500 && rank < 15000)
                {
                    badge = "BRONZE";
                }
                else if (rank > 15000)
                {
                    badge = "STRAGGLER";
                }

                txt_rankbadge.Text = badge;
                rankStoryboard.Begin();
                updateUserName();
            }
            else {
                txt_rankbadge.Text = "BEGINNER";
                txt_rank.Text = "Beginner";
            
            }
        }


        private void btnLoadPrevious_Click(object sender, RoutedEventArgs e)
        {
            CreateScoresControllerIfNeeded();

            if (_scoresController.IsProcessing)
                return;

            _scoresController.LoadPreviousRange();
        }

        private void btnLoadPrevious_ClickKP(object sender, RoutedEventArgs e)
        {
            CreateScoresControllerIfNeededKP();

            if (_scoresControllerKP.IsProcessing)
                return;

            _scoresControllerKP.LoadPreviousRange();
        }

        private void btnLoadNext_Click(object sender, RoutedEventArgs e)
        {
            CreateScoresControllerIfNeeded();

            if (_scoresController.IsProcessing)
                return;

            _scoresController.LoadNextRange();
        }
        private void btnLoadNext_ClickKP(object sender, RoutedEventArgs e)
        {
            CreateScoresControllerIfNeededKP();

            if (_scoresControllerKP.IsProcessing)
                return;

            _scoresControllerKP.LoadNextRange();
        }

        void CreateRankconControllerIfNeeded()
        {
            if (rankcon == null)
            {
                rankcon = App.slClient.CreateScoresController();
                rankcon.RequestFailed += rankcon_RequestFailed;
                rankcon.RequestCancelled += rankcon_RequestFailed;
                rankcon.ScoresLoaded += rankcon_ScoresLoaded;
            }

        }
        void CreateScoresControllerIfNeeded()
        {
            if (_scoresController == null)
            {
                _scoresController = App.slClient.CreateScoresController();
                _scoresController.RequestFailed += ScoresController_RequestFailed;
                _scoresController.RequestCancelled += ScoresController_RequestFailed;
                _scoresController.ScoresLoaded += ScoresController_ScoresLoaded;
            }
        }

        void CreateScoresControllerIfNeededKP()
        {
            if (_scoresControllerKP == null)
            {
                _scoresControllerKP = App.slClient.CreateScoresController();
                _scoresControllerKP.RequestFailed += ScoresControllerKP_RequestFailed;
                _scoresControllerKP.RequestCancelled += ScoresControllerKP_RequestFailed;
                _scoresControllerKP.ScoresLoaded += ScoresController_ScoresLoadedKP;
            }
        }

        void rankcon_RequestFailed(object sender, RequestControllerEventArgs<IRequestController> e)
        {
            // MessageBox.Show(e.Error.ToString());
            // btnreload.Visibility = Visibility.Visible;
            //MessageBox.Show("We were unable to retrieve leaderboard scores. Please try again later. \n" + e.Error.ToString(), "Oops!", MessageBoxButton.OK);
        }

        void ScoresController_RequestFailed(object sender, RequestControllerEventArgs<IRequestController> e)
        {
             MessageBox.Show(e.Error.ToString());
            btnreload.Visibility = Visibility.Visible;
            MessageBox.Show("We were unable to retrieve leaderboard scores. Please try again later. \n", "Oops!", MessageBoxButton.OK);
        }

        void ScoresControllerKP_RequestFailed(object sender, RequestControllerEventArgs<IRequestController> e)
        {
            // MessageBox.Show(e.Error.ToString());
            btnreloadKP.Visibility = Visibility.Visible;
            //MessageBox.Show("We were unable to retrieve leaderboard koin power . Please try again later. \n" , "Oops!", MessageBoxButton.OK);
        }

        void ScoresController_ScoresLoaded(object sender, RequestControllerEventArgs<IScoresController> e)
        {
            highgrid.VerticalAlignment = VerticalAlignment.Top;
            lstScores.DataContext = _scoresController.Scores;
            loadinghighscoreStack.Visibility = Visibility.Collapsed;

            myStoryboard.Begin();
            btngrid.Visibility = _scoresController.HasNextRange ? Visibility.Visible : Visibility.Collapsed;
            highgrid.RowDefinitions[0].Height = _scoresController.HasNextRange ? new GridLength(150)  : new GridLength(50);
            // btnLoad.Visibility = _scoresController.HasPreviousRange || _scoresController.HasNextRange ? Visibility.Collapsed : Visibility.Visible;
            btnLoadNext.Visibility = _scoresController.HasNextRange ? Visibility.Visible : Visibility.Collapsed;
            btnLoadPrev.Visibility = _scoresController.HasPreviousRange ? Visibility.Visible : Visibility.Collapsed;
        }
        void ScoresController_ScoresLoadedKP(object sender, RequestControllerEventArgs<IScoresController> e)
        {
            highgridKP.VerticalAlignment = VerticalAlignment.Top;
            lstScoresKP.DataContext = _scoresControllerKP.Scores;
            loadinghighscoreStackKP.Visibility = Visibility.Collapsed;
           
            // hs_height.SetValue(RowDefinition.HeightProperty, 0)  ; 
            myStoryboardKP.Begin();

            highgridKP.RowDefinitions[0].Height = _scoresControllerKP.HasNextRange ? new GridLength(150) : new GridLength(50);
            btngridkp.Visibility = _scoresControllerKP.HasNextRange ? Visibility.Visible : Visibility.Collapsed;
            // btnLoad.Visibility = _scoresController.HasPreviousRange || _scoresController.HasNextRange ? Visibility.Collapsed : Visibility.Visible;
            btnLoadNextKP.Visibility = _scoresControllerKP.HasNextRange ? Visibility.Visible : Visibility.Collapsed;
            btnLoadPrevKP.Visibility = _scoresControllerKP.HasPreviousRange ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.slClient.Status.IsSessionAuthenticated)
                return;

            var sessionController = App.slClient.CreateSessionController();
            sessionController.RequestFailed += SessionController_RequestFailed;
            sessionController.RequestCancelled += SessionController_RequestFailed;
            // not subscribing to SessionAuthenticated, as this is already monitored
            // via App.slClient.Session.Authenticated event handler...

            sessionController.Authenticate();
            //txtStatus.Text = "Authenticating...";
        }

        


        public async Task updateDisplayName(string displayname, string email)
        {
            string errordetails = "";
            string nameholder = txt_displayname.Text;
            string emailholder = txt_email.Text;
            var userController = App.slClient.CreateUserController();
            // using lambda expressions to monitor the received asynchronous result:!
            userController.RequestFailed += (sender, e) =>
            {
                Debug.WriteLine("Update login/email failed with error: " +
                 e.Error);
                UserUpdateDetailsMask detail = (UserUpdateDetailsMask)
                 e.Error.ErrorDetail;
                if ((detail + "").Equals("NameTaken"))
                {
                    errordetails = "the name you selected ( " + displayname + " ) has already been taken";
                    MessageBox.Show("Ooops! We couldnt update your display because " + errordetails + ".\nPlease select a different dispaly name. Consider adding a number or special characters.", "Name Taken", MessageBoxButton.OK);
                }
                else
                {
                    errordetails = e.Error + "";
                    MessageBox.Show("Ooops! We couldnt update your display. " + errordetails + ".\nPlease select a different dispaly name.", "Display Name error", MessageBoxButton.OK);
                }

                Debug.WriteLine("Because of " + detail);
                txt_displayname.Text = oldname;
            };

            userController.UserUpdated += (sender, e) =>
            {

                Debug.WriteLine("Brawo User updated!");
                MainPage.updateDisplayNameDB(txt_displayname.Text);
                MainPage.userdisplayname = txt_displayname.Text;
                MainPage.useremail = txt_email.Text ?? string.Empty;

                MessageBox.Show("Your leaderboard details have been updated : " + displayname + " , " + email + " !", "Congratulations!", MessageBoxButton.OK);
                loadScores();
                loadScoresKP();
            };

            await Task.Run(
                () =>
                {
                    if (email.Equals(oldemail))
                    {
                        userController.Update(nameholder, "");
                        Debug.WriteLine("updateing only username " + nameholder);
                    }
                    else
                    {
                        userController.Update(nameholder, emailholder);
                    }

                });

        }

        private void btnreload_Click(object sender, RoutedEventArgs e)
        {
            loadScores();
        }

        private void btnreload_ClickKP(object sender, RoutedEventArgs e)
        {
            loadScoresKP();
        }

        private void btn_editdetails_Click(object sender, RoutedEventArgs e)
        {
            if (!savemode)
            {

                txt_displayname.IsEnabled = true;
                txt_email.IsEnabled = true;
                // txt_countr.IsEnabled = true;
                savemode = true;
                btn_editdetails.Content = " Save Changes";
                oldname = txt_displayname.Text;
                oldemail = txt_email.Text;
            }
            else
            {

                txt_displayname.IsEnabled = false;
                txt_email.IsEnabled = false;
                txt_countr.IsEnabled = false;
                btn_editdetails.Content = "Edit Profile";
                savemode = false;

                //Update saved data
                if (txt_displayname.Text.Equals(oldname) != true || txt_email.Text.Equals(oldemail) != true)
                {
                    updateDisplayName(txt_displayname.Text, txt_email.Text);
                }
            }

        }

        private void btn_editdetails_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }


    }

}


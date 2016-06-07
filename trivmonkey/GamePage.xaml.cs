using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.ComponentModel;
using Windows.Storage;
using System.Windows.Media.Imaging;
using Microsoft.Devices;
using System.Windows.Media;
using System.Diagnostics;

namespace TrivMonkey
{
    public partial class GamePage : PhoneApplicationPage
    {

        QuestionItem currentquestion = new QuestionItem();
        IEnumerable<QuestionItem> currentset  ;
        public const int SCORE_MODE_HIGHSCORE = 0 ;
        public const int SCORE_MODE_LIFETIMESCORE = 1 ;
        public const int SCORE_MODE_KOINPOWER = 2 ;

        int cointimefactor =   20 * 60 ;
        //int cointimefactor = 1;
        //int fulltime = 60;

        public int timecounter = 0;
        public int gametime = 60;
        public static int score = 0;
        public static int wrongs = 0;
        int nontime = 0;

        private static DateTime EndTime { get; set; } 
        private DispatcherTimer dispatcherTimer;

        public GamePage()
        {

            InitializeComponent();

             var CategoryItemsInDB = (from CategoryItem subcats in MainPage.toDoDB.CategoryItems
                                        where subcats.value .Equals(MainPage.CurrentSubCategory.category)
                                        select subcats).FirstOrDefault();
           // System.Diagnostics.Debug.WriteLine("Found the lockstatus " + thecategory);
            string lockstatus = CategoryItemsInDB.lockstatus; 
            if (lockstatus.Equals("0"))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            var SubcategoryItemsInDB = (from SubCategoryItem subcats in MainPage.toDoDB.SubCategoryItems
                                        where subcats.title.Equals(MainPage.CurrentSubCategory.title)
                                        select subcats).FirstOrDefault();
            MainPage.questionidex = Int32.Parse(SubcategoryItemsInDB.lastquestion);
            txt_title.Text = MainPage.CurrentSubCategory.title;
            lbl_score.Text = score + "";
            BitmapImage bm = new BitmapImage(new Uri(@"/img/" + MainPage.CurrentSubCategory.imagelink, UriKind.RelativeOrAbsolute));
            img_thumb3.Source = bm;
        
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        { 
            base.OnBackKeyPress(e);
            var result = MessageBox.Show("Want to quit current game? Dont give up on your " + score + " points!", "Leaving ?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            { 
               // NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
               // return;
            }
            else
            {
                e.Cancel = true;
            }
             
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           // MainPage.questionidex = 0;
                generateQuestions();
                nontime = 0; ;
                score = 0;
                wrongs = 0;
                img_life1.Opacity = 1;
                img_life2.Opacity = 1;
                img_life3.Opacity = 1;
                lbl_score.Text = score + "";
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            this.dispatcherTimer.Stop();
        }
        public async Task  generateQuestions() {

         await   Task.Run(() =>
            {
                currentset = from Question in MainPage.Questions
                                where Question.subcategoryid.Equals(MainPage.CurrentSubCategory.value)
                                select Question;
                //loadUI();
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        loadUI();
                        if (this.dispatcherTimer == null)
                        {
                            this.dispatcherTimer = new DispatcherTimer();
                            this.dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
                            this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        }

                        

                        this.dispatcherTimer.Start();
                    }
                            );
                //foreach (var item in currentset)
                //{
                //    System.Diagnostics.Debug.WriteLine("Extracted : " + item.content);

                //}
            }).ConfigureAwait (continueOnCapturedContext :false );

            
        }

        void SubmitScore(int thescore)
        {
                var scoreController = App.slClient.CreateScoreController();

            scoreController.RequestFailed += (sender, e) => 
        { Debug.WriteLine("Score submission failed with error: " + e.Error); };
             scoreController.ScoreSubmitted += (sender, e) => 
        { Debug.WriteLine("Brawo! Score submitted!"); };

             // create a score with value 100.0 for 0-mode:!
            // use lifetime score as a secondary parameter on 
             var score = scoreController.CreateScore(thescore, Double.Parse( MainPage.lifetimescore) , SCORE_MODE_HIGHSCORE );
            // var lifetimescore = scoreController.CreateScore(Double.Parse(MainPage.lifetimescore), thescore , SCORE_MODE_LIFETIMESCORE);

             // and send it to the server:!
             scoreController.Submit(score);
            // scoreController.Submit(lifetimescore );
        }

        private static void SubmitKoinPower(int koinpower)
        {
            var scoreController = App.slClient.CreateScoreController();

            scoreController.RequestFailed += (sender, e) =>
            { Debug.WriteLine("KOIN submission failed with error: " + e.Error); };
            scoreController.ScoreSubmitted += (sender, e) =>
            { Debug.WriteLine("Brawo! KOIN POWER submitted! " + koinpower); };

            // create a score with value 100.0 for 0-mode:!
            var score = scoreController.CreateScore(koinpower, Double.Parse(MainPage.playtime), SCORE_MODE_KOINPOWER);

            // and send it to the server:!
            scoreController.Submit(score);
        }
        private void gameover() {
           
        this.dispatcherTimer.Stop();
        updateDatabase(timecounter, score);
                timecounter = 0;
                playSound("Audio/gameover.wav");
                NavigationService.Navigate(new Uri("/GameOverPage.xaml", UriKind.Relative));
        }

        private async Task updateDatabase(int timecount, int score)
        {
            await Task.Run(() =>
           {
               Deployment.Current.Dispatcher.BeginInvoke(() =>
               {
               //System.Diagnostics.Debug.WriteLine("Countere time " + timecount);
               var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                       select settings).FirstOrDefault();
               MainPage.playtime = (Int32.Parse(SettingItemsInDB.playtime) + timecount) + "";
               MainPage.lifetimescore = (Int32.Parse(SettingItemsInDB.score) + score) + "";
               SettingItemsInDB.playtime = MainPage.playtime;
               SettingItemsInDB.score = MainPage.lifetimescore;
               
               //Check if new playtime can be added
               int newplaytimemod = (Int32.Parse(SettingItemsInDB.playtimemod) + timecount);

               if (newplaytimemod > cointimefactor)
               {
                   //update the number of coins and update playtimemod
                   updateKoins(newplaytimemod / cointimefactor);
                   // MainPage.gamemonkeycoins = (Int32.Parse(SettingItemsInDB.mcoins) + (newplaytimemod / cointimefactor)) + "";
                   //SettingItemsInDB.mcoins = MainPage.gamemonkeycoins;
                   //SettingItemsInDB.lifetimemcoins = (Int32.Parse(SettingItemsInDB.lifetimemcoins) + (newplaytimemod / cointimefactor)) + "";
                   // System.Diagnostics.Debug.WriteLine("Coin update" + (Int32.Parse(SettingItemsInDB.mcoins) + (newplaytimemod / cointimefactor)) + "");
                   SettingItemsInDB.playtimemod = (newplaytimemod % cointimefactor) + "";
               }

               //Update games played
               MainPage.gamesplayed = (Int32.Parse(SettingItemsInDB.gamesplayed) + 1) + "";
               SettingItemsInDB.gamesplayed = MainPage.gamesplayed;



               //Check if we have a high score and act accordingly
               if (score > (Int32.Parse(SettingItemsInDB.highscore)))
               {
                   MainPage.gamehighscore = score + "";
                   SettingItemsInDB.highscore = MainPage.gamehighscore;
                   SubmitScore(score);
               }

               // MainPage.gamevibration = "1";

               //Update Last Question value
               var SubcategoryItemsInDB = (from SubCategoryItem subcats in MainPage.toDoDB.SubCategoryItems
                                           where subcats.title.Equals(MainPage.CurrentSubCategory.title)
                                           select subcats).FirstOrDefault();
               SubcategoryItemsInDB.lastquestion = MainPage.questionidex + "";

               //UPdate highscore fo this particular title
               if (score > (Int32.Parse(SubcategoryItemsInDB.highscore)))
               {
                   SubcategoryItemsInDB.highscore = score + "";
               }

               //UPdate number of times played
               SubcategoryItemsInDB.timesplayed = (Int32.Parse(SubcategoryItemsInDB.timesplayed) + 1) + "";

               MainPage.toDoDB.SubmitChanges();

               MainPage.updateCatandSubUI(); }
               );

           });
        }

        public static void updateKoins(int newkoins) {

            System.Diagnostics.Debug.WriteLine("Coin update" + newkoins  +  "");
                var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                        select settings).FirstOrDefault();
                //update the number of coins and update playtimemod
                MainPage.gamemonkeycoins = (Int32.Parse(SettingItemsInDB.mcoins) + newkoins) + "";
                SettingItemsInDB.mcoins = MainPage.gamemonkeycoins;
                SubmitKoinPower((Int32.Parse(SettingItemsInDB.lifetimemcoins) + newkoins));
                SettingItemsInDB.lifetimemcoins = ((Int32.Parse(SettingItemsInDB.lifetimemcoins) + newkoins) ) + "";
                // System.Diagnostics.Debug.WriteLine("Coin update" + (Int32.Parse(SettingItemsInDB.mcoins) + (newplaytimemod / cointimefactor)) + "");
               // SettingItemsInDB.playtimemod = (newplaytimemod % cointimefactor) + "";
               

              

                MainPage.toDoDB.SubmitChanges();
            

        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            double width = (Convert.ToDouble( timecounter) / Convert.ToDouble( gametime) ) * Convert.ToDouble( stacktimerholder.ActualWidth) ;
           // System.Diagnostics.Debug.WriteLine("Timer width " + Convert.ToInt32(  width));
            stktimeleftlabel.Width = Convert.ToInt32(width);
            if (timecounter >= gametime ){
                gameover();
            }

            if ((gametime - timecounter) < 10 || nontime > 15 ){
                vibratePhone();

            }
            lbltime.Text = (gametime - timecounter) + "s Left";
            timecounter++;
            //System.Diagnostics.Debug.WriteLine("Countere time " + timecounter);
            nontime++ ;
        }
        public void loadUI (){

            RectA.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            RectB.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            RectC.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            RectD.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];

            System.Diagnostics.Debug.WriteLine("Current question " + MainPage.questionidex);
            if ( currentset != null & currentset.Count() > 0 ){
            currentquestion = currentset.ElementAt(MainPage.questionidex );
                questionblock.Text = currentquestion.content;
                txt_opta.Text  = currentquestion.optiona  ;
                txt_optb.Text = currentquestion.optionb;
                txt_optc.Text = currentquestion.optionc;
                txt_optd.Text = currentquestion.optiond;
                MainPage.questionidex = (  MainPage.questionidex + 1) % currentset.Count()  ;
             }

        }

      

        private void vibratePhone() {
            if (MainPage.gamevibration.Equals("1"))
            {
                VibrateController.Default.Start(TimeSpan.FromMilliseconds(200));
            }
        }

        private void playSound(string soundfile) {
            if (MainPage.gamesound.Equals("1"))
            {
                Stream stream = TitleContainer.OpenStream(soundfile);
                SoundEffect effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                effect.Play();
            }
        }
        private void checkCorrect(string pressedoption) {
            nontime = 0;
            if (pressedoption.Equals(currentquestion.correct))
            {
                score+= 10 ;
                lbl_score.Text = score + "";
                playSound("Audio/right.wav");
                gametime += 2 ;
               // System.Diagnostics.Debug.WriteLine("Correct" + pressedoption  + " : " + currentquestion.correct ) ;
            }
            else {
                playSound("Audio/wrong.wav");
               // System.Diagnostics.Debug.WriteLine("Wrong " + pressedoption + " : " + currentquestion.correct);
                wrongs++;
                switch (wrongs)
                {
                    case 1:
                        img_life3.Opacity = 0;
                        break;
                    case 2:
                        img_life2.Opacity = 0;
                        break;
                    case 3:
                        img_life1.Opacity = 0;
                        gameover();
                        break;
                    default:
                        gameover();
                        break;
                }
            }

            loadUI();
        }

        

        private void RectA_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectA.Background = new SolidColorBrush(Colors.LightGray);            
        }

        private void RectA_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectA.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            checkCorrect("a");

        }

        private void RectB_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectB.Background = new SolidColorBrush(Colors.LightGray);            
        }

        private void RectB_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectB.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            checkCorrect("b");
        }

        private void RectC_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectC.Background = new SolidColorBrush(Colors.LightGray);            
        }

        private void RectC_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectC.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            checkCorrect("c");
        }

        private void RectD_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectD.Background = new SolidColorBrush(Colors.LightGray);            
        }

        private void RectD_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RectD.Background = (SolidColorBrush)App.Current.Resources["yellowrange"];
            checkCorrect("d");
        }
     }
}
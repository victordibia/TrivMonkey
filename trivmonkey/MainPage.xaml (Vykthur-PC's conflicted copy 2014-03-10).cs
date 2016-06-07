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

namespace TrivMonkey
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/triviamonkey.sdf";

        public static ListBox catlistbox;
        public static ListBox featlistbox;
        public static Popup my_popup_cs = new Popup();

        public static string lifetimescore = "0";
        public static string gamesound = "1";
        public static string gamevibration = "1";
        public static string gamehighscore= "0";
        public static string gamemonkeycoins = "0";
        public static string playtime = "0";
        public static string gamesplayed = "0";
        public static string itemsunlocked= "0";

        //Colors
        public static Color[] colorjam = new Color[] { Colors.Orange, Color.FromArgb(255, 3, 153, 61), Color.FromArgb(255, 0, 166, 216), Color.FromArgb(255, 191, 0, 34) , Colors.DarkGray};
        public static string[] colorjamnames = new string[] { "Monkey", "Leaf", "Sea" ,"Granate",  "Grayvel" };


        public static int questionidex = 0;
        public static System.Collections.ObjectModel.ObservableCollection<SubCategoryItem> SubCategories = new ObservableCollection<SubCategoryItem>();
        public static System.Collections.ObjectModel.ObservableCollection<SubCategoryItem> featuredSubCategories = new ObservableCollection<SubCategoryItem>();
        public static System.Collections.ObjectModel.ObservableCollection<CategoryItem> Categories = new ObservableCollection<CategoryItem>();
        public static System.Collections.ObjectModel.Collection<QuestionItem> Questions = new Collection<QuestionItem>();


        public static SubCategoryItem CurrentSubCategory = new SubCategoryItem();
        public static CategoryItem CurrentCategory = new CategoryItem();
        public static CategoryItem unlockCurrentCategory = new CategoryItem();

        public const string SUBCATEGORY_FILE = "subcategory.txt";
        public const string CATEGORY_FILE = "category.txt";
        public const string QUESTIONS_FILE = "questions.txt";
        public const string SCORE_FILE = "score.txt";
        public const string SUBCATEGORY_FILE_SOURCE = "subcategory.txt";
        public const string CATEGORY_FILE_SOURCE = "category.txt";
        public const string QUESTIONS_FILE_SOURCE = "questions.txt";
        public List<string> questionslist = new List<string>();

        private static readonly string IconicTileQuery = "tile=iconic";

        public static ToDoDataContext toDoDB;

        // Constructor
        public MainPage()
        {
            InitializeComponent();


           
            DataContext = App.ViewModel;


            //Read Categories and all from DB
            toDoDB = new ToDoDataContext(DBConnectionString);
 loadContentUI();
            categorieslistbox.DataContext = Categories;
            featuredlistbox.DataContext = featuredSubCategories;



            // Open database and read content only if it hasnt been read.
            System.Diagnostics.Debug.WriteLine(" UI Loaded");

           // createTile();

            //object DeviceUniqueID;

            //byte[] DeviceIDbyte = null;

            //if (UserExtendedProperties.TryGetValue("ANID2", out DeviceUniqueID))

            //    DeviceIDbyte = (byte[])DeviceUniqueID;

            //string DeviceID = Convert.ToBase64String(DeviceIDbyte);
           ;  //UserExtendedProperties.GetValue("ANID2")

          

        }

        public static string GetWindowsLiveAnonymousID()
        {
            int ANIDLength = 20;
            int ANIDOffset = 2;

            string result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID2", out anid))
            {
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                {
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                }
            }

            return result;
        }
        public static IconicTileData setTileData()
        {
            Uri tileUri = new Uri(string.Concat("/MainPage.xaml?", IconicTileQuery), UriKind.Relative);
            IconicTileData iconicTileData = new IconicTileData();
            iconicTileData.Count = Int32 .Parse (itemsunlocked) + 0 ;
            iconicTileData.BackgroundColor = ((SolidColorBrush)App.Current.Resources["yellowrange"]).Color;
            iconicTileData.Title = "Trivia Monkey";
            iconicTileData.IconImage = new Uri("img/iconimage.png", UriKind.Relative);
            iconicTileData.SmallIconImage = new Uri("img/smalliconimage.png", UriKind.Relative);
            iconicTileData.WideContent1 = "Your Trivia Monkey Stats ";
            iconicTileData.WideContent2 = "High Score : " + gamehighscore + "\nLife time score" + lifetimescore;
            iconicTileData.WideContent3 = "Items Unlocked : " + itemsunlocked ;
            return iconicTileData;
        }
        public static void deleteTile()
        { 
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
            tile => tile.NavigationUri.ToString().Contains(IconicTileQuery));
            if (shellTile != null)
            {
                shellTile.Delete();
            }
        }
        public static bool checkTile() {

            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
            tile => tile.NavigationUri.ToString().Contains(IconicTileQuery));
            if (shellTile != null)
            {
                return true;
            }

            return false;
        }
        private static void updateTIle() {
            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
             tile => tile.NavigationUri.ToString().Contains(IconicTileQuery));
            if (shellTile != null)
            {
              shellTile.Update(setTileData());
          }
        
        }
        public static void createTile() {

            ShellTile shellTile = ShellTile.ActiveTiles.FirstOrDefault(
             tile => tile.NavigationUri.ToString().Contains(IconicTileQuery));
 
            //ShellTile iconicTile = this.FindTile(IconicTileQuery);
            if (shellTile == null)
            {
                Uri tileUri = new Uri(string.Concat("/MainPage.xaml?", IconicTileQuery), UriKind.Relative);
                ShellTile.Create(tileUri, setTileData(), true);

            }
            else { 
             
                shellTile.Update (setTileData()) ;
            }

        }
        public static async Task updateCatandSubUI() {
            updateTIle();
            Categories.Clear();
            SubCategories.Clear();
            featuredSubCategories.Clear();
            await Task.Run(
                () =>
                {

             var CategoryItemsInDB = from CategoryItem categories in toDoDB.CategoryItems
                                        select categories;
              foreach (CategoryItem item in CategoryItemsInDB)
                 {
                     Deployment.Current.Dispatcher.BeginInvoke(() =>
                        Categories.Add(item)
                        );
                     ;
                 }

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {  // Load Subcategories from DB
                 var SubCategoryItemsInDB = from SubCategoryItem subcategories in toDoDB.SubCategoryItems
                                            select subcategories;
                 foreach (SubCategoryItem item in SubCategoryItemsInDB)
                 {
                     
                        
                                SubCategories.Add(item);
                             if (item.featured.Equals("1"))
                             {
                                 featuredSubCategories.Add(item);
                             }
                        }
                        
                     ;

                     
                 }
                 );

                });

        
        }
        private void loadSettings() {

            var SettingItemsInDB = (from SettingItem settings in toDoDB.SettingItems
                                   select settings).FirstOrDefault();
            if (SettingItemsInDB == null)
            {

                toDoDB.SettingItems.InsertOnSubmit(new SettingItem
                {
                    score = "0",
                    themecolor = "0",
                    mcoins = "0",
                    vibration = "1",
                    gamesplayed = "0" ,
                    playtime = "0" ,
                    playtimemod = "0" ,
                    itemsunlocked = "0",
                    highscore = "0" , 
                    gamesound = "1" 



                });
                toDoDB.SubmitChanges();
                highscorelabel.Text = "0";
                lifetimescorelabel.Text = "0";
                monkeycoinslabel.Text = "5";
                playtimelabel.Text = "0";
                
            }
            else
            {
               // new SolidColorBrush(thi); n
                lifetimescore = SettingItemsInDB.score;
                SolidColorBrush brush = (SolidColorBrush)App.Current.Resources["yellowrange"];
                brush.Color = MainPage.colorjam[Int32 .Parse (SettingItemsInDB.themecolor)];

                gamesplayed = SettingItemsInDB.gamesplayed; 
                gamesound = SettingItemsInDB.gamesound; 
                gamevibration= SettingItemsInDB.vibration;  
                gamehighscore = SettingItemsInDB.highscore;  
                lifetimescore  = SettingItemsInDB.score;  
                gamemonkeycoins  = SettingItemsInDB.mcoins;  
                playtime = SettingItemsInDB.playtime;
                itemsunlocked = SettingItemsInDB.itemsunlocked;
            }

            loadStatistics();
        }

        public void loadStatistics() {
             updateTIle();
            highscorelabel.Text = gamehighscore;
            lifetimescorelabel.Text = lifetimescore;
            gamesplayedlabel.Text = gamesplayed;
            monkeycoinslabel.Text = gamemonkeycoins;
            itemsunlockedlabel.Text = itemsunlocked;
            statlabel.Text = "Unlock stuff with your " + gamemonkeycoins + " koins!";
            //playtimelabel.Text = playtime ;

            int lifescore = Int32.Parse(lifetimescore);
            if (lifescore > 1000)
            {
                lifescoredesig.Text = "k";
                playtimelabel.Text = (lifescore / 1000).ToString ("#.#") + "";
            }
            else {
                lifescoredesig.Text = "pts";
                playtimelabel.Text = lifetimescore;
            }

            int thetime = Int32.Parse(playtime);
            if (thetime > 60)
            {
                timedesig.Text = "min(s)";
                playtimelabel.Text = (thetime / 60).ToString("#.#") + "";
            }
            else if (thetime > (60 * 60))
            {
                timedesig.Text = "hours";
                playtimelabel.Text = (thetime / (60 * 60)).ToString("#.#") + "";
            }
            else if (thetime > (60 * 60 * 24))
            {
                timedesig.Text = "day(s)";
                playtimelabel.Text = (thetime / (60 * 60 * 24)).ToString("#.#") + "";
            }
            else
            {
                timedesig.Text = "sec(s)";
                playtimelabel.Text = (thetime) + "";
            } 
        }

        public void loadContentUI()
        {


            if (SubCategories.Count() <= 0)
            {

                // Load categories from DB
                var CategoryItemsInDB = from CategoryItem categories in toDoDB.CategoryItems
                                        select categories;

                // System.Diagnostics.Debug.WriteLine( CategoryItemsInDB.Count()  + " Subcount less than zero ");
                if (CategoryItemsInDB.Count() > 0)
                {
                    foreach (CategoryItem item in CategoryItemsInDB)
                    {
                        Categories.Add(item);
                    }

                    // Load Subcategories from DB
                    var SubCategoryItemsInDB = from SubCategoryItem subcategories in toDoDB.SubCategoryItems
                                               select subcategories;
                    foreach (SubCategoryItem item in SubCategoryItemsInDB)
                    {
                        SubCategories.Add(item);
                        if (item.featured.Equals("1"))
                        {
                            featuredSubCategories.Add(item);
                        }
                    }

                    // Load Questions from DB
                    var QuestionItemsInDB = from QuestionItem questions in toDoDB.QuestionItems
                                            select questions;
                    foreach (QuestionItem item in QuestionItemsInDB)
                    {
                        Questions.Add(item);
                    }

                    loadingCategoriesStack.Visibility = Visibility.Collapsed;
                    loadingFeaturedStack.Visibility = Visibility.Collapsed;

                    myStoryboard.Begin();

                    //Load Lifetime Score and ThemeColor
                    loadSettings();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("About to populate database");
                    populateDB();

                }
                //Show the Featured List
                // FadeIn(featuredlistbox);
            }
            else {
                loadingCategoriesStack.Visibility = Visibility.Collapsed;
                loadingFeaturedStack.Visibility = Visibility.Collapsed;
                myStoryboard.Begin();
                loadStatistics();
               
                
            }
        }

        public async Task loadDataFromFile()
        {
            
            string[] filesarray = new string[3] { SUBCATEGORY_FILE, CATEGORY_FILE, QUESTIONS_FILE };
            string[] filesourcearray = new string[3] { SUBCATEGORY_FILE_SOURCE, CATEGORY_FILE_SOURCE, QUESTIONS_FILE_SOURCE };

            for (int i = 0; i < filesarray.Length; i++)
            {
                var readfiletask = Task.Run(async () =>
                {
                    await loadTestFile(filesarray[i], filesarray[i]);
                });

                readfiletask.Wait();

            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>       loadContentUI()                     );
        }

        private async Task loadTestFile(string source, string destination)
        {

            var res = App.GetResourceStream(new Uri(source, UriKind.Relative));
            string txt = new StreamReader(res.Stream).ReadToEnd();
            updateDataModel(txt, destination);

        }



        void updateDataModel(string filecontent, string category)
        {

            switch (category)
            {
                case SUBCATEGORY_FILE:
                    var jsondata = JObject.Parse(filecontent)["subcategory"];
                    foreach (var item in jsondata)
                    {
                        using (ToDoDataContext db = new ToDoDataContext(MainPage.DBConnectionString))
                        {

                            // Add a Categories to db
                            db.SubCategoryItems.InsertOnSubmit(new SubCategoryItem
                            {
                                title = item["title"] + "",
                                description = item["description"] + "",
                                value = item["value"] + "",
                                category = item["category"] + "",
                                lastquestion = "0" ,
                                highscore = "0" ,
                                timesplayed = "0" ,
                                imagelink = item["imagelink"] + "",
                                featured = item["featured"] + ""

                            });

                            db.SubmitChanges();
                        }
                        //Deployment.Current.Dispatcher.BeginInvoke(() =>
                        //    {
                        //        SubCategories.Add(new Subcategory(item["title"] + "", item["category"] + "", item["description"] + "", item["value"] + "", new DateTime(2008, 2, 5), item["imagelink"] + "", item["featured"] + ""));
                        //        System.Diagnostics.Debug.WriteLine(item["featured"]);

                        //        if ( (item["featured"]+"").Equals ("1"))
                        //        {

                        //           // featuredSubCategories.Add(new Subcategory(item["title"] + "", item["category"] + "", item["description"] + "", item["value"] + "", new DateTime(2008, 2, 5), item["imagelink"] + "", item["featured"] + ""));
                        //        }
                        //    }
                        //     );
                    }
                    break;
                case CATEGORY_FILE:
                    var jsoncatdata = JObject.Parse(filecontent)["category"];
                    foreach (var item in jsoncatdata)
                    {

                        using (ToDoDataContext db = new ToDoDataContext(MainPage.DBConnectionString))
                        {

                            // Add a Categories to db
                            db.CategoryItems.InsertOnSubmit(new CategoryItem
                            {
                                title = item["title"] + "",
                                description = item["description"] + "",
                                value = item["value"] + "",
                                mcoins  = item["mcoins"] + "",
                                lockstatus = item["lockstatus"] + "",
                                imagelink = item["imagelink"] + ""

                            });
                            db.SubmitChanges();
                        }

                    }
                    break;
                case QUESTIONS_FILE:
                    //  System.Diagnostics.Debug.WriteLine(filecontent);

                    var jsonqdata = JObject.Parse(filecontent)["questions"];
                    foreach (var item in jsonqdata)
                    {

                        using (ToDoDataContext db = new ToDoDataContext(MainPage.DBConnectionString))
                        {

                            // Add a Categories to db
                            db.QuestionItems.InsertOnSubmit(new QuestionItem
                            {
                                content = item["content"] + "",
                                optiona = item["optiona"] + "",
                                optionb = item["optionb"] + "",
                                optionc = item["optionc"] + "",
                                optiond = item["optiond"] + "",
                                correct = item["correct"] + "",
                                explanation = item["explanation"] + "",
                                subcategoryid = item["subcategoryid"] + "",
                                categoryid = item["categoryid"] + ""

                            });
                            db.SubmitChanges();
                        }


                        //Deployment.Current.Dispatcher.BeginInvoke(() =>

                        //    Questions.Add (new Question ( item["content"] + "", item["optiona"] + "" ,item["optionb"] + "" ,
                        //        item["optionc"] + "" ,item["optiond"] + "" ,item["correct"] + "" ,
                        //        item["categoryid"] + "" ,item["subcategoryid"] + "" , item["explanation"] + "" )) ) ;

                    }
                    break;
                default: break;
            }



        }




        void getApiData(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {

                    WebResponse response = request.EndGetResponse(result);
                    using (StreamReader httpwebStreamReader = new StreamReader(response.GetResponseStream()))
                    {
                        string results = httpwebStreamReader.ReadToEnd();
                        //updateDataModel(results);

                        // WriteTextFile("category/category.json", results);

                    }
                    response.Close();
                }
                catch (WebException e)
                {
                    // gamerTag = "Gamertag not found.";
                    return;
                }
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {



        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (my_popup_cs.IsOpen == true)
            {
                System.Diagnostics.Debug.WriteLine("Backpressed "  );
                my_popup_cs.IsOpen = false;
            }
            //e.Cancel = true;
        }
        public static void launchGame(string thecategory, SubCategoryItem holder)
        {  
            //Hold your horses, we check first check if the sibcat is unlocked
            //Update Last Question value
            var CategoryItemsInDB = (from CategoryItem subcats in MainPage.toDoDB.CategoryItems
                                        where subcats.value .Equals(holder.category)
                                        select subcats).FirstOrDefault();
           // System.Diagnostics.Debug.WriteLine("Found the lockstatus " + thecategory);
            string lockstatus = CategoryItemsInDB.lockstatus;

            unlockCurrentCategory = CategoryItemsInDB;
            System.Diagnostics.Debug.WriteLine(lockstatus + "Clicked is lock");
            if (lockstatus.Equals("0"))
            {
                
               // MessageBox.Show(" This Category is locked"); int
                display_cspopup(CategoryItemsInDB.title, "unlock", Int32.Parse(CategoryItemsInDB.mcoins));
            }
            else
            {
                CurrentSubCategory = holder;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            }

          
            // CurrentSubCategoryid = 
        }

        private void playbuttonHandler(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                SubCategoryItem subcatholder  = button.DataContext as SubCategoryItem;
                launchGame(subcatholder.category, subcatholder);
                // System.Diagnostics.Debug.WriteLine( currentsubcat .title + currentsubcat .value + currentsubcat .description ); 
                //  App.ViewModel.DeleteToDoItem(toDoForDelete);
            }

           
        }

        private void sharebuttonHandler(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                CurrentSubCategory = button.DataContext as SubCategoryItem;
                // System.Diagnostics.Debug.WriteLine( currentsubcat .title + currentsubcat .value + currentsubcat .description ); 
                //  App.ViewModel.DeleteToDoItem(toDoForDelete);
                ShareStatusTask sst = new ShareStatusTask();
                sst.Status = "I have played the " + CurrentSubCategory.title + " quiz on Trivia Monkey for Windows Phone app! Check it out!";
                sst.Show();
            }
        }

        private void CategorybuttonHandler(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                CurrentCategory = button.DataContext as CategoryItem;

            }

            NavigationService.Navigate(new Uri("/CategoryPage.xaml", UriKind.Relative));
            // CurrentSubCategoryid = 
        }

        private void rateHandler(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void howtoHandler(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HowtoPage.xaml", UriKind.Relative));
        }

        private void creditsClickHandler(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreditsPage.xaml", UriKind.Relative));
        }

        private void btnrecommend_Click(object sender, RoutedEventArgs e)
        {
            var task = new EmailComposeTask { To = "", Subject = "Have you played Trivia Monkey", Body = "One of the most fun and engaging trivia games out there ... Give it a try, its free! \n\n Warm Regards." };
            task.Show();
        }

        private void settingsHandler(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        public static void display_cspopup(string itemtitle, string condition, int coinvalue)
        {
            Border border = new Border();                                                     // to create green color border
            border.BorderBrush = new SolidColorBrush(Colors.Green);
            border.BorderThickness = new Thickness(2);
            border.Margin = new Thickness(10, 10, 10, 10);

            Grid totalgrid = new Grid();
           
            SolidColorBrush gridback = new SolidColorBrush(Colors.Black);
            gridback.Opacity = 0.8;

            totalgrid.SetValue(Grid.BackgroundProperty, gridback );

            totalgrid.Height = Application.Current.Host.Content.ActualHeight;
            totalgrid.Width = Application.Current.Host.Content.ActualWidth;
            totalgrid.VerticalAlignment = VerticalAlignment.Center; 
             
           // totalgrid.Opacity = 0.8;


            Style style = (Style)Application.Current.Resources["OptionButtonStyle"];

            StackPanel skt_pnl_outter = new StackPanel();                             // stack panel 
            skt_pnl_outter.VerticalAlignment = VerticalAlignment.Center;
            //skt_pnl_outter.Background = new SolidColorBrush(Colors.LightGray);
            skt_pnl_outter.Orientation = System.Windows.Controls.Orientation.Vertical;

            Image img_disclaimer = new Image();                                       // Image
            img_disclaimer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            img_disclaimer.Height = 100;
            img_disclaimer.Width = 100;
            img_disclaimer.Stretch = Stretch. UniformToFill;
            img_disclaimer.Margin = new Thickness(0, 15, 0, 5);
            Uri uriR = new Uri("img/unlock.png", UriKind.Relative);
            BitmapImage imgSourceR = new BitmapImage(uriR);
            img_disclaimer.Source = imgSourceR;

            TextBlock txt_blk1 = new TextBlock();                                         // Textblock 1
            txt_blk1.Text = "Unlock New Item ";
            txt_blk1 .TextAlignment = TextAlignment.Center ;
            txt_blk1.TextAlignment = TextAlignment.Center;
            txt_blk1.FontSize = 46;
            txt_blk1.Margin = new Thickness(10, 0, 30, 0);
            txt_blk1.Foreground = new SolidColorBrush(Colors.White);

            SolidColorBrush brush = (SolidColorBrush)App.Current.Resources["yellowrange"];
            TextBlock txt_blk3 = new TextBlock();                                      // Textblock 2
            txt_blk3.Text =  itemtitle.ToUpper() ;
            txt_blk3.TextAlignment = TextAlignment.Center;
            txt_blk3 .TextAlignment = TextAlignment.Center ;
            txt_blk3.FontSize = 32;
            //txt_blk3.FontWeight = FontWeights.Bold;
            txt_blk3.Foreground = brush;//new SolidColorBrush(Colors.Orange);
            txt_blk3.Margin = new Thickness(10, 20, 10, 20);
             
            string unlockmessage = "";
            TextBlock txt_blk2 = new TextBlock();
            StackPanel skt_pnl_inner = new StackPanel();
            skt_pnl_inner.Margin =   new Thickness(0, 20, 0, 0);
            Button btn_continue = new Button();
            Button btn_cancel = new Button();

            if (condition.Equals("unlock"))
            {

                if ( (coinvalue) > Int32.Parse(gamemonkeycoins))
                {
                    unlockmessage = "You need " + (coinvalue - Int32.Parse(gamemonkeycoins)) + " more MONKEY KOIN(S) to unlock this item. Continue playing to earn koins or buy from the store.";

                    btn_continue.Content = "Buy Koins";
                    btn_continue.Style = style;
                    btn_continue.Width = 215;
                    btn_continue.Margin = new Thickness(0, 0, 15, 0);
                    btn_continue.Click += new RoutedEventHandler(btn_buy_Click);

                    // Button cancel                                     
                    btn_cancel.Content = "Continue";
                    btn_cancel.Style = style;
                    btn_cancel.Width = 215;
                    btn_cancel.Click += new RoutedEventHandler(btn_continue_Click);
                }
                else
                {
                    unlockmessage = "You are about to spend " + coinvalue  + " KOINS to unlock this item. \n";
                    btn_continue.Content = "Unlock";
                    btn_continue.Style = style;
                    btn_continue.Width = 215;
                    btn_continue.Margin = new Thickness(0, 0, 15, 0);
                    btn_continue.Tag = coinvalue;
                     btn_continue.Click += new RoutedEventHandler(btn_unlock_Click);

                    // Button cancel                                     
                    btn_cancel.Content = "Cancel";
                    btn_cancel.Style = style;
                    btn_cancel.Width = 215;
                    btn_cancel.Margin = new Thickness(0, 0, 15, 0);
                    btn_cancel.Click += new RoutedEventHandler(btn_continue_Click);

                }
            }
            else {

                unlockmessage = "Congratulations! " + itemtitle .ToUpper () + " has been unlocked. Click done to go back to menu";
                btn_continue.Visibility = Visibility.Collapsed;
                btn_cancel.Content = "Done";
                btn_cancel.Style = style;
                btn_cancel.Width = 215;
                btn_cancel.Margin = new Thickness(0, 20, 15, 0);
                btn_cancel.Click += new RoutedEventHandler(btn_done_Click);

            }


            txt_blk2.Text = unlockmessage;
            txt_blk2.TextWrapping = TextWrapping.Wrap;
            txt_blk2.TextAlignment = TextAlignment.Center;
            txt_blk2.TextAlignment = TextAlignment.Center;
            txt_blk2.FontSize = 21;
            txt_blk2.Margin = new Thickness(20, 0, 20, 0);
            txt_blk2.Foreground = new SolidColorBrush(Colors.White);


            //Adding control to stack panel
            totalgrid.Children.Add(skt_pnl_outter);
            skt_pnl_outter.Children.Add(img_disclaimer);
            skt_pnl_outter.Children.Add(txt_blk1);
            skt_pnl_outter.Children.Add(txt_blk3);
            skt_pnl_outter.Children.Add(txt_blk2);

              
            skt_pnl_inner.HorizontalAlignment = HorizontalAlignment.Center;
            skt_pnl_inner.Orientation = System.Windows.Controls.Orientation.Horizontal;


            skt_pnl_inner.Children.Add(btn_continue);
            skt_pnl_inner.Children.Add(btn_cancel);


            skt_pnl_outter.Children.Add(skt_pnl_inner);

            // Adding stackpanel  to border
            //border.Child = skt_pnl_outter;

            // Adding border to pup-up
            my_popup_cs.Child = totalgrid ;

            

            my_popup_cs.IsOpen = true;
        }
        public static void performUnlock(string mcoins) { 
        
            //update coin data
            var SettingItemsInDB = (from SettingItem settings in MainPage.toDoDB.SettingItems
                                    select settings).FirstOrDefault(); 
            gamemonkeycoins = (Int32.Parse(gamemonkeycoins) - Int32.Parse(mcoins)) + "" ;
            itemsunlocked = (Int32.Parse(itemsunlocked) + 1) + "";
            SettingItemsInDB.itemsunlocked = itemsunlocked;
            SettingItemsInDB.mcoins= gamemonkeycoins;
             
            //update the category
            var CategoryItemsInDB = (from  CategoryItem subcats in MainPage.toDoDB. CategoryItems
                                        where subcats.value.Equals(MainPage.CurrentSubCategory.category)
                                        select subcats).FirstOrDefault();
            CategoryItemsInDB.lockstatus = "1";

            MainPage.toDoDB.SubmitChanges();
            MainPage.updateCatandSubUI();
        }

        private static void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            if (my_popup_cs.IsOpen)
            {
                my_popup_cs.IsOpen = false;
            }
        }

        private static void btn_buy_Click(object sender, RoutedEventArgs e)
        {
            if (my_popup_cs.IsOpen)
            {
                my_popup_cs.IsOpen = false;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/BuyPage.xaml", UriKind.Relative));
            }
        }

        private static void btn_done_Click(object sender, RoutedEventArgs e)
        {
            if (my_popup_cs.IsOpen)
            {
                my_popup_cs.IsOpen = false;
                //(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private static void btn_unlock_Click(object sender, RoutedEventArgs e)
        {
            
            if (my_popup_cs.IsOpen)
            {
                
                performUnlock(CurrentCategory.mcoins) ;
                my_popup_cs.IsOpen = false;
                display_cspopup(unlockCurrentCategory.title, "congrats", Int32.Parse(unlockCurrentCategory.mcoins));
            }
        }
        public async Task populateDB()
        {
            // Create the database if it does not exist.

            await Task.Run(
                () =>
                {
                    loadDataFromFile();

                }

                );



        }

        private void creditsbutton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BuyPage.xaml", UriKind.Relative));
        }

    }
}
 

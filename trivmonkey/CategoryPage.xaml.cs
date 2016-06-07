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
using System.ComponentModel;

namespace TrivMonkey
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        IEnumerable<SubCategoryItem> currentset;

        public CategoryPage()
        {
            InitializeComponent();
           // DataContext = App.ViewModel;
            ////
            txt_title.Text = MainPage.CurrentCategory.title;
            lbltitle.Text = (MainPage.CurrentCategory.title + " CATEGORY").ToUpper();

            System.Diagnostics.Debug.WriteLine(MainPage.SubCategories.Count() + " " + MainPage.CurrentCategory.value);
            currentset = from SubCategoryItem  in MainPage.SubCategories
                         where SubCategoryItem.category.Equals(MainPage.CurrentCategory.value)
                         select SubCategoryItem;

            if(MainPage.CurrentCategory.lockstatus.Equals ("0")){
                lockimage.Visibility = Visibility.Visible;
                txt_title.Visibility = Visibility.Visible; 
            }
            //currentset = from SubcategoryItem in MainPage.SubCategories
            //             where SubcategoryItem.category.Equals(MainPage.CurrentCategory.value)
            //             select SubcategoryItem;




            System.Collections.ObjectModel.ObservableCollection<SubCategoryItem> SubCategories = new ObservableCollection<SubCategoryItem>(currentset);
            featuredlistbox.DataContext = SubCategories;

            lblitems.Text = currentset.Count() + " Item(s)";
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (MainPage.my_popup_cs.IsOpen == true)
            {
                System.Diagnostics.Debug.WriteLine("Backpressed ");
                MainPage.my_popup_cs.IsOpen = false;
                e.Cancel = true;
            }
            //e.Cancel = true;
        }

        private void playbuttonHandler(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                MainPage.subcatholder = button.DataContext as SubCategoryItem;
               // launchGame(subcatholder.category, subcatholder);
              //  SubCategoryItem subcatholder = button.DataContext as SubCategoryItem;
                MainPage.launchGame(MainPage.subcatholder.category, MainPage.subcatholder);// System.Diagnostics.Debug.WriteLine( currentsubcat .title + currentsubcat .value + currentsubcat .description ); 
                //  App.ViewModel.DeleteToDoItem(toDoForDelete);
            }

          //  NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            // CurrentSubCategoryid = 
        }

        private void sharebuttonHandler(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                MainPage.CurrentSubCategory = button.DataContext as SubCategoryItem;
                // System.Diagnostics.Debug.WriteLine( currentsubcat .title + currentsubcat .value + currentsubcat .description ); 
                //  App.ViewModel.DeleteToDoItem(toDoForDelete);
                ShareStatusTask sst = new ShareStatusTask();
                sst.Status = "I have just finished playing a " + MainPage.CurrentSubCategory.title + " quiz on Trivmonkey for Windows Phone! Check it out!";
                sst.Show();
            }
        }
    }
}
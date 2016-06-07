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
    public partial class CreditsPage : PhoneApplicationPage
    {
        

        public CreditsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            

        } 
    }
}
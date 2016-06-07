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

namespace TrivMonkey
{
    public partial class HowtoPage : PhoneApplicationPage
    {
       
        public HowtoPage()
        {
            InitializeComponent();

           

            DataContext = App.ViewModel;
            Tblock1.Text = "Trivia Monkey is a fun time challenge game where you answer questions on interesting and fun topics and score as many points as you can before the time runs out. \n" +
"As faces are different .. so are interests also different! Thus we have diverse categories such as history, sports, science, TV Series, and reality shows each complete with different titles! Select the title of your choice and ride on!  \n" +
"Your goal is to answer each question correctly while earning game points and monkey koins . Each time you answer a question correctly, you get game points and bonus time added to your remaining time. A new question will appear once you answer a question. Wrong answers will not attract any points. \n" +
"The game will end when the time runs out or when you lose all of your available tries. \n" +
"Level up by playing regularly and scoring high. Earn monkey points by recommending Trivia Monkey to friends and sharing your scores on social networks. \n" ;


            Tblock3.Text = "You earn MONKEY KOINS by playing! 20 minutes of play automatically earns you one MONKEY KOIN! You can then use your MONKEY KOINs to unlock items within the game such as new categories for play! . \n" +
  " The more you play, the greater your ability to unlock the hidden prizes within Trivia Monkey! \n" ;

            txt_leaderboard.Text = "You have done a great job earning those highscores and gathering monkey koins! Now its your chance to shine and take your position at the top of the leaderboards so everyone knows who's boss! \n" +
                "On the highscore leaderboard, you take get to the top by having both the best highscore and lifetime score! \n" +
                "On the koin power leaderboard you take the top spot by earning lots of koins and your play time dedication! Time to get going! \n\n\n";

        } 
    }
}
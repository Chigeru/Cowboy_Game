using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cowboy_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int timeRound = 1;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer NPCtimer = new DispatcherTimer();
        Random rand = new Random();
        Cowboy player1;
        Cowboy player2;
        string dificulty = "";

        public MainWindow(string playerName, string bandit)
        {
            InitializeComponent();
            dificulty = bandit;
            player1 = new Cowboy(playerName);
            player2 = new Cowboy("The Bandit");


            // Indsætter den baggrund med den valgte bandit på en sketchy måde
            gridHolder.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/Western Background - " + dificulty + ".jpg")));

            // Ved en svære bandit - vil knappen først komme frem når man kan skyde + random location
            HardButtonRelocation();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            if(dificulty.ToLower() == "hard") { NPCtimer.Interval = TimeSpan.FromSeconds(1); } 
            else { NPCtimer.Interval = TimeSpan.FromMilliseconds(300); }
            
            NPCtimer.Tick += Bandit_Tick;



            lblClock.Content = "Wait for highnoon";
            ResetTimer();
            timeRound = rand.Next(2, 4);

            //Image missed = new Image();
            //missed.Source = new BitmapImage(new Uri("Missed"));
            //missed.Margin = new Thickness(90, 145, 0, 0);


        }

        // Handlinger der sker hvert sekundt på timeren
        void timer_Tick(object sender, EventArgs e)
        {
            int seconds = 60 - timeRound;
            if (seconds < 60)
            {
                lblClock.Content = $"11:59:{seconds}";
                timeRound--;
            }
            else
            {
                seconds -= 60;
                lblClock.Content = $"12:00:{seconds}";
                if (btnFire.Visibility == Visibility.Hidden) { btnFire.Visibility = Visibility.Visible; }
                if (NPCtimer.IsEnabled == false)
                {
                    NPCtimer.Start();
                }
                timeRound--;
            }
        }

        // Hnandlingen der sker hvert interval på NPCtimeren
        void Bandit_Tick(object sender, EventArgs e)
        {
            FireShot(player2, player1, rand.Next(0, 100));
        }

        // reset bruges når en spiller er ramt
        public void ResetTimer()
        {
            NPCtimer.Stop();
            timeRound = rand.Next(4, 7);
            if (dificulty.ToLower() == "hard") { btnFire.Visibility = Visibility.Hidden; }
        }

        void HardButtonRelocation()
        {
            if (dificulty.ToLower() == "hard")
            {
                btnFire.Margin = new Thickness(rand.Next(20, 400), rand.Next(20, 400), 0, 0);
            }
        }

        private void btnFire_Click(object sender, RoutedEventArgs e)
        {
            int seconds = 60 - timeRound;
            if (seconds >= 60) { FireShot(player1, player2, rand.Next(0, 100)); }
        }

        void FireShot(Cowboy cowboy, Cowboy loser, int hitChance)
        {
            Console.WriteLine($"{cowboy.name}: {hitChance}");
            if (hitChance > 10)
            {
                if (cowboy.wounded)
                {
                    if (hitChance > 30)
                    {
                        //cowboy.Fire();      // Grafik
                        
                        Ellipse point = new Ellipse();
                        point.Height = 30;
                        point.Width = 30;
                        point.Margin = new Thickness(5, 0, 5, 0);
                        
                        if (loser.wounded) { GameEnd(cowboy); }
                        else { loser.wounded = true; }
                        ResetTimer();
                        // grafik hvis man rammer ved siden af
                    }
                }
                else
                {
                    // cowboy.Fire();       // Grafik
                    if (loser.wounded) { GameEnd(cowboy); }
                    else { loser.wounded = true; }
                    ResetTimer();
                }
            }

            HardButtonRelocation();
        }

        void GameEnd(Cowboy cowboy)
        {
            NPCtimer.Stop();
            timer.Stop();
            MessageBox.Show($"{cowboy.name} won the duel!");
            System.Windows.Application.Current.Shutdown();
        }
    }
}

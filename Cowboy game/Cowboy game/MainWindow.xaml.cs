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
        string difficulty = "";

        public MainWindow(string playerName, string bandit)
        {
            InitializeComponent();

            difficulty = bandit.Substring(0, 4);
            player1 = new Cowboy(playerName);
            player2 = new Cowboy(bandit.Substring(7, bandit.Length - 7));

            // Indsætter den baggrund med den valgte bandit på en sketchy måde
            gridHolder.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/Western Background - " + difficulty + ".jpg")));

            // Ved en svære bandit - vil knappen først komme frem når man kan skyde + random location
            HardButtonRelocation();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            if (difficulty.ToLower() == "hard") { NPCtimer.Interval = TimeSpan.FromMilliseconds(800); }
            else { NPCtimer.Interval = TimeSpan.FromMilliseconds(300); }

            NPCtimer.Tick += Bandit_Tick;

            lblClock.Content = "Wait";
            ResetTimer();

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
            Image missed = (Image)VisualTreeHelper.GetChild(gridHolder, 3);
            Image missed1 = (Image)VisualTreeHelper.GetChild(gridHolder, 4);
            missed.Source = null;
            missed1.Source = null;
            timer.Stop();
            NPCtimer.Stop();
            timeRound = rand.Next(2, 4);
            if (difficulty.ToLower() == "hard") { btnFire.Visibility = Visibility.Hidden; }
            timer.Start();
        }

        void HardButtonRelocation()
        {
            if (difficulty.ToLower() == "hard")
            {
                btnFire.Margin = new Thickness(rand.Next(20, 400), rand.Next(20, 380), 0, 0);
            }
        }

        private void btnFire_Click(object sender, RoutedEventArgs e)
        {
            int seconds = 60 - timeRound;
            if (seconds >= 60) { FireShot(player1, player2, rand.Next(0, 100)); }
        }

        void FireShot(Cowboy cowboy, Cowboy loser, int hitChance)
        {
            if (hitChance > 5)
            {
                if (cowboy.wounded)
                {
                    if (hitChance > 25)
                    {
                        //cowboy.Fire();      // Grafik
                        // Indsætter point
                        Ellipse point = new Ellipse();
                        point.Height = 30;
                        point.Width = 30;
                        point.Margin = new Thickness(5, 0, 5, 0);
                        point.Fill = new SolidColorBrush(Color.FromRgb(200, 0, 128));
                        StackPanel pointHolder = (StackPanel)VisualTreeHelper.GetChild(PointContainer, cowboy.playerID);

                        pointHolder.Children.Add(point);
                        if (loser.wounded) { GameEnd(cowboy); }
                        else { loser.wounded = true; }
                        ResetTimer();
                        // grafik hvis man rammer ved siden af
                    }
                    else
                    {
                        Image missed = (Image)VisualTreeHelper.GetChild(gridHolder, cowboy.playerID + 3);
                        missed.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/Missed.jpg"));
                    }
                }
                else
                {
                    // cowboy.Fire();       // Grafik
                    // Indsætter point
                    Ellipse point = new Ellipse();
                    point.Height = 30;
                    point.Width = 30;
                    point.Margin = new Thickness(5, 0, 5, 0);
                    point.Fill = new SolidColorBrush(Color.FromRgb(200, 0, 128));
                    StackPanel pointHolder = (StackPanel)VisualTreeHelper.GetChild(PointContainer, cowboy.playerID);
                    pointHolder.Children.Add(point);

                    if (loser.wounded) { GameEnd(cowboy); }
                    else { loser.wounded = true; }
                    ResetTimer();
                }
            }
            else
            {
                Image missed = (Image)VisualTreeHelper.GetChild(gridHolder, cowboy.playerID + 3);
                missed.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/Missed.jpg"));
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

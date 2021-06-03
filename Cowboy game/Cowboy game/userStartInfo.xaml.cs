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
using System.Windows.Shapes;

namespace Cowboy_game
{
    /// <summary>
    /// Interaction logic for userStartInfo.xaml
    /// </summary>
    public partial class userStartInfo : Window
    {
        public userStartInfo()
        {
            InitializeComponent();
            txtGameInfo.Text = "Game Information:\n" +
                "Skyd banditen når klokken slår 12.00.\n" +
                "Spillet er bedst ud af 3.\n";
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string checkedRadio = "";
            if(Bandit0.IsChecked == true) { checkedRadio = Bandit0.Content.ToString(); } 
            else if(Bandit1.IsChecked == true) { checkedRadio = Bandit1.Content.ToString(); }
            if(txtPlayerName.Text != "")
            {
                if (Bandit0.IsChecked == true || Bandit1.IsChecked == true)
                {
                    Window gameWindow = new MainWindow(txtPlayerName.Text, checkedRadio);
                    gameWindow.Show();
                    this.Close();
                }
            }
            // error besked
        }
    }
}

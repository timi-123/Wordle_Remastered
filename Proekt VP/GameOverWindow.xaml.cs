using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proekt_VP
{
    public partial class GameOverWindow : Window
    {
        public bool PlayAgain { get; set; } = false;
        public GameOverWindow(string message, int score)
        {
            InitializeComponent();
            MessageText.Text = message;
            ScoreText.Text = $"Final score: {score}";
        }

        private void PlayAgainClick(object sender, RoutedEventArgs e)
        {
            PlayAgain = true;
            this.Close();
        }

        private void MainMenuClick(object sender, RoutedEventArgs e)
        {
            PlayAgain = false;
            this.Close();
        }
    }
}

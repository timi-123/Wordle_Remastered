using System.Windows;
using System.Windows.Controls;
using Proekt_VP.ViewModels;

namespace Proekt_VP
{
    public partial class SinglePlayerView : UserControl
    {
        public SinglePlayerView()
        {
            InitializeComponent();
        }

        private void StartGame(int minutes)
        {
            SinglePlayerViewModel model= new SinglePlayerViewModel("HELLO", minutes);
            model.GameOver += ModelGameOver;
            DataContext = model;
            DurationSelection.Visibility = Visibility.Collapsed;
            Board.Visibility = Visibility.Visible;
        }

        private void ModelGameOver(object? sender,EventArgs e)
        {
            if(sender is SinglePlayerViewModel model)
            {
                model.GameOver -= ModelGameOver;
                GameOverWindow popup = new GameOverWindow(model.ErrorMessage, 40) //da smenam tuka so model.Score koga ke implementiram logika za presmetuvanje poeni
                {
                    Owner = Window.GetWindow(this)
                };
                popup.ShowDialog();

                if (popup.PlayAgain)
                {
                    Board.Visibility = Visibility.Collapsed;
                    DurationSelection.Visibility = Visibility.Visible;
                }
                else
                {
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    if (mainWindow != null)
                    {
                        mainWindow.ChangeView(new MainMenuView());
                    }
                }
            }
        }

        private void OneMinuteClick(object sender, RoutedEventArgs e)
        {
            StartGame(1);
        }
        private void TwoMinutesClick(object sender, RoutedEventArgs e)
        {
            StartGame(2);
        }
        private void ThreeMinutesClick(object sender, RoutedEventArgs e)
        {
            StartGame(3);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.ChangeView(new MainMenuView());
        }
    }
}

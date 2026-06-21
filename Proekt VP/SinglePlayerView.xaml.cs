using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Proekt_VP.ViewModels;

namespace Proekt_VP
{
    public partial class SinglePlayerView : UserControl
    {
        private SinglePlayerViewModel? currentModelInstance;
        public SinglePlayerView()
        {
            InitializeComponent();
            Unloaded += SinglePlayerView_Unloaded;
        }

        private void SinglePlayerView_Unloaded(object sender, RoutedEventArgs e)
        {
            currentModelInstance?.StopTimer();
        }

        private void StartGame(int minutes)
        {
            currentModelInstance?.StopTimer();
            SinglePlayerViewModel model= new SinglePlayerViewModel("HELLO", minutes);
            model.GameOver += ModelGameOver;
            currentModelInstance = model;
            DataContext = model;
            DurationSelection.Visibility = Visibility.Collapsed;
            //Board.Visibility = Visibility.Visible;
            //TimerAndScore.Visibility = Visibility.Visible;
            Game.Visibility = Visibility.Visible;

            Focus();
        }

        private void KeyDownSPV(object sender, KeyEventArgs e)
        {
            if(DataContext is not SinglePlayerViewModel model)
            {
                return;
            }

            if (e.Key == Key.Enter)
            {
                model.EnterLetterCommand.Execute("ENTER");
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                model.EnterLetterCommand.Execute("⌫");
                e.Handled = true;
            }
            else if (e.Key>=Key.A && e.Key <= Key.Z)
            {
                model.EnterLetterCommand.Execute(e.Key.ToString());
                e.Handled = true;
            }
        }

        private void ModelGameOver(object? sender,EventArgs e)
        {
            if(sender is SinglePlayerViewModel model)
            {
                model.GameOver -= ModelGameOver;
                GameOverWindow popup = new GameOverWindow("Time's up!", model.Score) //da smenam tuka so model.Score koga ke implementiram logika za presmetuvanje poeni
                {
                    Owner = Window.GetWindow(this)
                };
                popup.ShowDialog();

                if (popup.PlayAgain)
                {
                    //Board.Visibility = Visibility.Collapsed;
                    //TimerAndScore.Visibility = Visibility.Collapsed;
                    Game.Visibility = Visibility.Collapsed;
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
            currentModelInstance?.StopTimer();
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.ChangeView(new MainMenuView());
        }
    }
}

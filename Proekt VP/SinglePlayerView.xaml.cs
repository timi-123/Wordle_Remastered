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
            DataContext = new SinglePlayerViewModel("HELLO",minutes);
            DurationSelection.Visibility = Visibility.Collapsed;
            Board.Visibility = Visibility.Visible;
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

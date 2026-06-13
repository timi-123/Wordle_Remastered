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
            DataContext = new SinglePlayerViewModel("HELLO");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.ChangeView(new MainMenuView());
        }
    }
}

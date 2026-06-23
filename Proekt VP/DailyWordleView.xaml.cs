using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Proekt_VP.ViewModels;

namespace Proekt_VP
{
    public partial class DailyWordleView : UserControl
    {
        public DailyWordleView()
        {
            InitializeComponent();

            DataContext = new UnlimitedWordleViewModel();

            Loaded += DailyWordleView_Loaded;
        }

        private void DailyWordleView_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            Focus();
            Keyboard.Focus(this);
        }

        private void DailyWordleView_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (DataContext is not UnlimitedWordleViewModel model)
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
            else if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                model.EnterLetterCommand.Execute(e.Key.ToString());
                e.Handled = true;
            }
        }

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow? mainWindow =
                Window.GetWindow(this) as MainWindow;

            mainWindow?.ChangeView(new MainMenuView());
        }
    }
}
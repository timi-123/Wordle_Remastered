using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proekt_VP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content= new MainMenuView();
        }
        public void ChangeView(UserControl view)
        {
            MainContent.Content = view;
            if(view is MainMenuView)
            {
                Title = "Wordle Main Menu";
            }
            if(view is DailyWordleView)
            {
                Title = "Daily (Unlimited) Wordle";
            }
            if(view is SinglePlayerView)
            {
                Title = "Single Player Wordle";
            }
            if (view is TwoPlayerView)
            {
                Title = "Two Player Wordle";
            }
        }

        
    }
}
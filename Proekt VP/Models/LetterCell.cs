using Proekt_VP.ViewModels;
using System.Windows.Media;

namespace Proekt_VP.Models
{
    public class LetterCell : ViewModelBase
    {
        private string _letter = "";
        public string Letter
        {
            get => _letter;
            set => SetProperty(ref _letter, value);
        }

        private Brush _backgroundColor = Brushes.Transparent;
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }
    }
}
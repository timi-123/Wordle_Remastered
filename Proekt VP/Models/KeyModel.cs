using Proekt_VP.ViewModels;
using System.Windows.Media;

namespace Proekt_VP.Models
{
    public class KeyModel : ViewModelBase
    {
        private string _key = "";
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        private Brush _backgroundColor = Brushes.LightGray;
        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }
    }
}
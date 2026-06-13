using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Proekt_VP.Models;

namespace Proekt_VP.ViewModels
{
    public class SinglePlayerViewModel : ViewModelBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string _targetWord;
        private int _currentRowIndex = 0;
        private int _currentColIndex = 0;

        public ObservableCollection<ObservableCollection<LetterCell>> Guesses { get; }
        public ObservableCollection<KeyModel> KeyboardKeys { get; }
        public ObservableCollection<ObservableCollection<KeyModel>> KeyboardRows { get; }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand EnterLetterCommand { get; }
        public ICommand SubmitGuessCommand { get; }
        public ICommand BackspaceCommand { get; }

        public SinglePlayerViewModel(string targetWord = "HELLO")
        {
            _targetWord = targetWord.ToUpper();

            Guesses = new ObservableCollection<ObservableCollection<LetterCell>>();
            for (int r = 0; r < 6; r++)
            {
                var row = new ObservableCollection<LetterCell>();
                for (int c = 0; c < 5; c++)
                {
                    row.Add(new LetterCell());
                }
                Guesses.Add(row);
            }

            KeyboardKeys = new ObservableCollection<KeyModel>();
            KeyboardRows = new ObservableCollection<ObservableCollection<KeyModel>>();

            var row1 = new ObservableCollection<KeyModel>();
            foreach (char c in "QWERTYUIOP")
            {
                var key = new KeyModel { Key = c.ToString() };
                KeyboardKeys.Add(key);
                row1.Add(key);
            }
            KeyboardRows.Add(row1);

            var row2 = new ObservableCollection<KeyModel>();
            foreach (char c in "ASDFGHJKL")
            {
                var key = new KeyModel { Key = c.ToString() };
                KeyboardKeys.Add(key);
                row2.Add(key);
            }
            KeyboardRows.Add(row2);

            var row3 = new ObservableCollection<KeyModel>();
            var enterKey = new KeyModel { Key = "ENTER" };
            KeyboardKeys.Add(enterKey);
            row3.Add(enterKey);

            foreach (char c in "ZXCVBNM")
            {
                var key = new KeyModel { Key = c.ToString() };
                KeyboardKeys.Add(key);
                row3.Add(key);
            }

            var backKey = new KeyModel { Key = "⌫" };
            KeyboardKeys.Add(backKey);
            row3.Add(backKey);

            KeyboardRows.Add(row3);

            EnterLetterCommand = new RelayCommand(OnEnterLetter);
            SubmitGuessCommand = new RelayCommand(o => OnSubmitGuess());
            BackspaceCommand = new RelayCommand(o => OnBackspace());
        }

        private void OnEnterLetter(object? parameter)
        {
            ErrorMessage = ""; 
            if (parameter is string key)
            {
                if (key == "ENTER")
                {
                    OnSubmitGuess();
                    return;
                }

                if (key == "⌫" || key == "BACK")
                {
                    OnBackspace();
                    return;
                }

                if (_currentRowIndex < 6 && _currentColIndex < 5)
                {
                    Guesses[_currentRowIndex][_currentColIndex].Letter = key;
                    _currentColIndex++;
                }
            }
        }

        private void OnBackspace()
        {
            ErrorMessage = ""; 
            if (_currentColIndex > 0)
            {
                _currentColIndex--;
                Guesses[_currentRowIndex][_currentColIndex].Letter = "";
            }
        }

        private async void OnSubmitGuess()
        {
            if (_currentColIndex != 5) return; 

            string guess = string.Join("", Guesses[_currentRowIndex].Select(c => c.Letter));

            bool isValid = await IsValidWordAsync(guess);
            if (!isValid)
            {
                ErrorMessage = "Not in word list";
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                char guessedChar = guess[i];
                var cell = Guesses[_currentRowIndex][i];

                Brush cellColor;
                if (guessedChar == _targetWord[i])
                {
                    cellColor = Brushes.Green;
                }
                else if (_targetWord.Contains(guessedChar))
                {
                    cellColor = Brushes.Goldenrod; 
                }
                else
                {
                    cellColor = Brushes.DarkGray;
                }

                cell.BackgroundColor = cellColor;

                var keyModel = KeyboardKeys.FirstOrDefault(k => k.Key == guessedChar.ToString());
                if (keyModel != null)
                {
                    if (cellColor == Brushes.Green)
                    {
                        keyModel.BackgroundColor = Brushes.Green;
                    }
                    else if (cellColor == Brushes.Goldenrod && keyModel.BackgroundColor != Brushes.Green)
                    {
                        keyModel.BackgroundColor = Brushes.Goldenrod;
                    }
                    else if (cellColor == Brushes.DarkGray && keyModel.BackgroundColor != Brushes.Green && keyModel.BackgroundColor != Brushes.Goldenrod)
                    {
                        keyModel.BackgroundColor = Brushes.DarkGray;
                    }
                }
            }

            _currentRowIndex++;
            _currentColIndex = 0;
        }

        public async Task<bool> IsValidWordAsync(string word)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"https://api.dictionaryapi.dev/api/v2/entries/en/{word.ToLower()}"
                );
                return response.IsSuccessStatusCode; 
            }
            catch (HttpRequestException)
            {
                return true; 
            }
        }
    }
}
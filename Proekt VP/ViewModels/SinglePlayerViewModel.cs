 using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Proekt_VP.Models;

namespace Proekt_VP.ViewModels
{
    public class SinglePlayerViewModel : ViewModelBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private static string[] wordList = {
            "HELLO", "WORLD", "BRAIN", "CRANE", "PLANT", "SHINE", "GRAPE", "STONE",
            "FLAME", "BRAVE", "CLOUD", "DRIFT", "EARTH", "FAITH", "GIANT", "HONEY",
            "INPUT", "JEWEL", "KNIFE", "LEMON", "MAGIC", "NIGHT", "OCEAN", "PEACE",
            "QUEEN", "RIVER", "SMILE", "TIGER", "ULTRA", "VAPOR", "WATER", "XENON",
            "YACHT", "ZEBRA", "ANGEL", "BEACH", "CANDY", "DANCE", "EAGLE", "FROST",
            "GLOBE", "HEART", "IVORY", "JUICE", "KARMA", "LIGHT", "MONEY", "NORTH",
            "OLIVE", "PANIC", "QUOTA", "RADAR", "SUGAR", "TABLE", "UNCLE", "VIDEO",
            "WATCH", "EXTRA", "YOUTH", "ZONAL", "ALBUM", "BLANK", "CHAIR", "DREAM",
            "EIGHT", "FLOOD", "GRACE", "HAPPY", "IMAGE", "JOKER", "KNEEL", "LASER",
            "MANOR", "NOBLE", "ORBIT", "PIANO", "QUIET", "RAPID", "SALAD", "THINK"
        };
        private static Random random = new Random();
        private string _targetWord;
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

        //Logika za timer
        private readonly DispatcherTimer timer;
        private TimeSpan timeRemaining;

        public string TimeRemainingText => timeRemaining.ToString(@"mm\:ss");

        private bool _isGameOver;
        public bool isGameOver
        {
            get => _isGameOver;
            set => SetProperty(ref _isGameOver, value);
        }

        public event EventHandler? GameOver;
        private void EndGame(string message)
        {
            timer.Stop();
            isGameOver = true;
            ErrorMessage = message;
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private static int[] AttemptBonuses = { 60, 50, 40, 30, 20, 10 };

        private int score;
        public int Score
        {
            get => score;
            set => SetProperty(ref score, value);
        }

        private int wordc;
        public int wordCount
        {
            get => wordc;
            set => SetProperty(ref wordc, value);
        }

        private void ClearBoard()
        {
            foreach(ObservableCollection<LetterCell> guessedWord in Guesses)
            {
                foreach(LetterCell cell in guessedWord)
                {
                    cell.Letter = "";
                    cell.BackgroundColor = Brushes.Transparent;
                }
            }

            foreach(KeyModel key in KeyboardKeys)
            {
                key.BackgroundColor = Brushes.LightGray;
            }

            _currentRowIndex = 0;
            _currentColIndex = 0;
            ErrorMessage = "";

            string newWord="";
            do
            {
                newWord = wordList[random.Next(wordList.Length)];
            } while (newWord == _targetWord);

            _targetWord = newWord;

            listForGreenLetters.Clear();
            listForYellowLetters.Clear();
        }

        public SinglePlayerViewModel(string targetWord = "HELLO", int durationMinutes=5)
        {
            _targetWord = wordList[random.Next(wordList.Length)];

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


            timeRemaining = TimeSpan.FromMinutes(durationMinutes);
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timeRemaining -= TimeSpan.FromSeconds(1);
            OnPropertyChanged(nameof(TimeRemainingText));

            if (timeRemaining <= TimeSpan.Zero)
            {
                EndGame($"Time's up! The word was {_targetWord}");
            }
        }

        private void OnEnterLetter(object? parameter)
        {
            if (isGameOver)
            {
                return;
            }
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

        private List<int> listForGreenLetters = new List<int>();
        private List<int> listForYellowLetters = new List<int>();

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


            List<char> remainingLetters = _targetWord.ToList();
            for(int i = 0; i < 5; i++)
            {
                if (guess[i] == _targetWord[i])
                {
                    remainingLetters.Remove(guess[i]);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                char guessedChar = guess[i];
                var cell = Guesses[_currentRowIndex][i];

                Brush cellColor;
                if (guessedChar == _targetWord[i])
                {
                    cellColor = Brushes.Green;
                    if (!listForGreenLetters.Contains(i))
                    {
                        Score += 5;
                        listForGreenLetters.Add(i);
                    }
                }
                else if (remainingLetters.Contains(guessedChar))
                {
                    cellColor = Brushes.Goldenrod;
                    remainingLetters.Remove(guessedChar);
                    if (!listForYellowLetters.Contains(i))
                    {
                        Score += 2;
                        listForYellowLetters.Add(i);
                    }
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

            if (guess == _targetWord)
            {
                Score += AttemptBonuses[_currentRowIndex];
                wordCount++;
                isGameOver = true;
                ErrorMessage = "Correct word!";
                await Task.Delay(1500);
                isGameOver = false;
                ClearBoard();
                return;
            }

            _currentRowIndex++;
            _currentColIndex = 0;

            if (_currentRowIndex == 6 && guess!=_targetWord)
            {
                isGameOver = true;
                ErrorMessage = $"The word was {_targetWord}";
                await Task.Delay(1500);
                isGameOver = false;
                ClearBoard();
                return;
            }

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

        //da impementiram funkcija za zapiranje na timer za da ne vrti vo pozadina nekoj timer sto ne zavrsil i da ja povikuvam vo SinglePlayerView
        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
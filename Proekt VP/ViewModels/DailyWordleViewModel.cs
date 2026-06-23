using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Proekt_VP.Models;

namespace Proekt_VP.ViewModels
{
    public class UnlimitedWordleViewModel : ViewModelBase
    {
        private static readonly string[] WordList =
        {
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

        private static readonly Random Random = new Random();

        private string _targetWord = "";
        private int _currentRowIndex;
        private int _currentColIndex;
        private bool _isSubmitting;

        public ObservableCollection<ObservableCollection<LetterCell>> Guesses { get; }

        public ObservableCollection<KeyModel> KeyboardKeys { get; }

        public ObservableCollection<ObservableCollection<KeyModel>> KeyboardRows { get; }

        private string _errorMessage = "";

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isGameOver;

        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                SetProperty(ref _isGameOver, value);
                OnPropertyChanged(nameof(IsGameActive));
            }
        }

        public bool IsGameActive => !IsGameOver;

        // -------------------------------------------------
        // SESSION STATISTICS
        // -------------------------------------------------

        private int _gamesPlayed;

        public int GamesPlayed
        {
            get => _gamesPlayed;
            set
            {
                SetProperty(ref _gamesPlayed, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int _gamesWon;

        public int GamesWon
        {
            get => _gamesWon;
            set
            {
                SetProperty(ref _gamesWon, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int _gamesLost;

        public int GamesLost
        {
            get => _gamesLost;
            set => SetProperty(ref _gamesLost, value);
        }

        private int _wonIn1;

        public int WonIn1
        {
            get => _wonIn1;
            set => SetProperty(ref _wonIn1, value);
        }

        private int _wonIn2;

        public int WonIn2
        {
            get => _wonIn2;
            set => SetProperty(ref _wonIn2, value);
        }

        private int _wonIn3;

        public int WonIn3
        {
            get => _wonIn3;
            set => SetProperty(ref _wonIn3, value);
        }

        private int _wonIn4;

        public int WonIn4
        {
            get => _wonIn4;
            set => SetProperty(ref _wonIn4, value);
        }

        private int _wonIn5;

        public int WonIn5
        {
            get => _wonIn5;
            set => SetProperty(ref _wonIn5, value);
        }

        private int _wonIn6;

        public int WonIn6
        {
            get => _wonIn6;
            set => SetProperty(ref _wonIn6, value);
        }

        public double WinPercentage =>
            GamesPlayed == 0
                ? 0
                : (double)GamesWon / GamesPlayed * 100;

        // -------------------------------------------------
        // COMMANDS
        // -------------------------------------------------

        public ICommand EnterLetterCommand { get; }
        public ICommand SubmitGuessCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand NewGameCommand { get; }

        public UnlimitedWordleViewModel()
        {
            Guesses =
                new ObservableCollection<ObservableCollection<LetterCell>>();

            for (int rowIndex = 0; rowIndex < 6; rowIndex++)
            {
                var row = new ObservableCollection<LetterCell>();

                for (int columnIndex = 0; columnIndex < 5; columnIndex++)
                {
                    row.Add(new LetterCell());
                }

                Guesses.Add(row);
            }

            KeyboardKeys = new ObservableCollection<KeyModel>();

            KeyboardRows =
                new ObservableCollection<ObservableCollection<KeyModel>>();

            CreateKeyboard();

            EnterLetterCommand =
                new RelayCommand(OnEnterLetter);

            SubmitGuessCommand =
                new RelayCommand(_ => OnSubmitGuess());

            BackspaceCommand =
                new RelayCommand(_ => OnBackspace());

            NewGameCommand =
                new RelayCommand(_ => StartNewGame());

            StartNewGame();
        }

        // -------------------------------------------------
        // KEYBOARD CREATION
        // -------------------------------------------------

        private void CreateKeyboard()
        {
            var firstRow = new ObservableCollection<KeyModel>();

            foreach (char letter in "QWERTYUIOP")
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                firstRow.Add(key);
            }

            KeyboardRows.Add(firstRow);

            var secondRow = new ObservableCollection<KeyModel>();

            foreach (char letter in "ASDFGHJKL")
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                secondRow.Add(key);
            }

            KeyboardRows.Add(secondRow);

            var thirdRow = new ObservableCollection<KeyModel>();

            var enterKey = new KeyModel
            {
                Key = "ENTER"
            };

            KeyboardKeys.Add(enterKey);
            thirdRow.Add(enterKey);

            foreach (char letter in "ZXCVBNM")
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                thirdRow.Add(key);
            }

            var backspaceKey = new KeyModel
            {
                Key = "⌫"
            };

            KeyboardKeys.Add(backspaceKey);
            thirdRow.Add(backspaceKey);

            KeyboardRows.Add(thirdRow);
        }

        // -------------------------------------------------
        // START NEW GAME
        // -------------------------------------------------

        public void StartNewGame()
        {
            string previousWord = _targetWord;

            do
            {
                _targetWord =
                    WordList[Random.Next(WordList.Length)];
            }
            while (WordList.Length > 1 &&
                   _targetWord == previousWord);

            foreach (ObservableCollection<LetterCell> row in Guesses)
            {
                foreach (LetterCell cell in row)
                {
                    cell.Letter = "";
                    cell.BackgroundColor = Brushes.Transparent;
                }
            }

            foreach (KeyModel key in KeyboardKeys)
            {
                key.BackgroundColor = Brushes.LightGray;
            }

            _currentRowIndex = 0;
            _currentColIndex = 0;
            _isSubmitting = false;

            ErrorMessage = "";
            IsGameOver = false;

            // Session statistics intentionally stay unchanged.
        }

        // -------------------------------------------------
        // LETTER INPUT
        // -------------------------------------------------

        private void OnEnterLetter(object? parameter)
        {
            if (IsGameOver ||
                _isSubmitting ||
                parameter is not string key)
            {
                return;
            }

            ErrorMessage = "";

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

            if (key.Length != 1)
            {
                return;
            }

            if (_currentRowIndex < 6 &&
                _currentColIndex < 5)
            {
                Guesses[_currentRowIndex][_currentColIndex].Letter =
                    key.ToUpper();

                _currentColIndex++;
            }
        }

        private void OnBackspace()
        {
            if (IsGameOver || _isSubmitting)
            {
                return;
            }

            ErrorMessage = "";

            if (_currentColIndex > 0)
            {
                _currentColIndex--;

                Guesses[_currentRowIndex][_currentColIndex].Letter = "";
            }
        }

        // -------------------------------------------------
        // SUBMIT GUESS
        // -------------------------------------------------

        private async void OnSubmitGuess()
        {
            if (IsGameOver || _isSubmitting)
            {
                return;
            }

            if (_currentColIndex != 5)
            {
                ErrorMessage = "Enter a five-letter word.";
                return;
            }

            _isSubmitting = true;

            try
            {
                string guess = string.Join(
                    "",
                    Guesses[_currentRowIndex]
                        .Select(cell => cell.Letter));

                bool isValid =
                    await Services.WordValidator.IsValidWordAsync(guess);

                if (!isValid)
                {
                    ErrorMessage = "Not in word list";
                    return;
                }

                EvaluateGuess(guess);

                if (guess == _targetWord)
                {
                    int attemptNumber = _currentRowIndex + 1;

                    GamesPlayed++;
                    GamesWon++;

                    RegisterWin(attemptNumber);

                    ErrorMessage =
                        $"Correct! You guessed the word in " +
                        $"{attemptNumber} attempt(s).";

                    IsGameOver = true;
                    return;
                }

                _currentRowIndex++;
                _currentColIndex = 0;

                if (_currentRowIndex >= 6)
                {
                    GamesPlayed++;
                    GamesLost++;

                    ErrorMessage =
                        $"The word was {_targetWord}";

                    IsGameOver = true;
                }
            }
            catch (Exception)
            {
                ErrorMessage =
                    "An error occurred while checking the word.";
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        // -------------------------------------------------
        // STATISTICS
        // -------------------------------------------------

        private void RegisterWin(int attemptNumber)
        {
            switch (attemptNumber)
            {
                case 1:
                    WonIn1++;
                    break;

                case 2:
                    WonIn2++;
                    break;

                case 3:
                    WonIn3++;
                    break;

                case 4:
                    WonIn4++;
                    break;

                case 5:
                    WonIn5++;
                    break;

                case 6:
                    WonIn6++;
                    break;
            }
        }

        // -------------------------------------------------
        // GUESS EVALUATION
        // -------------------------------------------------

        private void EvaluateGuess(string guess)
        {
            List<char> remainingLetters =
                _targetWord.ToList();

            // First remove the letters that are in the
            // correct position.
            for (int index = 0; index < 5; index++)
            {
                if (guess[index] == _targetWord[index])
                {
                    remainingLetters.Remove(guess[index]);
                }
            }

            for (int index = 0; index < 5; index++)
            {
                char guessedCharacter = guess[index];

                LetterCell cell =
                    Guesses[_currentRowIndex][index];

                Brush cellColor;

                if (guessedCharacter == _targetWord[index])
                {
                    cellColor = Brushes.Green;
                }
                else if (remainingLetters.Contains(guessedCharacter))
                {
                    cellColor = Brushes.Goldenrod;
                    remainingLetters.Remove(guessedCharacter);
                }
                else
                {
                    cellColor = Brushes.DarkGray;
                }

                cell.BackgroundColor = cellColor;

                UpdateKeyboardColor(
                    guessedCharacter.ToString(),
                    cellColor);
            }
        }

        // -------------------------------------------------
        // KEYBOARD COLOR
        // -------------------------------------------------

        private void UpdateKeyboardColor(
            string keyText,
            Brush newColor)
        {
            KeyModel? keyModel =
                KeyboardKeys.FirstOrDefault(
                    key => key.Key == keyText);

            if (keyModel == null)
            {
                return;
            }

            if (newColor == Brushes.Green)
            {
                keyModel.BackgroundColor = Brushes.Green;
            }
            else if (newColor == Brushes.Goldenrod &&
                     keyModel.BackgroundColor != Brushes.Green)
            {
                keyModel.BackgroundColor =
                    Brushes.Goldenrod;
            }
            else if (newColor == Brushes.DarkGray &&
                     keyModel.BackgroundColor != Brushes.Green &&
                     keyModel.BackgroundColor != Brushes.Goldenrod)
            {
                keyModel.BackgroundColor =
                    Brushes.DarkGray;
            }
        }
    }
}
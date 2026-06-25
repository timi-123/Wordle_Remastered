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
        private static readonly string[] wordList =
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

        private static readonly Random random = new Random();

        private string targetWord = "";
        private int currentRow;
        private int currentColumn;
        private bool isSubmitting;

        public ObservableCollection<ObservableCollection<LetterCell>> Guesses { get; }
        public ObservableCollection<KeyModel> KeyboardKeys { get; }
        public ObservableCollection<ObservableCollection<KeyModel>> KeyboardRows { get; }

        private string errorMessage = "";

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        private bool isGameOver;

        public bool IsGameOver
        {
            get => isGameOver;
            set
            {
                SetProperty(ref isGameOver, value);
                OnPropertyChanged(nameof(IsGameActive));
            }
        }

        public bool IsGameActive => !IsGameOver;

        private int gamesPlayed;

        public int GamesPlayed
        {
            get => gamesPlayed;
            set
            {
                SetProperty(ref gamesPlayed, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int gamesWon;

        public int GamesWon
        {
            get => gamesWon;
            set
            {
                SetProperty(ref gamesWon, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int gamesLost;

        public int GamesLost
        {
            get => gamesLost;
            set => SetProperty(ref gamesLost, value);
        }

        private int wonIn1;
        private int wonIn2;
        private int wonIn3;
        private int wonIn4;
        private int wonIn5;
        private int wonIn6;

        public int WonIn1
        {
            get => wonIn1;
            set
            {
                SetProperty(ref wonIn1, value);
                UpdateDistribution();
            }
        }

        public int WonIn2
        {
            get => wonIn2;
            set
            {
                SetProperty(ref wonIn2, value);
                UpdateDistribution();
            }
        }

        public int WonIn3
        {
            get => wonIn3;
            set
            {
                SetProperty(ref wonIn3, value);
                UpdateDistribution();
            }
        }

        public int WonIn4
        {
            get => wonIn4;
            set
            {
                SetProperty(ref wonIn4, value);
                UpdateDistribution();
            }
        }

        public int WonIn5
        {
            get => wonIn5;
            set
            {
                SetProperty(ref wonIn5, value);
                UpdateDistribution();
            }
        }

        public int WonIn6
        {
            get => wonIn6;
            set
            {
                SetProperty(ref wonIn6, value);
                UpdateDistribution();
            }
        }

        public double WinPercentage
        {
            get
            {
                if (GamesPlayed == 0)
                    return 0;

                return (double)GamesWon / GamesPlayed * 100;
            }
        }

        public double Distribution1Width => CalculateBarWidth(WonIn1);
        public double Distribution2Width => CalculateBarWidth(WonIn2);
        public double Distribution3Width => CalculateBarWidth(WonIn3);
        public double Distribution4Width => CalculateBarWidth(WonIn4);
        public double Distribution5Width => CalculateBarWidth(WonIn5);
        public double Distribution6Width => CalculateBarWidth(WonIn6);

        private int MaxDistributionValue
        {
            get
            {
                return new[]
                {
                    WonIn1,
                    WonIn2,
                    WonIn3,
                    WonIn4,
                    WonIn5,
                    WonIn6
                }.Max();
            }
        }

        public ICommand EnterLetterCommand { get; }
        public ICommand SubmitGuessCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand NewGameCommand { get; }

        public UnlimitedWordleViewModel()
        {
            Guesses =
                new ObservableCollection<ObservableCollection<LetterCell>>();

            for (int i = 0; i < 6; i++)
            {
                var row = new ObservableCollection<LetterCell>();

                for (int j = 0; j < 5; j++)
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
                new RelayCommand(_ => SubmitGuess());

            BackspaceCommand =
                new RelayCommand(_ => Backspace());

            NewGameCommand =
                new RelayCommand(_ => StartNewGame());

            StartNewGame();
        }

        private void CreateKeyboard()
        {
            AddKeyboardRow("QWERTYUIOP");
            AddKeyboardRow("ASDFGHJKL");

            var lastRow = new ObservableCollection<KeyModel>();

            var enterKey = new KeyModel
            {
                Key = "ENTER"
            };

            KeyboardKeys.Add(enterKey);
            lastRow.Add(enterKey);

            foreach (char letter in "ZXCVBNM")
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                lastRow.Add(key);
            }

            var backspaceKey = new KeyModel
            {
                Key = "⌫"
            };

            KeyboardKeys.Add(backspaceKey);
            lastRow.Add(backspaceKey);

            KeyboardRows.Add(lastRow);
        }

        private void AddKeyboardRow(string letters)
        {
            var row = new ObservableCollection<KeyModel>();

            foreach (char letter in letters)
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                row.Add(key);
            }

            KeyboardRows.Add(row);
        }

        public void StartNewGame()
        {
            string oldWord = targetWord;

            do
            {
                targetWord =
                    wordList[random.Next(wordList.Length)];
            }
            while (wordList.Length > 1 &&
                   targetWord == oldWord);

            foreach (var row in Guesses)
            {
                foreach (var cell in row)
                {
                    cell.Letter = "";
                    cell.BackgroundColor =
                        Brushes.Transparent;
                }
            }

            foreach (var key in KeyboardKeys)
            {
                key.BackgroundColor =
                    Brushes.LightGray;
            }

            currentRow = 0;
            currentColumn = 0;
            isSubmitting = false;

            ErrorMessage = "";
            IsGameOver = false;
        }

        private void OnEnterLetter(object? parameter)
        {
            if (IsGameOver ||
                isSubmitting ||
                parameter is not string key)
            {
                return;
            }

            ErrorMessage = "";

            if (key == "ENTER")
            {
                SubmitGuess();
                return;
            }

            if (key == "⌫" || key == "BACK")
            {
                Backspace();
                return;
            }

            if (key.Length != 1)
            {
                return;
            }

            if (currentRow < 6 &&
                currentColumn < 5)
            {
                Guesses[currentRow][currentColumn].Letter =
                    key.ToUpper();

                currentColumn++;
            }
        }

        private void Backspace()
        {
            if (IsGameOver || isSubmitting)
            {
                return;
            }

            ErrorMessage = "";

            if (currentColumn > 0)
            {
                currentColumn--;

                Guesses[currentRow][currentColumn].Letter = "";
            }
        }

        private async void SubmitGuess()
        {
            if (IsGameOver || isSubmitting)
            {
                return;
            }

            if (currentColumn != 5)
            {
                ErrorMessage =
                    "Enter a five-letter word.";

                return;
            }

            isSubmitting = true;

            try
            {
                string guess = string.Join(
                    "",
                    Guesses[currentRow]
                        .Select(cell => cell.Letter)
                );

                bool isValid =
                    await Services.WordValidator
                        .IsValidWordAsync(guess);

                if (!isValid)
                {
                    ErrorMessage = "Not in word list";
                    return;
                }

                EvaluateGuess(guess);

                if (guess == targetWord)
                {
                    int attempt = currentRow + 1;

                    GamesPlayed++;
                    GamesWon++;

                    AddWinToStatistics(attempt);

                    ErrorMessage =
                        $"Correct! You guessed the word in " +
                        $"{attempt} attempt(s).";

                    IsGameOver = true;
                    return;
                }

                currentRow++;
                currentColumn = 0;

                if (currentRow == 6)
                {
                    GamesPlayed++;
                    GamesLost++;

                    ErrorMessage =
                        $"The word was {targetWord}";

                    IsGameOver = true;
                }
            }
            catch
            {
                ErrorMessage =
                    "An error occurred while checking the word.";
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private void AddWinToStatistics(int attempt)
        {
            switch (attempt)
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

        private double CalculateBarWidth(int value)
        {
            int maximumValue = MaxDistributionValue;

            if (maximumValue == 0)
            {
                return 30;
            }

            const double minimumWidth = 30;
            const double additionalWidth = 170;

            return minimumWidth +
                   (double)value / maximumValue *
                   additionalWidth;
        }

        private void UpdateDistribution()
        {
            OnPropertyChanged(nameof(Distribution1Width));
            OnPropertyChanged(nameof(Distribution2Width));
            OnPropertyChanged(nameof(Distribution3Width));
            OnPropertyChanged(nameof(Distribution4Width));
            OnPropertyChanged(nameof(Distribution5Width));
            OnPropertyChanged(nameof(Distribution6Width));
        }

        private void EvaluateGuess(string guess)
        {
            List<char> remainingLetters =
                targetWord.ToList();

            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == targetWord[i])
                {
                    remainingLetters.Remove(guess[i]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                char guessedLetter = guess[i];

                LetterCell cell =
                    Guesses[currentRow][i];

                Brush color;

                if (guessedLetter == targetWord[i])
                {
                    color = Brushes.Green;
                }
                else if (remainingLetters.Contains(guessedLetter))
                {
                    color = Brushes.Goldenrod;
                    remainingLetters.Remove(guessedLetter);
                }
                else
                {
                    color = Brushes.DarkGray;
                }

                cell.BackgroundColor = color;

                UpdateKeyboardColor(
                    guessedLetter.ToString(),
                    color);
            }
        }

        private void UpdateKeyboardColor(
            string letter,
            Brush color)
        {
            KeyModel? key =
                KeyboardKeys.FirstOrDefault(
                    keyboardKey =>
                        keyboardKey.Key == letter);

            if (key == null)
            {
                return;
            }

            if (color == Brushes.Green)
            {
                key.BackgroundColor =
                    Brushes.Green;
            }
            else if (color == Brushes.Goldenrod &&
                     key.BackgroundColor != Brushes.Green)
            {
                key.BackgroundColor =
                    Brushes.Goldenrod;
            }
            else if (color == Brushes.DarkGray &&
                     key.BackgroundColor != Brushes.Green &&
                     key.BackgroundColor != Brushes.Goldenrod)
            {
                key.BackgroundColor =
                    Brushes.DarkGray;
            }
        }
    }
}
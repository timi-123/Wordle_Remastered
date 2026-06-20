using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Proekt_VP.Models;

namespace Proekt_VP.ViewModels
{
    public class PlayerBattleState : ViewModelBase
    {
        public string Name { get; }

        public ObservableCollection<ObservableCollection<LetterCell>> Guesses { get; }
        public ObservableCollection<KeyModel> KeyboardKeys { get; }
        public ObservableCollection<ObservableCollection<KeyModel>> KeyboardRows { get; }

        public ICommand EnterLetterCommand { get; }

        public Func<bool>? CanInput;
        public Action<PlayerBattleState>? SubmitRequested;

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private string _timeRemainingText = "";
        public string TimeRemainingText
        {
            get => _timeRemainingText;
            set => SetProperty(ref _timeRemainingText, value);
        }

        private int _maxHP = 1000;
        public int MaxHP
        {
            get => _maxHP;
            set { if (SetProperty(ref _maxHP, value)) OnPropertyChanged(nameof(HpFraction)); }
        }

        private int _hp = 1000;
        public int HP
        {
            get => _hp;
            set
            {
                if (SetProperty(ref _hp, value))
                {
                    OnPropertyChanged(nameof(HpFraction));
                    IsHpLow = HpFraction <= 0.4;
                    IsHpCritical = HpFraction <= 0.2;
                }
            }
        }

        public double HpFraction => MaxHP > 0 ? (double)HP / MaxHP : 0;

        private bool _isHpLow;
        public bool IsHpLow
        {
            get => _isHpLow;
            set => SetProperty(ref _isHpLow, value);
        }

        private bool _isHpCritical;
        public bool IsHpCritical
        {
            get => _isHpCritical;
            set => SetProperty(ref _isHpCritical, value);
        }

        private int _xp;
        public int XP
        {
            get => _xp;
            set => SetProperty(ref _xp, value);
        }

        private bool _isMyTurn;
        public bool IsMyTurn
        {
            get => _isMyTurn;
            set => SetProperty(ref _isMyTurn, value);
        }

        private bool _isInputEnabled;
        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set => SetProperty(ref _isInputEnabled, value);
        }

        public IReadOnlyList<string> TargetWords { get; private set; } = new List<string>();
        public int CurrentWordIndex { get; private set; }
        public int CurrentRowIndex => _currentRowIndex;
        public string CurrentTargetWord => CurrentWordIndex < TargetWords.Count ? TargetWords[CurrentWordIndex] : "";
        public bool HasClearedAllWords => CurrentWordIndex >= TargetWords.Count;
        public bool IsBoardFull => _currentRowIndex >= 6;

        private int _currentRowIndex;
        private int _currentColIndex;

        public PlayerBattleState(string name)
        {
            Name = name;

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

            EnterLetterCommand = new RelayCommand(Input, _ => CanInput?.Invoke() ?? true);
        }

        public void SetTargetWords(IReadOnlyList<string> words)
        {
            TargetWords = words;
            CurrentWordIndex = 0;
            ResetBoard();
            OnPropertyChanged(nameof(CurrentTargetWord));
        }

        public void Input(object? parameter)
        {
            if (!(CanInput?.Invoke() ?? true)) return;

            ErrorMessage = "";
            if (parameter is string key)
            {
                if (key == "ENTER")
                {
                    SubmitRequested?.Invoke(this);
                    return;
                }

                if (key == "⌫" || key == "BACK")
                {
                    Backspace();
                    return;
                }

                if (_currentRowIndex < 6 && _currentColIndex < 5)
                {
                    Guesses[_currentRowIndex][_currentColIndex].Letter = key;
                    _currentColIndex++;
                }
            }
        }

        private void Backspace()
        {
            if (_currentColIndex > 0)
            {
                _currentColIndex--;
                Guesses[_currentRowIndex][_currentColIndex].Letter = "";
            }
        }

        public string GetCurrentGuess()
        {
            if (_currentRowIndex >= 6) return "";
            return string.Join("", Guesses[_currentRowIndex].Select(c => c.Letter));
        }

        public (int Greens, int Yellows) ApplyGuessColors(string guess)
        {
            string target = CurrentTargetWord;
            int greens = 0;
            int yellows = 0;

            for (int i = 0; i < 5; i++)
            {
                char guessedChar = guess[i];
                var cell = Guesses[_currentRowIndex][i];

                Brush cellColor;
                if (i < target.Length && guessedChar == target[i])
                {
                    cellColor = Brushes.Green;
                    greens++;
                }
                else if (target.Contains(guessedChar))
                {
                    cellColor = Brushes.Goldenrod;
                    yellows++;
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

            return (greens, yellows);
        }

        public void AdvanceRow()
        {
            _currentRowIndex++;
            _currentColIndex = 0;
        }

        public void ClearCurrentRow()
        {
            if (_currentRowIndex >= 6) return;
            foreach (var cell in Guesses[_currentRowIndex])
            {
                cell.Letter = "";
            }
            _currentColIndex = 0;
        }

        public void AdvanceToNextWord()
        {
            CurrentWordIndex++;
            ResetBoard();
            OnPropertyChanged(nameof(CurrentTargetWord));
        }

        public void ResetBoard()
        {
            foreach (var row in Guesses)
            {
                foreach (var cell in row)
                {
                    cell.Letter = "";
                    cell.BackgroundColor = Brushes.Transparent;
                }
            }

            foreach (var key in KeyboardKeys)
            {
                key.BackgroundColor = Brushes.LightGray;
            }

            _currentRowIndex = 0;
            _currentColIndex = 0;
            ErrorMessage = "";
        }
    }
}

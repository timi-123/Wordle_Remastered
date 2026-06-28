using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Proekt_VP.Services;

namespace Proekt_VP.ViewModels
{
    public class TwoPlayerViewModel : ViewModelBase
    {
        private const int StartHP = 1000;
        private const int GreenDamage = 25;
        private const int YellowDamage = 10;
        private const int SolveBurst = 150;
        private const int GreenHeal = 12;
        private const int YellowHeal = 6;
        private const int TimeoutPenalty = 100;
        private const int WordsPerPlayer = 5;

        public PlayerBattleState Player1 { get; }
        public PlayerBattleState Player2 { get; }

        public ObservableCollection<string> P1ChosenWords { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> P2ChosenWords { get; } = new ObservableCollection<string>();

        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _turnStopwatch = new Stopwatch();

        private bool _p1Turn = true;
        public PlayerBattleState ActivePlayer => _p1Turn ? Player1 : Player2;
        private PlayerBattleState OpponentOf(PlayerBattleState p) => p == Player1 ? Player2 : Player1;

        public int TimeCapSeconds { get; private set; } = 20;

        public event Action<string, int>? BattleEnded;

        private bool _isBattle;
        public bool IsBattle
        {
            get => _isBattle;
            private set => SetProperty(ref _isBattle, value);
        }

        private bool _isResolving;
        public bool IsResolving
        {
            get => _isResolving;
            private set { if (SetProperty(ref _isResolving, value)) UpdateInputGates(); }
        }

        private int _remainingSeconds;
        public int RemainingSeconds
        {
            get => _remainingSeconds;
            private set => SetProperty(ref _remainingSeconds, value);
        }

        private string _timerText = "";
        public string TimerText
        {
            get => _timerText;
            private set => SetProperty(ref _timerText, value);
        }

        private bool _isTimerLow;
        public bool IsTimerLow
        {
            get => _isTimerLow;
            private set => SetProperty(ref _isTimerLow, value);
        }

        private string _turnText = "";
        public string TurnText
        {
            get => _turnText;
            private set => SetProperty(ref _turnText, value);
        }

        private string _wordEntryError = "";
        public string WordEntryError
        {
            get => _wordEntryError;
            set => SetProperty(ref _wordEntryError, value);
        }

        public bool P1EntryComplete => P1ChosenWords.Count == WordsPerPlayer;
        public bool P2EntryComplete => P2ChosenWords.Count == WordsPerPlayer;
        public bool P1CanAddWord => P1ChosenWords.Count < WordsPerPlayer;
        public bool P2CanAddWord => P2ChosenWords.Count < WordsPerPlayer;

        public TwoPlayerViewModel()
        {
            Player1 = new PlayerBattleState("Player 1");
            Player2 = new PlayerBattleState("Player 2");

            Player1.CanInput = () => Player1.IsInputEnabled;
            Player2.CanInput = () => Player2.IsInputEnabled;
            Player1.SubmitRequested = OnSubmit;
            Player2.SubmitRequested = OnSubmit;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += Timer_Tick;
        }

        public void SetTimeCap(int seconds)
        {
            TimeCapSeconds = seconds;
        }

        public async Task<bool> TryAddWordAsync(int player, string? word)
        {
            WordEntryError = "";
            word = (word ?? "").Trim().ToUpper();

            if (word.Length != 5 || !word.All(char.IsLetter))
            {
                WordEntryError = "Enter a 5-letter word";
                return false;
            }

            var list = player == 1 ? P1ChosenWords : P2ChosenWords;

            if (list.Count >= WordsPerPlayer)
            {
                WordEntryError = "You already chose 5 words";
                return false;
            }

            if (list.Contains(word))
            {
                WordEntryError = "Word already added";
                return false;
            }

            bool valid = await WordValidator.IsValidWordAsync(word);
            if (!valid)
            {
                WordEntryError = $"'{word}' is not a valid word";
                return false;
            }

            list.Add(word);
            OnPropertyChanged(nameof(P1EntryComplete));
            OnPropertyChanged(nameof(P2EntryComplete));
            OnPropertyChanged(nameof(P1CanAddWord));
            OnPropertyChanged(nameof(P2CanAddWord));
            return true;
        }

        public void RemoveWord(int player, string word)
        {
            var list = player == 1 ? P1ChosenWords : P2ChosenWords;
            list.Remove(word);
            WordEntryError = "";
            OnPropertyChanged(nameof(P1EntryComplete));
            OnPropertyChanged(nameof(P2EntryComplete));
            OnPropertyChanged(nameof(P1CanAddWord));
            OnPropertyChanged(nameof(P2CanAddWord));
        }

        public void StartBattle()
        {
            IsBattle = true;
            IsResolving = false;
            _p1Turn = true;

            Player1.MaxHP = StartHP;
            Player2.MaxHP = StartHP;
            Player1.HP = StartHP;
            Player2.HP = StartHP;

            Player1.SetTargetWords(P2ChosenWords.ToList());
            Player2.SetTargetWords(P1ChosenWords.ToList());

            SetTurn(Player1);
            StartTurnTimer();
        }

        public void StopTimer()
        {
            _timer.Stop();
            IsBattle = false;
        }

        public void HandleKey(string key)
        {
            if (!IsBattle || IsResolving) return;
            ActivePlayer.Input(key);
        }

        private void OnSubmit(PlayerBattleState attacker)
        {
            _ = ResolveGuessAsync(attacker);
        }

        private async Task ResolveGuessAsync(PlayerBattleState attacker)
        {
            if (!IsBattle || IsResolving || attacker != ActivePlayer) return;

            string guess = attacker.GetCurrentGuess();
            if (guess.Length != 5) return;

            IsResolving = true;
            _timer.Stop();
            _turnStopwatch.Stop();
            double elapsed = _turnStopwatch.Elapsed.TotalSeconds;

            bool valid = await WordValidator.IsValidWordAsync(guess);
            if (!valid)
            {
                attacker.ErrorMessage = "Not in word list";
                IsResolving = false;
                _turnStopwatch.Start();
                _timer.Start();
                return;
            }

            var (greens, yellows) = attacker.ApplyGuessColors(guess);
            bool solved = guess == attacker.CurrentTargetWord;

            double f = TimeCapSeconds > 0 ? Math.Clamp((TimeCapSeconds - elapsed) / TimeCapSeconds, 0.0, 1.0) : 0.0;
            double timeMultiplier = 1.0 + f;

            int raw = greens * GreenDamage + yellows * YellowDamage + (solved ? SolveBurst : 0);
            int damage = (int)Math.Round(raw * timeMultiplier);

            var opponent = OpponentOf(attacker);
            opponent.HP -= damage;

            attacker.HP += (int)Math.Round((greens * GreenHeal + yellows * YellowHeal) * timeMultiplier);

            if (solved)
            {
                int guessNumber = attacker.CurrentRowIndex + 1;
                attacker.HP += 100 + (int)Math.Round(100 * f) + (7 - guessNumber) * 15;
            }

            bool opponentDefeated = opponent.HP <= 0;

            if (solved)
            {
                bool wasLastWord = attacker.CurrentWordIndex >= attacker.TargetWords.Count - 1;
                if (opponentDefeated || wasLastWord)
                {
                    EndGame(attacker);
                    return;
                }
                attacker.AdvanceToNextWord();
                attacker.ErrorMessage = "Solved! Next word";
            }
            else
            {
                if (opponentDefeated)
                {
                    EndGame(attacker);
                    return;
                }
                attacker.AdvanceRow();
                if (attacker.IsBoardFull)
                {
                    string missed = attacker.CurrentTargetWord;
                    attacker.AdvanceToNextWord();
                    attacker.ErrorMessage = $"Out of guesses, the word was {missed}";
                }
            }

            IsResolving = false;
            ContinueOrEnd(attacker);
        }

        private void ForfeitTurn()
        {
            _timer.Stop();
            var who = ActivePlayer;
            who.ClearCurrentRow();
            who.HP -= TimeoutPenalty;
            who.ErrorMessage = $"Time! -{TimeoutPenalty} HP";

            if (who.HP <= 0)
            {
                EndGame(OpponentOf(who));
                return;
            }

            ContinueOrEnd(who);
        }

        private void ContinueOrEnd(PlayerBattleState attacker)
        {
            var opponent = OpponentOf(attacker);
            bool attackerOut = attacker.HasClearedAllWords;
            bool opponentOut = opponent.HasClearedAllWords;

            if (attackerOut && opponentOut)
            {
                EndByHp();
                return;
            }

            PlayerBattleState next = !opponentOut ? opponent : attacker;
            SetTurn(next);
            StartTurnTimer();
        }

        private void EndByHp()
        {
            if (Player1.HP == Player2.HP)
            {
                _timer.Stop();
                _turnStopwatch.Stop();
                IsBattle = false;
                IsResolving = false;
                BattleEnded?.Invoke("It's a draw!", Player1.HP);
                return;
            }

            EndGame(Player1.HP > Player2.HP ? Player1 : Player2);
        }

        private void EndGame(PlayerBattleState winner)
        {
            _timer.Stop();
            _turnStopwatch.Stop();
            IsBattle = false;
            IsResolving = false;
            BattleEnded?.Invoke($"{winner.Name} Wins!", winner.HP);
        }

        private void SetTurn(PlayerBattleState player)
        {
            _p1Turn = player == Player1;
            Player1.IsMyTurn = _p1Turn;
            Player2.IsMyTurn = !_p1Turn;
            TurnText = $"{player.Name}'s turn";
            UpdateInputGates();
        }

        private void StartTurnTimer()
        {
            _timer.Stop();
            _turnStopwatch.Restart();
            RemainingSeconds = TimeCapSeconds;
            RefreshTimerUi();
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (IsResolving || !IsBattle) return;

            int elapsed = (int)_turnStopwatch.Elapsed.TotalSeconds;
            RemainingSeconds = Math.Max(0, TimeCapSeconds - elapsed);
            RefreshTimerUi();

            if (RemainingSeconds <= 0)
            {
                ForfeitTurn();
            }
        }

        private void RefreshTimerUi()
        {
            TimerText = RemainingSeconds.ToString();
            IsTimerLow = RemainingSeconds <= 3;

            if (IsBattle)
            {
                ActivePlayer.TimeRemainingText = $"{RemainingSeconds}s";
                OpponentOf(ActivePlayer).TimeRemainingText = "";
            }
        }

        private void UpdateInputGates()
        {
            Player1.IsInputEnabled = IsBattle && _p1Turn && !IsResolving;
            Player2.IsInputEnabled = IsBattle && !_p1Turn && !IsResolving;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Proekt_VP.ViewModels;

namespace Proekt_VP
{
    public partial class TwoPlayerView : UserControl
    {
        private TwoPlayerViewModel _vm = null!;

        public TwoPlayerView()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            _vm = new TwoPlayerViewModel();
            _vm.BattleEnded += OnBattleEnded;
            DataContext = _vm;
            ShowPanel(TimeCapPanel);
        }

        private void ShowPanel(UIElement panel)
        {
            TimeCapPanel.Visibility = Visibility.Collapsed;
            P1EntryPanel.Visibility = Visibility.Collapsed;
            HandoffPanel.Visibility = Visibility.Collapsed;
            P2EntryPanel.Visibility = Visibility.Collapsed;
            BattlePanel.Visibility = Visibility.Collapsed;
            panel.Visibility = Visibility.Visible;
        }

        private void TimeCap10Click(object sender, RoutedEventArgs e) => StartWordEntry(10);
        private void TimeCap20Click(object sender, RoutedEventArgs e) => StartWordEntry(20);
        private void TimeCap30Click(object sender, RoutedEventArgs e) => StartWordEntry(30);

        private void StartWordEntry(int seconds)
        {
            _vm.SetTimeCap(seconds);
            ShowPanel(P1EntryPanel);
            P1WordInput.Focus();
        }

        private async void P1AddWordClick(object sender, RoutedEventArgs e)
        {
            if (await _vm.TryAddWordAsync(1, P1WordInput.Text))
            {
                P1WordInput.Clear();
            }
            P1WordInput.Focus();
        }

        private void P1WordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                P1AddWordClick(sender, e);
                e.Handled = true;
            }
        }

        private void P1ConfirmClick(object sender, RoutedEventArgs e)
        {
            _vm.WordEntryError = "";
            ShowPanel(HandoffPanel);
        }

        private void HandoffContinueClick(object sender, RoutedEventArgs e)
        {
            ShowPanel(P2EntryPanel);
            P2WordInput.Focus();
        }

        private async void P2AddWordClick(object sender, RoutedEventArgs e)
        {
            if (await _vm.TryAddWordAsync(2, P2WordInput.Text))
            {
                P2WordInput.Clear();
            }
            P2WordInput.Focus();
        }

        private void P2WordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                P2AddWordClick(sender, e);
                e.Handled = true;
            }
        }

        private void P2ConfirmClick(object sender, RoutedEventArgs e)
        {
            _vm.WordEntryError = "";
            ShowPanel(BattlePanel);
            _vm.StartBattle();
            Focus();
        }

        private void OnBattleEnded(string message, int score)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GameOverWindow popup = new GameOverWindow(message, score)
                {
                    Owner = Window.GetWindow(this)
                };
                popup.ShowDialog();

                if (popup.PlayAgain)
                {
                    _vm.BattleEnded -= OnBattleEnded;
                    InitGame();
                }
                else
                {
                    ReturnToMenu();
                }
            }));
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (BattlePanel.Visibility != Visibility.Visible) return;

            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                _vm.HandleKey(e.Key.ToString());
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                _vm.HandleKey("ENTER");
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                _vm.HandleKey("⌫");
                e.Handled = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToMenu();
        }

        private void ReturnToMenu()
        {
            _vm.StopTimer();
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.ChangeView(new MainMenuView());
        }
    }
}

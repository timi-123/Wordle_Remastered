using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Proekt_VP.Models;

namespace Proekt_VP.Services
{
    public enum LetterStatus
    {
        Correct,
        Present,
        Absent
    }

    public static class GuessEvaluator
    {
        public static LetterStatus[] Evaluate(string guess, string target)
        {
            var statuses = new LetterStatus[guess.Length];
            var remaining = new List<char>(target);

            for (int i = 0; i < guess.Length; i++)
            {
                statuses[i] = LetterStatus.Absent;
            }

            for (int i = 0; i < guess.Length; i++)
            {
                if (i < target.Length && guess[i] == target[i])
                {
                    statuses[i] = LetterStatus.Correct;
                    remaining.Remove(guess[i]);
                }
            }

            for (int i = 0; i < guess.Length; i++)
            {
                if (statuses[i] == LetterStatus.Correct) continue;

                if (remaining.Remove(guess[i]))
                {
                    statuses[i] = LetterStatus.Present;
                }
                else
                {
                    statuses[i] = LetterStatus.Absent;
                }
            }

            return statuses;
        }

        public static Brush ColorFor(LetterStatus status)
        {
            if (status == LetterStatus.Correct)
            {
                return Brushes.Green;
            }
            if (status == LetterStatus.Present)
            {
                return Brushes.Goldenrod;
            }
            return Brushes.DarkGray;
        }

        public static LetterStatus[] ApplyToRow(
            ObservableCollection<LetterCell> row,
            ObservableCollection<KeyModel> keyboardKeys,
            string guess,
            string target)
        {
            var statuses = Evaluate(guess, target);

            for (int i = 0; i < statuses.Length; i++)
            {
                Brush color = ColorFor(statuses[i]);
                row[i].BackgroundColor = color;

                var key = keyboardKeys.FirstOrDefault(k => k.Key == guess[i].ToString());
                if (key == null) continue;

                if (color == Brushes.Green)
                {
                    key.BackgroundColor = Brushes.Green;
                }
                else if (color == Brushes.Goldenrod && key.BackgroundColor != Brushes.Green)
                {
                    key.BackgroundColor = Brushes.Goldenrod;
                }
                else if (color == Brushes.DarkGray && key.BackgroundColor != Brushes.Green && key.BackgroundColor != Brushes.Goldenrod)
                {
                    key.BackgroundColor = Brushes.DarkGray;
                }
            }

            return statuses;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConsoleProgress
{
    internal class CursorPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }
    }

    public class ProgressBarOptions
    {
        public int NumberOfSquares { get; set; } = 10;
        //public bool FillRestOfLine { get; set; }
        public int? MaximumValue { get; set; }    // TODO: GENERIC?
        public bool ShowPercentage { get; set; } = true;
        public char FilledChar { get; set; } = CharConstants.Pipe;
        public char UnfilledChar { get; set; } = CharConstants.Dash;
        public ConsoleColor? FilledCharColour { get; set; }
        public ConsoleColor? UnfilledCharColour { get; set; }
        public bool ShowBorders { get; set; } = false;
    }

    public static class CharConstants
    {
        public const char Block = '\u2588';
        public const char Pipe = '|';
        public const char Dash = '-';
    }

    public class ProgressBar
    {
        private CursorPosition _cursorPosition;
        private readonly ProgressBarOptions _options;


        public ProgressBar(ProgressBarOptions options = null)
        {
            _options = options ?? new ProgressBarOptions();
        }

        public void Report(int current) // TODO: GENERIC
        {
            if (!_options.MaximumValue.HasValue)
            {
                throw new InvalidOperationException("A maximum value has not been set, use the Report(double) method instead");
            }

            double percentage = (double)current / _options.MaximumValue.Value;
            Report(percentage);
        }

        public void Report(double percentage)
        {
            SetCursorPosition();

            percentage = Math.Max(0, Math.Min(1, percentage));
            var filledProgressBarContent = GetFilledProgressBarContent(percentage);
            var unfilledProgressBarContent = GetUnfilledProgressBarContent(percentage);

            if (_options.ShowBorders)
            {
                Console.Write("\u250C");
                for (int i = 0; i < _options.NumberOfSquares; i++)
                {
                    Console.Write("\u2500");
                }
                Console.Write("\u2510");

                Console.CursorTop = _cursorPosition.Top + 1;
                Console.CursorLeft = _cursorPosition.Left;
            }
            

            var currentColour = Console.ForegroundColor;

            Console.Write(_options.ShowBorders ? "\u2502" : "[");

            Console.ForegroundColor = _options.FilledCharColour ?? currentColour;

            Console.Write(filledProgressBarContent);

            Console.ForegroundColor = _options.UnfilledCharColour ?? currentColour;

            Console.Write(unfilledProgressBarContent);


            Console.ForegroundColor = currentColour;

            Console.Write(_options.ShowBorders ? "\u2502" : "]");

            if (_options.ShowPercentage)
            {
                var percentageText = GetPercentageText(percentage);
                Console.Write(" " + percentageText);
            }

            if (_options.ShowBorders)
            {
                Console.CursorTop = _cursorPosition.Top + 2;
                Console.CursorLeft = _cursorPosition.Left;

                Console.Write("\u2514");
                for (int i = 0; i < _options.NumberOfSquares; i++)
                {
                    Console.Write("\u2500");
                }
                Console.Write("\u2518");
            }
        }

        private string GetPercentageText(double percentage)
        {
            return string.Format("{0,4:##0%}", percentage);
        }

        private string GetFilledProgressBarContent(double percentage)
        {
            var numBlocksFilled = (int)Math.Floor(_options.NumberOfSquares * percentage);

            return GetRepeatedCharString(numBlocksFilled, _options.FilledChar);
        }

        private string GetUnfilledProgressBarContent(double percentage)
        {
            var numBlocksFilled = (int)Math.Floor(_options.NumberOfSquares * percentage);
            var numBlocksUnfilled = _options.NumberOfSquares - numBlocksFilled;

            return GetRepeatedCharString(numBlocksUnfilled, _options.UnfilledChar);
        }

        private string GetRepeatedCharString(int numCharacters, char character)
        {
            StringBuilder s = new StringBuilder(numCharacters);
            for (int i = 0; i < numCharacters; i++)
            {
                s.Append(character);
            }
            return s.ToString();
        }

        private void SetCursorPosition()
        {
            if (_cursorPosition == null)
            {
                _cursorPosition = new CursorPosition
                {
                    Left = Console.CursorLeft,
                    Top = Console.CursorTop
                };
            }

            Console.CursorLeft = _cursorPosition.Left;
            Console.CursorTop = _cursorPosition.Top;
        }
    }
}

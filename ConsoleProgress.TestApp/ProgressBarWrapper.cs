using System;

namespace ConsoleProgress.TestApp
{
    internal abstract class ProgressBarWrapper
    {
        public ProgressBar ProgressBar { get; set; }
        public string TextBefore { get; set; }
        public string TextAfter { get; set; }

        public ProgressBarWrapper(ProgressBarOptions options)
        {
            ProgressBar = new ProgressBar(options);
        }

        public void Report(int iteration)
        {
            var top = Console.CursorTop;
            if (iteration == 0 && !string.IsNullOrEmpty(TextBefore))
            {
                Console.Write(TextBefore);
            }

            var result = GetReportedProgress(iteration);
            ProgressBar.Report(result);

            if (iteration == 0 && !string.IsNullOrEmpty(TextAfter))
            {
                Console.Write(TextAfter);
            }
            if (iteration == 0)
            {
                Console.SetCursorPosition(0, top + 1);
            }
        }

        protected abstract int GetReportedProgress(int iteration);
    }

    class SimpleProgressBarWrapper : ProgressBarWrapper
    {
        public SimpleProgressBarWrapper(ProgressBarOptions options) : base(options)
        {
        }

        protected override int GetReportedProgress(int iteration)
        {
            return iteration;
        }
    }

    class MultipliedProgressBarWrapper : ProgressBarWrapper
    {
        private readonly int _multiplier;


        public MultipliedProgressBarWrapper(int multiplier, ProgressBarOptions options) : base(options)
        {
            _multiplier = multiplier;
        }

        protected override int GetReportedProgress(int iteration)
        {
            return iteration * _multiplier;
        }
    }

    class RandomProgressBarWrapper : ProgressBarWrapper
    {
        private readonly Random Random = new Random();
        private int _current;
        public RandomProgressBarWrapper(ProgressBarOptions options) : base(options)
        {
            
        }

        protected override int GetReportedProgress(int iteration)
        {
            var random = Random.Next(1, 6);
            _current += random;
            return _current;
        }
    }
}
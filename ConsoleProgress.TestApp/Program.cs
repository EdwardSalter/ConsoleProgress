using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleProgress.TestApp
{
    class Program
    {
        private static readonly List<ProgressBarWrapper> ProgressBars = new List<ProgressBarWrapper>();

        static void Main()
        {
            SetupProgressBars();

            Report(0);

            Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    Thread.Sleep(1000);
                    Report(i);
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("All Done");
            });

            
            Console.ReadKey();
        }

        private static void Report(int iteration)
        {
            ProgressBars.ForEach(p => p.Report(iteration));
        }

        private static void SetupProgressBars()
        {
            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10
            }));

            ProgressBars.Add(new MultipliedProgressBarWrapper(10, new ProgressBarOptions
            {
                MaximumValue = 100,
                NumberOfSquares = 30,
                ShowPercentage = false
            }));

            ProgressBars.Add(new RandomProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 30,
                NumberOfSquares = 50
            }));

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 38
            }) {TextBefore = "Text Before "});

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 38
            })
            { TextAfter = " Text After" });

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 36,
                ShowPercentage = false
            })
            { TextBefore = "Before ", TextAfter = " After"});

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 10,
                FilledChar = CharConstants.Block,
                UnfilledChar = CharConstants.Dash
            }));

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 10,
                FilledChar = CharConstants.Block,
                UnfilledChar = CharConstants.Block,
                FilledCharColour = ConsoleColor.Blue,
                UnfilledCharColour = ConsoleColor.White
            }));

            ProgressBars.Add(new SimpleProgressBarWrapper(new ProgressBarOptions
            {
                MaximumValue = 10,
                NumberOfSquares = 10,
                ShowBorders = true,
                FilledChar = CharConstants.Block,
                UnfilledChar = ' '
            }));
        }
    }
}

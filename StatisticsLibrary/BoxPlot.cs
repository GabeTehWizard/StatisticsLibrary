using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StatisticsLibrary.CentralMeasurements;
using static StatisticsLibrary.MathUtilities;

namespace StatisticsLibrary
{
    public static class BoxPlot
    {
        public static int boxWidth = 143;
        public static void PrintRangeDiscardOutliers(double[] data, string title)
        {
            double Q1 = FindFirstQuartile(data);
            double Q2 = FindSecondQuartile(data);
            double Q3 = FindThirdQuartile(data);
            double IQR = CalculateInterQuartileRange(data);
            double[] acceptableLimits = new double[] { Q1 - 1.5 * IQR, Q3 + 1.5 * IQR };
            double[] arr = data.Where(d => d >= acceptableLimits[0] && d <= acceptableLimits[1]).OrderBy(x => x).ToArray();
            double min = arr.Min();
            double max = arr.Max();
            double range = acceptableLimits[1] - acceptableLimits[0];
            double startLocation = acceptableLimits[0];
            double endLocation = acceptableLimits[1];


            string outputString = $"Box Plot for {title}";
            Console.WriteLine($"\n\n{WhiteSpaceLeft(outputString, boxWidth)}{outputString}{WhiteSpaceRight(outputString, boxWidth)}\n\n");

            PrintBoxPlot(acceptableLimits[0], acceptableLimits[1], arr[0], arr[arr.Length - 1], Q1, Q2, Q3, out int outputLength);
            PrintNumbers(acceptableLimits[0], acceptableLimits[1], outputLength);

            Console.WriteLine("\nQuartile/Percentile Measurements\n");

            Console.WriteLine("Q1 " + Q1);
            Console.WriteLine("Q2 " + Q2);
            Console.WriteLine("Q3 " + Q3);
            Console.WriteLine("IQR " + IQR);
            Console.WriteLine("Upper Limit " + acceptableLimits[1]);
            Console.WriteLine("Lower Limit " + acceptableLimits[0]);
            Console.WriteLine("Upper Limit " + acceptableLimits[1]);
            Console.WriteLine("Min " + arr[0]);
            Console.WriteLine("Max " + arr[arr.Length - 1]);
            Console.WriteLine("Range " + range);

        }

        public static void PrintBoxPlot(double lowerLimit, double upperLimit, double min, double max, double Q1, double Q2, double Q3, out int outputLength)
        {
            double outerRange = upperLimit - lowerLimit;
            int startLength = (int)Math.Round((min - lowerLimit) / outerRange * boxWidth, 0);
            double tempInnerStartRange = (int)Math.Round((Q1 - min) / outerRange * boxWidth, 0);
            int innerStartRange = (int)Math.Round(tempInnerStartRange);
            double tempEndStartRange = (max - Q3) / outerRange * boxWidth;
            int innerEndtRange = (int)Math.Round(tempEndStartRange);
            int Q1Length = (int)Math.Round((Q2 - Q1) / outerRange * boxWidth, 0);
            int Q2Length = (int)Math.Round((Q3 - Q2) / outerRange * boxWidth, 0);
            int endLength = (int)Math.Round((upperLimit - max) / outerRange * boxWidth, 0);

            string output = $"{new string(' ', 5)}|{new string(' ', DefaultSubtraction(startLength,2))}|{new string('-', DefaultSubtraction(innerStartRange, 1))}{new string('\u2588', DefaultSubtraction(Q1Length,1))}|" +
                $"{new string('\u2588', Q2Length)}{new string('-', DefaultSubtraction(innerEndtRange, 1))}|{new string(' ', DefaultSubtraction(endLength, 1))}|\n";
            outputLength = output.Length;
            Console.WriteLine(output);
        }

        public static int DefaultSubtraction(double num, double subtrahend)
        {
            return (int)(num - subtrahend < 0 ? 1 : num - subtrahend);    
        }

        public static void PrintNumbers(double lowerLimit, double upperLimit, int outputLength)
        {
            int intervalCount = boxWidth / 11;
            int startCount = lowerLimit.ToString().Length;
            int endCount = upperLimit.ToString().Length;
            double increment = RoundUp((upperLimit - lowerLimit) / intervalCount, 2);
            int startIntervalCount = (lowerLimit + increment).ToString().Length;
            double currentInterval = Math.Round(lowerLimit,2);
            increment = Math.Round(increment,2);
            if (endCount <= 11 && startCount <= 11 && startIntervalCount <= 11)
            {
                while (currentInterval < upperLimit)
                {
                    Console.Write($"{WhiteSpaceLeft(Math.Round(currentInterval, 2).ToString(), 11)}{Math.Round(currentInterval, 2)}{WhiteSpaceRight(Math.Round(currentInterval, 2).ToString(), 11)}");
                    currentInterval += increment;
                }
                Console.WriteLine($"{WhiteSpaceLeft(Math.Round(currentInterval, 2).ToString(), 11)}{upperLimit}{WhiteSpaceRight(Math.Round(currentInterval, 2).ToString(), 11)}");
            }
        }

        public static string WhiteSpaceLeft(string str, int totalSpace)
        {
            return new string(' ', CalculateSpaceLeft(str, totalSpace));
        }
        public static string WhiteSpaceRight(string str, int totalSpace)
        {
            return new string(' ', CalculateSpaceRight(str, totalSpace));
        }

        public static int CalculateSpaceLeft(string str, int totalSpace)
        {
            return str.Length % 2 == 0 ? (int)RoundDown(((double)totalSpace - str.Length) / 2, 0) : (totalSpace - str.Length) / 2;
        }
        public static int CalculateSpaceRight(string str, int totalSpace)
        {
            return str.Length % 2 == 0 ? (int)RoundUp(((double)totalSpace - str.Length) / 2, 0) : (totalSpace - str.Length) / 2;
        }
    }
}

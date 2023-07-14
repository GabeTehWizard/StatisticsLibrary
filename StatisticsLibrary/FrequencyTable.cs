using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsLibrary
{
    public static class FrequencyTable
    {
        private static int tableWidth = 144;
        private static int headerWidth = (int)Math.Ceiling((double)(tableWidth - 7) / 6);

        public static void PrintBorder() => Console.WriteLine(new String('-', tableWidth));

        public static void PrintHeader(string groupHeader)
        {
            PrintDataRow(new List<string> { groupHeader, "Frequency", "Cumulative Frequency", "Middle Point (x)", "f(x)", "f(x)\u00B2" });
        }

        public static void PrintDataRow(List<string> data)
        {
            string output = "";
            foreach(string str in data)
            {
                output += $"|{new String(' ', (headerWidth - str.Length) / 2)}{str}" +
                    $"{new String(' ', (headerWidth - str.Length) / 2 + (str.Length % 2 == 0 ? 1:0))}";
            }
            Console.WriteLine($"{output.Substring(0, output.Length - 1)}|");
        }

        //Method to calculate K or the number of groups.
        public static int CalculateK(int dataLength) => (int)Math.Round(Math.Sqrt(dataLength));

        public static int CalculateK(double[] data, double startingPoint, double width)
        {
            return (int)Math.Round((data.OrderBy(x => x).Last() - startingPoint) / width);
        }

        //Method to calculate the Width, or range value for the grouping
        public static double CalculateWidth(double min, double max, int k, int decimalPlaces) => MathUtilities.RoundUp((double)(max-min) / k, decimalPlaces);

        //Method to create the groups
        public static List<double> CreateGroups(double[] data, int k, double width)
        {
            double floor = data.OrderBy(x => x).FirstOrDefault();
            List<double> groups = new List<double>();
            for(int i = 0; i <= k; i++)
            {
                groups.Add(floor);
                floor += width;
            }
            return groups;
        }

        public static List<double> CreateGroups(int k, double width, double startingPoint)
        {
            double floor = startingPoint;
            List<double> groups = new List<double>();
            for (int i = 0; i <= k; i++)
            {
                groups.Add(floor);
                floor += width;
            }
            return groups;
        }

        public static Dictionary<string, List<double>> CreateTableData(double[] data, List<double> groupIntervals, int decimalPlaces)
        {
            Dictionary<string, List<double>> tableData = new Dictionary<string, List<double>>();
            double cumulativeFrequency = 0;
            for(int i = 0; i < groupIntervals.Count - 1; i++)
            {
                int frequency = data.Where(d => d >= groupIntervals[i] && d < groupIntervals[i + 1]).Count();
                double middlePoint = (groupIntervals[i] + groupIntervals[i + 1] - MathUtilities.ShiftDecimalPlaces(1, decimalPlaces)) / 2;
                cumulativeFrequency += frequency;
                double fx = frequency * middlePoint;
                double fxsqrd = frequency * middlePoint * middlePoint;
                tableData.Add($"{groupIntervals[i]} - {groupIntervals[i + 1] - MathUtilities.ShiftDecimalPlaces(1, decimalPlaces)}", new List<double>() { frequency, cumulativeFrequency, middlePoint, fx, fxsqrd });
            }
            return tableData;
        }

        public static double GroupMean(Dictionary<string, List<double>> tableData) => tableData.Sum(entry => entry.Value[3]) / tableData.Last().Value[1];

        public static string GroupMode(Dictionary<string, List<double>> tableData)
        {
            var values = tableData.OrderByDescending(grp => grp.Value[0]).ToList();
            return values[0].Value[0] == values[1].Value[0] ? throw new Exception("no mode") : values[0].Value[2].ToString();
        }

        public static double GroupMedian(Dictionary<string, List<double>> tableData, int decimalPlaces)
        {
            double n = (int)tableData.Last().Value[1];
            double median = n / 2;
            KeyValuePair<string, List<double>> medianGroup = tableData.First(entry => entry.Value[1] >= median);
            double lm = double.Parse(medianGroup.Key.Split()[0]);
            double um = double.Parse(medianGroup.Key.Split()[2]);
            double width = um - lm + MathUtilities.ShiftDecimalPlaces(1,decimalPlaces);
            double fm = medianGroup.Value[0];
            var cfpEntry = tableData.Where(entry => entry.Value[1] < median);
            int cfp = (int)tableData.First().Value[1];
            if (cfpEntry.Count() > 1)
                cfp = (int)tableData.LastOrDefault(entry => entry.Value[1] < median).Value[1];

            return lm + ((n + 1) / 2 - cfp) / fm * width;
        }

        public static double GroupStandardDeviation(Dictionary<string, List<double>> tableData)
        {
            double sumfxSqrd = tableData.Sum(entry => entry.Value[0] * Math.Pow(entry.Value[2],2));
            double sumfxThenSqrd = Math.Pow(tableData.Sum(entry => entry.Value[3]),2);
            return Math.Sqrt((sumfxSqrd - sumfxThenSqrd / tableData.Last().Value[1]) / (tableData.Last().Value[1] - 1));
        }

        public static void CreateTable(string groupHeader, double[] data, int groupDecimalPlaces)
        {
            int k = CalculateK(data.Length);
            double width = CalculateWidth((int)data.Min(), (int)data.Max(), k, groupDecimalPlaces);
            List<double> groupIntervals = CreateGroups(data, k, width);
            Dictionary<string, List<double>> tableData = CreateTableData(data, groupIntervals, groupDecimalPlaces);

            PrintBorder();
            PrintHeader(groupHeader);
            PrintBorder();
            foreach(KeyValuePair<string, List<double>> entry in tableData)
            {
                PrintDataRow(new List<string>() { entry.Key.Length % 2 == 0? entry.Key + "" : entry.Key + " ", entry.Value[0].ToString(), entry.Value[1].ToString(), entry.Value[2].ToString(), entry.Value[3].ToString(), entry.Value[4].ToString() });
            }
            PrintBorder();

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("\nGrouped Data Central Measurements\n");
            Console.WriteLine($"\u03A3f = {data.Length}");
            Console.WriteLine($"\u03A3fx = {tableData.Select(entry => entry.Value[3]).Sum()}");
            Console.WriteLine($"\u03A3fx\u00B2 = {tableData.Select(entry => entry.Value[4]).Sum()}");
            Console.WriteLine($"The group min value is {tableData.First().Key.Split()[0]}");
            Console.WriteLine($"The group max value is {tableData.Last().Key.Split()[2]}");
            Console.WriteLine($"The group range is {double.Parse(tableData.Last().Key.Split()[2]) - double.Parse(tableData.First().Key.Split()[0])}.");
            Console.WriteLine($"The group mean is {GroupMean(tableData)}.");
            try
            {
                Console.WriteLine($"The group mode is {GroupMode(tableData)}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The group mode is {ex.Message}.");
            }
            Console.WriteLine($"The group median is {GroupMedian(tableData, groupDecimalPlaces)}.");
            Console.WriteLine($"The group standard deviation is {GroupStandardDeviation(tableData)}");

            Console.WriteLine("\nCentral Measurements\n");
            Console.WriteLine($"The min value is {data.Min()}");
            Console.WriteLine($"The max value is {data.Max()}");
            Console.WriteLine($"The range is {CentralMeasurements.CalculateRange(data)}.");
            Console.WriteLine($"The mean is {CentralMeasurements.CalculateMean(data)}.");
            Console.WriteLine($"The median is {CentralMeasurements.CalculateMedian(data)}.");
            try
            {
                Console.WriteLine($"The mode is {CentralMeasurements.CalculateMode(data)}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The mode is {ex.Message}.");
            }
            Console.WriteLine($"The sample standard deviation is {CentralMeasurements.CalculateSampleStandardDeviation(data)}.");
            Console.WriteLine($"The population standard deviation is {CentralMeasurements.CalculatePopulationStandardDeviation(data)}.");
            Console.WriteLine($"The sample variance is {CentralMeasurements.CalculateSampleVariance(data)}.");
            Console.WriteLine($"The population variance is {CentralMeasurements.CalculatePopulationVariance(data)}.");

            Console.WriteLine();
        }

        public static void CreateTable(string groupHeader, double[] data, int groupDecimalPlaces, double width, double startingPoint)
        {
            int k = CalculateK(data, startingPoint, width);
            List<double> groupIntervals = CreateGroups(k, width, startingPoint);
            Dictionary<string, List<double>> tableData = CreateTableData(data, groupIntervals, groupDecimalPlaces);
            PrintBorder();
            PrintHeader(groupHeader);
            PrintBorder();
            foreach (KeyValuePair<string, List<double>> entry in tableData)
            {
                PrintDataRow(new List<string>() { entry.Key.Length % 2 == 0 ? entry.Key + "" : entry.Key + " ", entry.Value[0].ToString(), entry.Value[1].ToString(), entry.Value[2].ToString(), entry.Value[3].ToString(), entry.Value[4].ToString() });
            }
            PrintBorder();
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("\nGrouped Data Central Measurements\n");
            Console.WriteLine($"\u03A3f = {data.Length}");
            Console.WriteLine($"\u03A3fx = {tableData.Select(entry => entry.Value[3]).Sum()}");
            Console.WriteLine($"\u03A3fx\u00B2 = {tableData.Select(entry => entry.Value[4]).Sum()}");
            Console.WriteLine($"The group min value is {tableData.First().Key.Split()[0]}");
            Console.WriteLine($"The group max value is {tableData.Last().Key.Split()[2]}");
            Console.WriteLine($"The group range is {double.Parse(tableData.Last().Key.Split()[2]) - double.Parse(tableData.First().Key.Split()[0])}.");
            Console.WriteLine($"The group mean is {GroupMean(tableData)}.");
            try
            {
                Console.WriteLine(GroupMode(tableData));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The group mode is {ex.Message}.");
            }
            Console.WriteLine($"The group median is {GroupMedian(tableData, groupDecimalPlaces)}.");
            Console.WriteLine($"The group standard deviation is {GroupStandardDeviation(tableData)}");
            Console.WriteLine("\nCentral Measurements\n");
            Console.WriteLine($"The min value is {data.Min()}");
            Console.WriteLine($"The max value is {data.Max()}");
            Console.WriteLine($"The range is {CentralMeasurements.CalculateRange(data)}.");
            Console.WriteLine($"The mean is {CentralMeasurements.CalculateMean(data)}.");
            Console.WriteLine($"The median is {CentralMeasurements.CalculateMedian(data)}.");
            try
            {
                Console.WriteLine($"The mode is {CentralMeasurements.CalculateMode(data)}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The mode is {ex.Message}.");
            }
            Console.WriteLine($"The sample standard deviation is {CentralMeasurements.CalculateSampleStandardDeviation(data)}.");
            Console.WriteLine($"The population standard deviation is {CentralMeasurements.CalculatePopulationStandardDeviation(data)}.");
            Console.WriteLine($"The sample variance is {CentralMeasurements.CalculateSampleVariance(data)}.");
            Console.WriteLine($"The population variance is {CentralMeasurements.CalculatePopulationVariance(data)}.");
            Console.WriteLine();
        }
    }
}

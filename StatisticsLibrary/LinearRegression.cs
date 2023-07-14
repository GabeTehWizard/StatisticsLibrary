using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsLibrary
{
    public static class LinearRegression
    {
        public static Dictionary<string,List<double>> CreateRegressionDataSet(List<double> xVariables, List<double> yVariables)
        {
            List<double> xx = xVariables.Select(x => x * x).ToList();
            List<double> yy = yVariables.Select(y => y * y).ToList();
            List<double> xy = new List<double>();
            
            for(int i = 0; i < xVariables.Count; i++)
                xy.Add(xVariables[i] * yVariables[i]);

            return new Dictionary<string, List<double>>()
            {
                { "x", xVariables },
                { "y", yVariables },
                { "xx", xx },
                { "yy", yy },
                { "xy", xy }
            };
        }

        public static double CalculateB(double n, double sumX, double sumY, double sumXX, double sumXY)
        {
            return (n * sumXY - sumX * sumY) / ( n * sumXX - Math.Pow(sumX, 2) );
        }

        public static double CalculateA(double b, double n, double sumX, double sumY)
        {
            return (sumY / n) - b * (sumX / n);
        }
        //Intended for use in a Console Application
        public static void DisplayRegressionStatistics(List<double> xList, List<double> yList)
        {
            Dictionary<string, List<double>> dataset = LinearRegression.CreateRegressionDataSet(xList, yList);
            double n = dataset["x"].Count;
            double sumX = dataset["x"].Sum();
            double sumY = dataset["y"].Sum();
            double sumXX = dataset["xx"].Sum();
            double sumYY = dataset["yy"].Sum();
            double sumXY = dataset["xy"].Sum();
            double b = CalculateB(dataset["x"].Count, sumX, sumY, sumXX, sumXY);
            double a = CalculateA(b, n, sumX, sumY);
            Console.WriteLine($"The calculation for b: {b}");
            Console.WriteLine($"The calculation for a: {a}");
            Console.WriteLine();
        }
    }
}

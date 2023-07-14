using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StatisticsLibrary.MathUtilities;

namespace StatisticsLibrary
{
    public static class CentralMeasurements
    {
        public static double CalculateMean(double[] data) => data.Sum() / data.Length;
        public static double CalculateWeightedMean(double[] xData, double[] wData)
        {
            double numerator = 0;
            for(int i = 0; i < xData.Length; i++)
                numerator += xData[i] * wData[i];
            return numerator / wData.Sum();
        }

        public static double CalculateMedian(double[] data)
        {
            List<double> values = data.OrderBy(numbers => numbers).ToList();
            int centerPoint = (int)Math.Floor((double)(data.Length / 2));
            return data.Length % 2 == 0 ? (values[centerPoint] + values[centerPoint - 1]) / 2 : values[centerPoint];
        }

        public static double CalculateMode(double[] data)
        {
            var values = data.GroupBy(x => x).OrderByDescending(grp => grp.Count()).ToList();
            return values[0].Count() == values[1].Count() ? throw new Exception("no mode") : values[0].Key;
        }

        public static double CalculateRange(double[] data) => data.Max() - data.Min();

        public static double CalculateSampleStandardDeviation(double[] data)
        {
            double mean = CalculateMean(data);
            double numerator = data.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(numerator / (data.Length - 1));
        }

        public static double CalculatePopulationStandardDeviation(double[] data)
        {
            double mean = CalculateMean(data);
            double numerator = data.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(numerator / data.Length);
        }

        public static double CalculateVariance(double standardDev)
        {
            return Math.Pow(standardDev, 2);
        }

        public static double CalculateSampleVariance(double[] data)
        {
            double mean = CalculateMean(data);
            double numerator = data.Sum(x => Math.Pow(x - mean,2));
            return numerator / (data.Length - 1);
        }

        public static double CalculatePopulationVariance(double[] data)
        {
            double mean = CalculateMean(data);
            double numerator = data.Sum(x => Math.Pow(x - mean, 2));
            return numerator / data.Length;
        }

        public static double FindFirstQuartile(double[] data)
        {
            //Find the location (L) of the kth percentile.
            double lk = (double)25 / 100 * data.Length;
            double[] orderedData = data.OrderBy(x => x).ToArray();
            return !IsInt(lk) ? orderedData[(int)RoundUp(lk,0) - 1] :
                (orderedData[(int)RoundUp(lk, 0) - 1] + orderedData[(int)RoundUp(lk, 0)]) / 2;
        }

        public static double FindSecondQuartile(double[] data)
        {
            //Find the location (L) of the kth percentile.
            double lk = (double)50 / 100 * data.Length;
            double[] orderedData = data.OrderBy(x => x).ToArray();
            return !IsInt(lk) ? orderedData[(int)RoundUp(lk, 0) - 1] :
                (orderedData[(int)RoundUp(lk, 0) - 1] + orderedData[(int)RoundUp(lk, 0)]) / 2;
        }

        public static double FindThirdQuartile(double[] data)
        {
            //Find the location (L) of the kth percentile.
            double lk = (double)75 / 100 * data.Length;
            double[] orderedData = data.OrderBy(x => x).ToArray();
            return !IsInt(lk) ? orderedData[(int)RoundUp(lk, 0) - 1] :
                (orderedData[(int)RoundUp(lk, 0) - 1] + orderedData[(int)RoundUp(lk, 0)]) / 2;
        }

        public static double FindPercentile(double[] data, double percentile)
        {
            //Find the location (L) of the kth percentile.
            double lk = (double)percentile / 100 * data.Length;
            double[] orderedData = data.OrderBy(x => x).ToArray();
            return !IsInt(lk) ? orderedData[(int)RoundUp(lk, 0) - 1] :
                (orderedData[(int)RoundUp(lk, 0) - 1] + orderedData[(int)RoundUp(lk, 0)]) / 2;
        }

        public static double CalculateInterQuartileRange(double[] data) => FindThirdQuartile(data) - FindFirstQuartile(data);

        //Calculate the coefficient of variance or CV
        public static double CalculateSampleCV(double[] data)
        {
            return CalculateSampleStandardDeviation(data) / CalculateMean(data) * 100;
        }

        public static double CalculatePopulationCV(double[] data)
        {
            return CalculatePopulationStandardDeviation(data) / CalculateMean(data) * 100;
        }

        //Calculate CV given standard deviation and mean
        public static double CalculateCV(double standardDeviation, double mean)
        {
            return standardDeviation / mean * 100;
        }

        //Calculate Skewness of Data Sk
        public static double CalculateSK(double[] data)
        {
            return (3 * (CalculateMean(data) - CalculateMedian(data))) / CalculateSampleStandardDeviation(data);
        }
    }
}

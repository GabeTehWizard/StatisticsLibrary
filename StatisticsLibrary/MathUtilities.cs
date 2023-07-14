using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsLibrary
{
    public static class MathUtilities
    {
        //Rounds a number down regardless of midpoint. Can be rounded to specific decimal places.
        public static double RoundDown(double num, int decimalPlaces) 
        {
            //Validate Arguments
            if (decimalPlaces < 0)
                throw new ArgumentException("Decimal places must be a non-negative whole number.");
            if (double.IsNaN(num) || double.IsInfinity(num))
                throw new ArgumentException("Please ensure that your numeric input is valid.");

            checked
            {   
                //Multiplies a number by a power of 10 equivalent to the amount of decimal places desired,
                //then truncates any decimals, and divides by the power it was raised to.
                double multiplier = Math.Pow(10, decimalPlaces);
                return Math.Truncate(num * multiplier) / multiplier;
            }
        }

        //Rounds a number up regardless of midpoint. Can be rounded to specific decimal places.
        public static double RoundUp(double num, int decimalPlaces)
        {
            //Validate Arguments
            if (decimalPlaces < 0)
                throw new ArgumentException("Decimal places must be a non-negative whole number.");
            if (double.IsNaN(num) || double.IsInfinity(num))
                throw new ArgumentException("Please ensure that your numeric input is valid.");

            checked
            {
                //Multiplies a number by a power of 10 equivalent to the amount of decimal places desired,
                //then selects the next highest integer, and divides by the power it was raised to.
                double power = Math.Pow(10, decimalPlaces);
                return Math.Ceiling(num * power) / power;
            }
        }

        public static double Round(double num, int decimalPlaces)
        {
            double power = Math.Pow(10, decimalPlaces);
            return Math.Round(num * power, MidpointRounding.AwayFromZero) / power;
        }

        public static double ShiftDecimalPlaces(double num, int decimalPlaces)
        {
            double power = Math.Pow(10, decimalPlaces);
            return num / power;
        }

        public static bool IsInt(double num) => Math.Abs(num % 1) <= (double.Epsilon * 100);

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid argument count. It should be: triangle.exe <side1> <side2> <side3>");
                return;
            }
            try
            {
                double[] sides = CheckArguments(args);
                Console.WriteLine(GetTriangleType(sides));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string GetTriangleType(double[] sides)
        {
            double a = sides[0];
            double b = sides[1];
            double c = sides[2];

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                return "Triangle does not exist";
            }

            else if (a == b && b == c)
            {
                return "Equilateral";
            }

            else if (a == b || a == c || b == c)
            {
                return "Isosceles";
            }

            return "Usual";
        }

        public static double[] CheckArguments(string[] args)
        {
            double[] result = new double[3];

            try
            {
                result[0] = Double.Parse(args[0], CultureInfo.InvariantCulture);
                result[1] = Double.Parse(args[1], CultureInfo.InvariantCulture);
                result[2] = Double.Parse(args[2], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new FormatException("Sides should be numeric");
            }


            if (result[0] == 0 || result[1] == 0 || result[2] == 0)
            {
                throw new ArgumentException("Triangle sides should not be equal to 0");
            }

            return result;
        }
    }
}

﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Functions
{
    public class MathFunctions:IMathFunc
    {
        public double GetAverageValue(List<double> list)
        {
            double sum = 0;
            foreach (var v in list)
            {
                sum += v;
            }
            return sum / list.Count; 
        }

        internal static Dictionary<double, int> GetRepeat(List<double> list)
        {
            Dictionary<double, int> repeatPairs = new Dictionary<double, int>();
            repeatPairs = list.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .ToDictionary(x => x.Key, y => y.Count());
            return repeatPairs;
        }
        public double GetExpectedValue(List<double> list)
        {
            double sum = 0;
            Dictionary<double, int> repeat = GetRepeat(list);
            foreach (var item in list)
            {
                int itemcount = repeat.Where(x => x.Key == item).FirstOrDefault().Value;
                sum += item * (itemcount / list.Count);
            }
            return sum;
        }

        public double GetDispersion(List<double> list)
        {
            double MathExecValue = GetExpectedValue(list.Select(x => x * x).ToList());
            return MathExecValue - Math.Pow((GetExpectedValue(list)), 2);
        }
   
        public double GetSquareDeviation(List<double> list)
        {
            return Math.Sqrt(GetDispersion(list));
        }
    }
}

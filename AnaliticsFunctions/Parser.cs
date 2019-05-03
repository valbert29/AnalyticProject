﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnaliticsFunctions
{
    class Parser
    {
        public static Dictionary<string, double> Reader(string path)
        {
            Dictionary<string, double> values = new Dictionary<string, double>();
            StreamReader sr = new StreamReader(path);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                values.Add(line.Replace(@"(\w*): ", ""), double.Parse(line.Replace(@":(\w*)", "")));
            }
            sr.Close();
            return values;            
        }

        public static List<string> GetNames(Dictionary<string, double> dictionary) =>
            dictionary.Select(x => x.Key).ToList();

        public static List<double> GetValues(Dictionary<string, double> dictionary) =>
            dictionary.Select(x => x.Value).ToList();

    }
}

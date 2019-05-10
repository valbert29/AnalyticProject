using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Functions
{
    public class Parser
    {
        public static Dictionary<string, double> Reader(string path)
        {
            Dictionary<string, double> values = new Dictionary<string, double>();
            StreamReader sr = new StreamReader(path);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                string[] arr = line.Split(':');
                values.Add(arr[0], double.Parse(arr[1].Replace(" ", "")));
            }
            sr.Close();
            return values;
        }

        public static string[] GetArray(Dictionary<string, double> dict)
        {
            string[] arrNames = GetNames(dict).ToArray();
            double[] arrValues = GetValues(dict).ToArray();
            string[] arr = new string[dict.Count()];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arrNames[i] +", "+ arrValues[i];
            }
            return arr;
        }

        public static List<string> GetNames(Dictionary<string, double> dictionary) =>
            dictionary.Select(x => x.Key).ToList();

        public static List<double> GetValues(Dictionary<string, double> dictionary) =>
            dictionary.Select(x => x.Value).ToList();

        public static string[] GetMethods()
        {
            return Array.ConvertAll<MethodInfo, String>(typeof(MathFunctions)
                .GetMethods(), delegate (MethodInfo fo)
                { return fo.Name; });
        }

        public static void MethodCall(string name)
        {
            MathFunctions mf = new MathFunctions();
            MethodInfo m = mf.GetType().GetMethod(name);
            m.Invoke(mf, null);
        }

        public static string FindLastFile(string path)
        {
            path = path.Replace(@"~\", "");
            var directory = new DirectoryInfo(path);
            var myFile = directory
                .GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First();
            return myFile.FullName;
        }
    }
}

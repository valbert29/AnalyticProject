using System;
using System.Collections.Generic;
using System.Text;

namespace AnaliticsFunctions
{
    public interface IParser
    {
        Dictionary<string, double> Reader(string path);
        List<string> GetNames(Dictionary<string, double> dictionary);
        List<double> GetValues(Dictionary<string, double> dictionary);
        string[] GetMethods();
        string FindLastFile(string path);
    }
}

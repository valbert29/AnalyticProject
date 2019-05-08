using System;
using System.Collections.Generic;
using System.Text;

namespace AnaliticsFunctions
{
    public interface IMathFunc
    {
        double GetAverageValue(List<double> list);
        double GetExpectedValue(List<double> list);
        double GetDispersion(List<double> list);
        double GetSquareDeviation(List<double> list);
    }
}

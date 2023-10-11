using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Console
{
    public interface IReport
    {
        void GenerateReport(DateOnly monthDate);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    public class ComparisonInfo
    {
        public List<ComparisonRun> Runs;
        public readonly string SourceDirectory;

        public ComparisonInfo(string sourceDirectory)
        {
            SourceDirectory = sourceDirectory;
            this.Runs = new List<ComparisonRun>();
        }

        public bool AnyDiscrepancies
        {
            get
            {
                return Runs.Any(run => run.Discrepancies.Any());
            }
        }
    }

    public class ComparisonRun
    {
        public readonly string ComparisonDirectory;
        public List<string> Discrepancies;

        public ComparisonRun(string comparisonDirectory)
        {
            ComparisonDirectory = comparisonDirectory;
            Discrepancies = new List<string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    internal class CompareEngine
    {
        readonly Dictionary<string, List<Type>> items;
        readonly TypeComparer comparer;
        public CompareEngine(Dictionary<string, List<Type>> items)
        {
            this.items = items;
            comparer = new TypeComparer();
        }

        public ComparisonInfo Analyze()
        {
            var source = items.First();
            var sourceTypes = source.Value;
            var info = new ComparisonInfo(source.Key);
            foreach(var item in items)
            {
                if (item.Key.Equals(source.Key)) { continue; }
                info.Runs.Add(Compare(sourceTypes, item.Value, item.Key));
            }
            return info;
        }

        ComparisonRun Compare(List<Type> sourceTypes, List<Type> comparisonTypes, string comparisonDirectory)
        {
            var run = new ComparisonRun(comparisonDirectory);
            object lockObj = new object();

            Parallel.ForEach(sourceTypes, sourceType =>
            {
                var compareType = comparisonTypes.FirstOrDefault(c => c.FullName.Equals(sourceType.FullName));
                if (compareType == null)
                {
                    run.Discrepancies.Add($"{sourceType.FullName} does not exist");
                    return;
                }
                var discrepancies = comparer.CompareTypes(sourceType, compareType);
                run.Discrepancies.AddRange(discrepancies);
            });

            return run;
        }
    }
}

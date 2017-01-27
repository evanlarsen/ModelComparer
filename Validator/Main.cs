namespace Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Main
    {
        readonly string[] directories;
        public Main(params string[] directories)
        {
            this.directories = directories;
        }

        public ComparisonInfo Run()
        {
            var comparisonItems = new Dictionary<string, List<Type>>();
            var goodDirectories = directories.Where(dir => !string.IsNullOrWhiteSpace(dir));

            if (goodDirectories.Count() < 2)
            {
                throw new ArgumentException("Must pass in two directories or else there is nothing to compare.");
            }

            foreach(string directory in goodDirectories)
            {
                comparisonItems.Add(directory, AssemblyReflector.GetCachesFrom(directory));
            }
            var engine = new CompareEngine(comparisonItems);
            var info = engine.Analyze();

            return info;
        }
    }
}

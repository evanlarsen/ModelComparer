using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    internal class AssemblyReflector
    {
        public static List<Type> GetCachesFrom(string directory)
        {
            var caches = new List<Type>();
            string[] files = Directory.GetFiles(directory, "*.dll");
            foreach (string file in files)
            {
                Type[] types;
                try
                {
                    types = Assembly.LoadFrom(file).GetTypes();
                }
                catch
                {
                    continue;
                }
                var cachesQuery =
                    from type in types
                    where type.Name.EndsWith("Cache")
                    && type.Namespace.Contains("AlaskaAir")
                    && type.IsClass && type.IsPublic
                    select type;

                caches.AddRange(cachesQuery);
            }
            return caches;
        }
    }
}

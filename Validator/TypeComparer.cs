using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    internal class TypeComparer
    {
        
        public List<string> CompareTypes(Type source, Type compare)
        {
            var discrepancies = new List<string>();
            if (TypeDeterminer.IsSimple(source)) // int, bool, string,
            {
                discrepancies.AddRange(CompareSimpleTypes(source, compare));
            }
            else if (TypeDeterminer.IsGenericList(source))
            {
                discrepancies.AddRange(CompareGenericList(source, compare));
            }
            else
            {
                discrepancies.AddRange(CompareClasses(source, compare));
            }

            return discrepancies;
        }

        List<string> CompareClasses(Type source, Type compare)
        {
            var discrepancies = new List<string>();
            if (!source.FullName.Equals(compare.FullName))
            {
                discrepancies.Add($"{source.FullName} does not match {compare.FullName}");
            }
            var compareProps = GetProperties(compare);
            foreach (var sourceProp in GetProperties(source))
            {
                var compareProp = FindSameProperty(sourceProp, compareProps);
                if (compareProp == null)
                {
                    discrepancies.Add($"{GetPropertyInfoFullName(sourceProp)} - could not find matching property");
                }
                discrepancies.AddRange(CompareProperties(sourceProp, compareProp));
            }
            var compareFields = GetFields(compare);
            foreach (var sourceField in GetFields(source))
            {
                var compareField = FindSameField(sourceField, compareFields);
                if (compareField == null)
                {
                    discrepancies.Add($"{GetFieldInfoFullName(sourceField)} - could not find matching field");
                }
                discrepancies.AddRange(CompareFields(sourceField, compareField));
            }

            return discrepancies;
        }

        List<string> CompareProperties(PropertyInfo sourceProp, PropertyInfo compareProp)
        {
            return CompareTypes(sourceProp.PropertyType, compareProp.PropertyType);
        }

        List<string> CompareFields(FieldInfo sourceField, FieldInfo compareField)
        {
            return CompareTypes(sourceField.FieldType, compareField.FieldType);
        }

        List<string> CompareGenericList(Type source, Type compare)
        {
            var discrepancies = new List<string>();
            foreach (Type sourceType in source.GenericTypeArguments)
            {
                var compareType = FindSameType(sourceType, compare.GenericTypeArguments);
                if (compareType == null)
                {
                    discrepancies.Add($"{sourceType.FullName} - could not find matching type");
                    continue;
                }
                discrepancies.AddRange(CompareTypes(sourceType, compareType));
            }
            return discrepancies;
        }

        List<string> CompareSimpleTypes(Type source, Type compare)
        {
            var discrepancies = new List<string>();
            string sourceFullName = GetSimpleTypeFullName(source);
            string compareFullName = GetSimpleTypeFullName(compare);
            if (!sourceFullName.Equals(compareFullName))
            {
                discrepancies.Add($"{sourceFullName} is different from {compareFullName}");
            }
            return discrepancies;
        }

        Type FindSameType(Type sourceType, Type[] compareTypes)
        {
            return compareTypes.FirstOrDefault(compareType => sourceType.FullName.Equals(compareType.FullName));
        }

        PropertyInfo FindSameProperty(PropertyInfo sourceProp, PropertyInfo[] compareProps)
        {
            if (TypeDeterminer.IsSimple(sourceProp.PropertyType))
            {
                string sourcePropName = GetPropertyInfoFullName(sourceProp);
                return compareProps.FirstOrDefault(c => sourcePropName.Equals(GetPropertyInfoFullName(c)));
            }
            else
            {
                return compareProps.FirstOrDefault(prop => prop.PropertyType.FullName.Equals(sourceProp.PropertyType.FullName));
            }
        }

        FieldInfo FindSameField(FieldInfo sourceField, FieldInfo[] compareFields)
        {
            if (TypeDeterminer.IsSimple(sourceField.FieldType))
            {
                string sourceFieldName = GetFieldInfoFullName(sourceField);
                return compareFields.FirstOrDefault(c => sourceFieldName.Equals(GetFieldInfoFullName(c)));
            }
            else
            {
                return compareFields.FirstOrDefault(field => field.FieldType.FullName.Equals(sourceField.FieldType.FullName));
            }
        }

        PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        FieldInfo[] GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        string GetSimpleTypeFullName(Type simpleType)
        {
            return $"{simpleType.FullName}";
        }

        string GetPropertyInfoFullName(PropertyInfo simpleProp)
        {
            if (simpleProp.DeclaringType != null)
            {
                return $"{simpleProp.DeclaringType.FullName}.{simpleProp.Name} - {simpleProp.PropertyType.FullName}";
            }
            return $"{simpleProp.Name} - {simpleProp.PropertyType.FullName}";
        }

        string GetFieldInfoFullName(FieldInfo simpleField)
        {
            if (simpleField.DeclaringType != null)
            {
                return $"{simpleField.DeclaringType.FullName}.{simpleField.Name} - {simpleField.FieldType.FullName}";
            }
            return $"{simpleField.Name} - {simpleField.FieldType.FullName}";
        }
    }
}

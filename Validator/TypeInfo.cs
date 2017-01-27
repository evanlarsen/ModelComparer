using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    internal class TypeInfo
    {
        public readonly string Name;
        public readonly string Type;
        public readonly bool IsClass;
        public readonly List<TypeInfo> Properties;

        public TypeInfo(string name, string type)
        {
            Name = name;
            Type = type;
            IsClass = false;
            Properties = new List<TypeInfo>();
        }

        public TypeInfo(Type type)
        {
            Properties = new List<TypeInfo>();
            Name = type.FullName;
            IsClass = type.IsClass;
            if (type.IsClass && type.IsPublic)
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!IsSimple(prop.PropertyType))
                    {
                        Properties.Add(new TypeInfo(prop.PropertyType));
                    }
                    else
                    {
                        Properties.Add(new TypeInfo($"{prop.DeclaringType.FullName}.{prop.Name}", prop.PropertyType.FullName));
                    }
                }
                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!IsSimple(field.FieldType))
                    {
                        Properties.Add(new TypeInfo(field.FieldType));
                    }
                    else
                    {
                        Properties.Add(new TypeInfo($"{field.DeclaringType.FullName}.{field.Name}", field.FieldType.FullName));
                    }
                }
            }
        }

        bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public override string ToString()
        {
            return Name + (!String.IsNullOrEmpty(Type) ? $" - {Type}" : string.Empty);
        }
    }
}

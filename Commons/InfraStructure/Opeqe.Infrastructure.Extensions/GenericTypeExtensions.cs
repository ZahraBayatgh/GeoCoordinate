using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Opeqe.Infrastructure.Extensions
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }

        public static TResult GetJObjectPropertyValue<TResult>(this object t, string propertyName)
        {
            var jObject = (JObject)t;
            jObject.TryGetValue(propertyName, out JToken value);
            return value.ToObject<TResult>();
        }

    }

}

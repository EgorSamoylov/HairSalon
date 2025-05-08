using System.Reflection;

namespace Infrastructure.Database.TypeMappings
{
    public static class DapperExtensions
    {
        public static Dictionary<string, object> AsDapperParams(this object o)
        {
            var properties = o.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.CanRead)
                .ToArray();

            return properties
                .Select(c => new { Key = c.Name, Value = c.GetValue(o), Type = c.PropertyType })
                .ToDictionary(
                    c => c.Key,
                    c => c.Type.IsEnum || Nullable.GetUnderlyingType(c.Type)
                        ?.IsEnum == true
                        ? c.Value.ToString()
                        : c.Value);
        }
    }
}

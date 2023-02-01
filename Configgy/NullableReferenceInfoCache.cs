using System.Collections.Concurrent;
using System.Reflection;

namespace Configgy;

internal static class NullabilityInfoGetter
{
    private static readonly NullabilityInfoContext _context = new();
    private static readonly ConcurrentDictionary<string, NullabilityInfo> _cache = new();

    public static NullabilityInfo Get(FieldInfo fieldInfo)
    {
        var name = GetKey(fieldInfo);

        return _cache.GetOrAdd(name, x => _context.Create(fieldInfo));
    }
    
    public static NullabilityInfo Get(PropertyInfo propertyInfo)
    {
        var name = GetKey(propertyInfo);

        return _cache.GetOrAdd(name, x => _context.Create(propertyInfo));
    }

    private static string GetKey(MemberInfo memberInfo)
    {
        return memberInfo.DeclaringType?.Assembly.FullName
               + "$" + memberInfo.DeclaringType?.FullName
               + "$" + memberInfo.Name;
    }
}
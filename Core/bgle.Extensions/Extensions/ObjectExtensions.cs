using System.Collections.Generic;
using System.Linq;

public static class ObjectExtensions
{
    public static bool IsNotNull(this object obj)
    {
        return obj != null;
    }

    public static bool IsNull(this object obj)
    {
        return obj == null;
    }

    public static IDictionary<string, object> ToDictionary(this object obj)
    {
        var props = obj.GetType().GetProperties();
        var dic = props.ToDictionary(x => x.Name, x => x.GetValue(obj, null));
        return dic;
    }
}
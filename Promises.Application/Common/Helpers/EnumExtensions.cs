using System.ComponentModel;
using System.Reflection;

namespace Promises.Application.Common.Helpers;

public static class EnumExtentions
{
    public static string GetDescription<T>(this T source)
    { 
        try
        {
            if(source == null) return string.Empty;
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }
    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    public static T GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }
        throw new ArgumentException("Not found.", nameof(description));
    }
}
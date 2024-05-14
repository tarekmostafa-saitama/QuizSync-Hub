using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CleanArchitecture.Application.Common.Helpers;

public static class General
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
    }
}
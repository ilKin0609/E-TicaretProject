using System.Reflection;

namespace E_Ticaret_Project.Application.Shared.Permissions;

public static class PermissionHelper
{
    public static Dictionary<string, List<string>> GetAllPermissions()
    {
        var result = new Dictionary<string, List<string>>();

        var nestedTypes = typeof(Permission).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var moduleType in nestedTypes)
        {
            var allField = moduleType.GetField("All", BindingFlags.Public | BindingFlags.Static);

            if (allField is not null)
            {
                var permissions = allField.GetValue(null) as List<string>;

                if (permissions is not null)

                    result.Add(moduleType.Name, permissions);
            }
        }
        return result;
    }

    public static List<string> GetAllPermissionList()
    {
        return GetAllPermissions().SelectMany(x => x.Value).ToList();

    }
}

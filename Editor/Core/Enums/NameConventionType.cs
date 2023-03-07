using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    public enum NameConventionType
    {
        None = 0,
        CamelCase = 1, // firstName 
        SnakeCase = 2, // first_name 
        KebabkCase = 3, // first-name 
        PascalCase = 4 // FirstName 
    }
}
using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    enum NameConventionType
    {
        /// <summary>
        /// No formatting would apply to provided user variant.
        /// </summary>
        None = 0,
        /// <summary>
        /// Example: firstName
        /// </summary>
        CamelCase = 1,
        
        /// <summary>
        /// Example: first_name
        /// </summary>
        SnakeCase = 2,  
        
        /// <summary>
        /// Example: first-name 
        /// </summary>
        KebabkCase = 3,
        
        /// <summary>
        /// Example: FirstName
        /// </summary>
        PascalCase = 4,
    }
}
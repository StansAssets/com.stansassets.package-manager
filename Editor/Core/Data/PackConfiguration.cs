using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    /// <summary>
    ///     Packages configuration
    /// </summary>
    [Serializable]
    class PackConfiguration
    {
        [SerializeField] string m_Guid;
        [SerializeField] string m_Name = "Default";

        [SerializeField] NamingConvention m_NamingConvention = new NamingConvention();
        [SerializeField] FoldersSpecification m_FoldersSpecification = new FoldersSpecification();
        [SerializeField] GeneralSpecification m_GeneralSpecification = new GeneralSpecification();

        [SerializeField] AssemblyDefinitionsSpecification m_AssemblyDefinitions
            = new AssemblyDefinitionsSpecification();

        internal PackConfiguration()
        {
            m_Guid = System.Guid.NewGuid().ToString();
        }

        internal string Guid => m_Guid;

        internal string Name
        {
            get => m_Name;
            set => m_Name = value;
        }

        internal NamingConvention NamingConvention => m_NamingConvention;

        internal FoldersSpecification Folders => m_FoldersSpecification;

        internal GeneralSpecification General => m_GeneralSpecification;

        internal AssemblyDefinitionsSpecification AssemblyDefinitions => m_AssemblyDefinitions;
    }
}
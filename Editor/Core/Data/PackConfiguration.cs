using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    /// <summary>
    ///     Packages configuration
    /// </summary>
    [Serializable]
    public class PackConfiguration
    {
        [SerializeField] string m_Name = "Default";
        [SerializeField] NamingConvention m_NamingConvention = new NamingConvention();
        [SerializeField] FoldersSpecification m_FoldersSpecification = new FoldersSpecification();
        [SerializeField] GeneralSpecification m_GeneralSpecification = new GeneralSpecification();
        [SerializeField] AssemblyDefinitionsSpecification m_AssemblyDefinitions = new AssemblyDefinitionsSpecification();

        public readonly string Guid;

        public PackConfiguration()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public NamingConvention NamingConvention => m_NamingConvention;

        public FoldersSpecification Folders => m_FoldersSpecification;

        public GeneralSpecification General => m_GeneralSpecification;

        public AssemblyDefinitionsSpecification AssemblyDefinitions => m_AssemblyDefinitions;

        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
    }
}
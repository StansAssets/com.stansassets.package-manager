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

        /// <summary>
        /// Return a copy of this object
        /// </summary>
        /// <returns></returns>
        internal PackConfiguration Copy()
        {
            var copy = new PackConfiguration();
            CopyTo(copy);
            
            return copy;
        }
        
        /// <summary>
        /// Copy data from this object to
        /// </summary>
        /// <param name="to">copy to object</param>
        internal void CopyTo(PackConfiguration to)
        {
            to.Name = Name;

            to.Folders.Runtime = Folders.Runtime;
            to.Folders.RuntimeTests = Folders.RuntimeTests;

            to.Folders.Editor = Folders.Editor;
            to.Folders.EditorTests = Folders.EditorTests;

            to.General.AutoReferenced = General.AutoReferenced;
            to.General.OverrideReferences = General.OverrideReferences;
            to.General.AllowUnsafeCode = General.AllowUnsafeCode;
            to.General.NoEngineReferences = General.NoEngineReferences;

            to.NamingConvention.Prefix = NamingConvention.Prefix;
            to.NamingConvention.Postfix = NamingConvention.Postfix;
            to.NamingConvention.ConventionType = NamingConvention.ConventionType;

            CopyAssemblyDefinitionsSpecification(AssemblyDefinitions, to.AssemblyDefinitions);
        }

        void CopyAssemblyDefinitionsSpecification(
            AssemblyDefinitionsSpecification from,
            AssemblyDefinitionsSpecification to)
        {
            CopyAssemblyDefinitions(from.RuntimeAssemblies, to.RuntimeAssemblies);
            CopyAssemblyDefinitions(from.EditorAssemblies, to.EditorAssemblies);
        }

        void CopyAssemblyDefinitions(
            AssemblyDefinitions from,
            AssemblyDefinitions to)
        {
            to.UseGuids = from.UseGuids;

            to.PrecompiledAssemblies.Clear();
            to.PrecompiledAssemblies
                .AddRange(from.PrecompiledAssemblies);

            to.InternalVisibleToAssemblyDefinitionAssets.Clear();
            to.InternalVisibleToAssemblyDefinitionAssets
                .AddRange(from.InternalVisibleToAssemblyDefinitionAssets);

            to.AssemblyDefinitionAssets.Clear();
            to.AssemblyDefinitionAssets
                .AddRange(from.AssemblyDefinitionAssets);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class AssemblyDefinitions
    {
        bool m_UseGuids;

        [SerializeField] List<AssemblyDefinitionAsset> m_AssemblyDefinitionAssets 
            = new List<AssemblyDefinitionAsset>();

        [SerializeField] List<AssemblyDefinitionAsset> m_InternalVisibleToAssemblyDefinitionAssets 
            = new List<AssemblyDefinitionAsset>();

        [SerializeField] List<string> m_PrecompiledAssemblies = new List<string>();

        internal bool UseGuids
        {
            get => m_UseGuids;
            set => m_UseGuids = value;
        }

        internal List<AssemblyDefinitionAsset> AssemblyDefinitionAssets => m_AssemblyDefinitionAssets;

        internal List<AssemblyDefinitionAsset> InternalVisibleToAssemblyDefinitionAssets =>
            m_InternalVisibleToAssemblyDefinitionAssets;

        internal List<string> PrecompiledAssemblies => m_PrecompiledAssemblies;
    }
}
using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    [Serializable]
    class AssemblyDefinitions
    {
        [SerializeField] bool m_UseGuids;

        [SerializeField] List<AssemblyDefinitionAsset> m_AssemblyDefinitionAssets
            = new List<AssemblyDefinitionAsset>();

        [SerializeField] List<AssemblyDefinitionAsset> m_InternalVisibleToAssemblyDefinitionAssets
            = new List<AssemblyDefinitionAsset>();

        [FormerlySerializedAs("m_PrecompiledAssemblies")] [SerializeField]
        List<string> m_PrecompiledReferences = new List<string>();

        internal bool UseGuids
        {
            get => m_UseGuids;
            set => m_UseGuids = value;
        }

        internal List<AssemblyDefinitionAsset> AssemblyDefinitionAssets => m_AssemblyDefinitionAssets;

        internal List<AssemblyDefinitionAsset> InternalVisibleToAssemblyDefinitionAssets =>
            m_InternalVisibleToAssemblyDefinitionAssets;

        internal List<string> PrecompiledReferences => m_PrecompiledReferences;
    }
}
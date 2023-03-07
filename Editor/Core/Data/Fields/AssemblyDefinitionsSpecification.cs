using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class AssemblyDefinitionsSpecification
    {
        [SerializeField] AssemblyDefinitions m_RuntimeAssemblies = new AssemblyDefinitions();
        [SerializeField] AssemblyDefinitions m_EditorAssemblies = new AssemblyDefinitions();

        internal AssemblyDefinitions RuntimeAssemblies => m_RuntimeAssemblies;
        
        internal AssemblyDefinitions EditorAssemblies => m_EditorAssemblies;
    }
}
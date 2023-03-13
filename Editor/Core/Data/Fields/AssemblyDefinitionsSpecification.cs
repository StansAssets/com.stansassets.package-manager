using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class AssemblyDefinitionsSpecification
    {
        [SerializeField] AssemblyDefinitions m_RuntimeAssemblies = new AssemblyDefinitions();
        [SerializeField] AssemblyDefinitions m_RuntimeTestsAssemblies = new AssemblyDefinitions();
        [SerializeField] AssemblyDefinitions m_EditorAssemblies = new AssemblyDefinitions();
        [SerializeField] AssemblyDefinitions m_EditorTestsAssemblies = new AssemblyDefinitions();

        internal AssemblyDefinitions RuntimeAssemblies => m_RuntimeAssemblies;
        internal AssemblyDefinitions RuntimeTestsAssemblies => m_RuntimeTestsAssemblies;
        
        internal AssemblyDefinitions EditorAssemblies => m_EditorAssemblies;
        internal AssemblyDefinitions EditorTestsAssemblies => m_EditorTestsAssemblies;
    }
}
using System;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class AssemblyDefinitionsSpecification
    {
        public AssemblyDefinitions m_RuntimeAssemblies = new AssemblyDefinitions();
        public AssemblyDefinitions m_EditorAssemblies = new AssemblyDefinitions();
    }
}
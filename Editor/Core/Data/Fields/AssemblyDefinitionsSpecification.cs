using System;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class AssemblyDefinitionsSpecification
    {
        public AssemblyDefinitions RuntimeAssemblies = new AssemblyDefinitions();
        public AssemblyDefinitions EditorAssemblies = new AssemblyDefinitions();
    }
}
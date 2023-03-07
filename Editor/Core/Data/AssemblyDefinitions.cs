using System;
using System.Collections.Generic;
using UnityEditorInternal;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class AssemblyDefinitions
    {
        public bool UseGuids;
        public List<AssemblyDefinitionAsset> AssemblyDefinitionAssets;
        public List<AssemblyDefinitionAsset> InternalVisibleToAssemblyDefinitionAssets;
        public List<string> PrecompiledAssemblies;
    }
}
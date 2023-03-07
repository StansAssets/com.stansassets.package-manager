using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class FoldersSpecification
    {
        public bool Runtime;
        public bool RuntimeTests;
        public bool Editor;
        public bool EditorTests;
    }
}
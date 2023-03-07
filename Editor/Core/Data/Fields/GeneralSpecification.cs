using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class GeneralSpecification
    {
        public bool AllowUnsafeCode;
        public bool AutoReferenced;
        public bool OverrideReferences;
        public bool NoEngineReferences;
    }
}
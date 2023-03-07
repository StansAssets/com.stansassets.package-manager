using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    class GeneralSpecification
    {
        bool m_AllowUnsafeCode;
        bool m_AutoReferenced;
        bool m_OverrideReferences;
        bool m_NoEngineReferences;

        internal bool AllowUnsafeCode
        {
            get => m_AllowUnsafeCode;
            set => m_AllowUnsafeCode = value;
        }

        internal bool AutoReferenced
        {
            get => m_AutoReferenced;
            set => m_AutoReferenced = value;
        }

        internal bool OverrideReferences
        {
            get => m_OverrideReferences;
            set => m_OverrideReferences = value;
        }

        internal bool NoEngineReferences
        {
            get => m_NoEngineReferences;
            set => m_NoEngineReferences = value;
        }
    }
}
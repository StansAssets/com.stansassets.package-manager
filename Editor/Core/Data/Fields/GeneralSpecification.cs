using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class GeneralSpecification
    {
        [SerializeField] bool m_AllowUnsafeCode;
        [SerializeField] bool m_AutoReferenced;
        [SerializeField] bool m_OverrideReferences;
        [SerializeField] bool m_NoEngineReferences;

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
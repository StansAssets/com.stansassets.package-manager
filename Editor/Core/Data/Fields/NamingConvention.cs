using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class NamingConvention
    {
        [SerializeField] string m_DisplayPrefix;
        [SerializeField] string m_NamePrefix;
        [SerializeField] NameConventionType m_ConventionType;

        internal string DisplayPrefix
        {
            get => m_DisplayPrefix;
            set => m_DisplayPrefix = value;
        }

        internal string NamePrefix
        {
            get => m_NamePrefix;
            set => m_NamePrefix = value;
        }

        internal NameConventionType ConventionType
        {
            get => m_ConventionType;
            set => m_ConventionType = value;
        }

        internal NamingConvention Copy()
        {
            return new NamingConvention()
            {
                ConventionType = ConventionType,
                NamePrefix = NamePrefix,
                DisplayPrefix = DisplayPrefix
            };
        }
    }
}
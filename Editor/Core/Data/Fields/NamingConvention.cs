using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class NamingConvention
    {
        [SerializeField] string m_Prefix;
        [SerializeField] string m_Postfix;
        [SerializeField] NameConventionType m_ConventionType;

        internal string Prefix
        {
            get => m_Prefix;
            set => m_Prefix = value;
        }

        internal string Postfix
        {
            get => m_Postfix;
            set => m_Postfix = value;
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
                Postfix = Postfix,
                Prefix = Prefix
            };
        }
    }
}
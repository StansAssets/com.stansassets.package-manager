using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    class NamingConvention
    {
        string m_Prefix;
        string m_Postfix;
        NameConventionType m_ConventionType;

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
    }
}
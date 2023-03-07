using System;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class NamingConvention
    {
        public string m_Prefix;
        public string m_Postfix;
        public NameConventionType m_ConventionType;
    }
}
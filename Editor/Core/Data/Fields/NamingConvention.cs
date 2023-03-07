using System;
using UnityEngine.Serialization;

namespace StansAssets.PackageManager
{
    [Serializable]
    public class NamingConvention
    {
        public string Prefix;
        public string Postfix;
        public NameConventionType ConventionType;
    }
}
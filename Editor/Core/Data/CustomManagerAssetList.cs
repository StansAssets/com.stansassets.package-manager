using System;
using System.Collections.Generic;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class CustomManagerAssetList
    {
        [SerializeField] string m_Name;
        [SerializeField] string m_DisplayName;
        [SerializeField] List<string> m_Packages;

        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }

        public string DisplayName
        {
            get => m_DisplayName;
            set => m_DisplayName = value;
        }

        public List<string> Packages => m_Packages;

        internal CustomManagerAssetList(string name, string displayName, List<string> packages)
        {
            m_Name = name;
            m_DisplayName = displayName;

            m_Packages = new List<string>();
            m_Packages.AddRange(packages);
        }
        
        internal CustomManagerAssetList(string name, string displayName)
        {
            m_Name = name;
            m_DisplayName = displayName;

            m_Packages = new List<string>();
        }
    }
}
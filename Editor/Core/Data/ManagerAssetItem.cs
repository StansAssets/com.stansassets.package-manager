using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class ManagerAssetItem
    {
        [SerializeField] PackageBindType m_PackageBindType;
        [SerializeField] PackageInfo m_PackageJson;
        [SerializeField] string m_PackagePath;
        [SerializeField] bool m_Enabled;

        internal PackageBindType PackageBindType
        {
            get => m_PackageBindType;
            set => m_PackageBindType = value;
        }

        internal PackageInfo PackageJson
        {
            get => m_PackageJson;
            set => m_PackageJson = value;
        }

        internal string PackagePath
        {
            get => m_PackagePath;
            set => m_PackagePath = value;
        }

        internal bool Enabled
        {
            get => m_Enabled;
            set => m_Enabled = value;
        }
    }
}
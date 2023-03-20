using System.Collections.Generic;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.PackageManager
{
    class PackManagerAssetSettings : PackageScriptableSettingsSingleton<PackManagerAssetSettings>
    {
        public override string PackageName => PackageManagerConfig.PackageName;

        [SerializeField] List<ManagerAssetItem> m_PackagesList = new List<ManagerAssetItem>();

        internal List<ManagerAssetItem> PackagesList => m_PackagesList;
    }
}
using System.Collections.Generic;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.PackageManager
{
    class PackManagerAssetSettings : PackageScriptableSettingsSingleton<PackManagerAssetSettings>
    {
        public override string PackageName => PackageManagerConfig.PackageName;
        protected override bool IsEditorOnly => true;

        [SerializeField] List<ManagerAssetItem> m_PackagesList = new List<ManagerAssetItem>();
        [SerializeField] List<CustomManagerAssetList> m_ManagerAssetLists = new List<CustomManagerAssetList>();

        internal List<ManagerAssetItem> PackagesList => m_PackagesList;
        internal List<CustomManagerAssetList> ManagerAssetLists => m_ManagerAssetLists;
    }
}
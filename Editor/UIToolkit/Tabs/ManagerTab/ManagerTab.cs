#if UNITY_2019_4_OR_NEWER

using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ManagerTab : BaseTab
    {
        internal ManagerTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/ManagerTab/ManagerTab")
        {
            Root.Q<Button>("create-package-button").clicked += () =>
            {
                CreateNewPackageWindow.ShowTowardsInspector();
            };
        }
    }
}

#endif
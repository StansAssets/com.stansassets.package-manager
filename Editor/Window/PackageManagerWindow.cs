#if UNITY_2019_4_OR_NEWER

using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.PackageManager.Editor
{
    class PackageManagerWindow : PackageSettingsWindow<PackageManagerWindow>
    {
        internal static GUIContent WindowTitle => new GUIContent(PackageManagerConfig.DisplayName);

        protected override PackageInfo GetPackageInfo() =>
            PackageManagerUtility.GetPackageInfo(PackageManagerConfig.PackageName);

        protected override void OnWindowEnable(VisualElement root)
        {
            AddTab("Manage", new ManagerTab());
            AddTab("Configuration", new ConfigurationTab());
            AddTab("About", new AboutTab());
        }

        void OnDisable()
        {
            PackConfigurationSettings.Save();
        }
    }
}

#endif
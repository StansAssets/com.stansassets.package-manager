using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.PackageManager.Editor
{
    public class PackageManagerWindow : PackageSettingsWindow<PackageManagerWindow>
    {
        public static GUIContent WindowTitle => new GUIContent(PackageManagerConfig.DisplayName);

        protected override PackageInfo GetPackageInfo() =>
            PackageManagerUtility.GetPackageInfo(PackageManagerConfig.PackageName);

        protected override void OnWindowEnable(VisualElement root)
        {
            AddTab("Configuration", new ConfigurationTab());
            // AddTab("Settings", new SettingsTab());
            AddTab("About", new AboutTab());
        }

        void OnDisable()
        {
            PackConfigurationSettings.Save();
        }
    }
}
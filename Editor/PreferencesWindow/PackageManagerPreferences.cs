using System.Collections.Generic;
using StansAssets.Foundation.Editor;
using UnityEditor;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.PackageManager.Editor
{
    public class PackageManagerPreferences : PackagePreferencesWindow
    {
        [SettingsProvider]
        static SettingsProvider ManagerPreferencesItem()
        {
            var provider = new PackageManagerPreferences(
                PackageManagerConfig.RootMenu,
                SettingsScope.Project,
                new[] { "Plugin", "Manager", "Package" })
            {
                label = PackageManagerConfig.DisplayName
            };

            return provider;
        }

        PackageManagerPreferences(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(
            path, scopes, keywords)
        {
        }

        protected override PackageInfo GetPackageInfo()
            => PackageManagerUtility.GetPackageInfo(PackageManagerConfig.PackageName);

        protected override void OnWindowEnable(string searchContext, VisualElement rootElement)
        {
        }
    }
}
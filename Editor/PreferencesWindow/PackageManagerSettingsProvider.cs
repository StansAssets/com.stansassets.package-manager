#if UNITY_2019_4_OR_NEWER

using JetBrains.Annotations;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.PackageManager.Editor
{
    [UsedImplicitly]
    class PackageManagerSettingsProvider : PackagePreferencesWindow
    {
        
        protected override PackageInfo GetPackageInfo()
            => PackageManagerUtility.GetPackageInfo(PackageManagerConfig.PackageName);
        protected override string SettingsPath => $"{PluginsDevKitPackage.RootMenu}/{PackageManagerConfig.DisplayName}";
        protected override SettingsScope Scope => SettingsScope.Project;
        protected override void OnActivate(string searchContext, VisualElement rootElement)
        {
            ContentContainerFlexGrow(1);

            AddTab("Manage", new ManagerTab());
            AddTab("Configuration", new ConfigurationTab());
            AddTab("About", new AboutTab());
        }
        protected override void OnDeactivate() { }
    }
}

#endif

using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor.PackageManager;

namespace StansAssets.PackageManager
{
    static class PackageManagerConfig
    {
        internal const string PackageName = "com.stansassets.package-manager";
        internal const string DisplayName = "Package Manager";
        internal const string DocumentationUrl = "https://github.com/StansAssets/com.stansassets.package-manager";
        internal const string RootMenu = PluginsDevKitPackage.RootMenu + "/" + DisplayName + "/";

        internal const string RootPath = "Packages/" + PackageName;

#if UNITY_2019_4_OR_NEWER
        internal static readonly PackageInfo Info = PackageManagerUtility.GetPackageInfo(PackageName);
#endif
        internal static readonly string WindowTabsPath = $"{RootPath}/Editor/UIToolkit/Tabs";
        internal static readonly string UIToolkitPath = $"{RootPath}/Editor/UIToolkit";
        internal static readonly string ControlsPath = $"{UIToolkitPath}/Controls";
    }
}
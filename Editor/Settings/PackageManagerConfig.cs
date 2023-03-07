using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor.PackageManager;

namespace StansAssets.PackageManager
{
    public static class PackageManagerConfig
    {
        public const string PackageName = "com.stansassets.package-manager";
        public const string DisplayName = "Package Manager";
        public const string DocumentationUrl = "https://github.com/StansAssets/com.stansassets.package-manager";
        public const string RootMenu = PluginsDevKitPackage.RootMenu + "/" + DisplayName + "/";

        public const string RootPath = "Packages/" + PackageName;
        
#if UNITY_2019_4_OR_NEWER
        public static readonly PackageInfo Info = PackageManagerUtility.GetPackageInfo(PackageName);
#endif
        internal static readonly string WindowTabsPath = $"{RootPath}/Editor/UIToolkit/Tabs";
        internal static readonly string UIToolkitPath = $"{RootPath}/Editor/UIToolkit";
        internal static readonly string ControlsPath = $"{UIToolkitPath}/Controls";
    }
}
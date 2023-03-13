using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class DependencyItem
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            $"{PackageManagerConfig.ControlsPath}/" +
            "DependencyItem/" +
            "DependencyItem.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "DependencyItem/" +
                                                     "DependencyItem";

        internal const string NameComponent = "package-name";
        internal const string VersionComponent = "package-version";
    }
}
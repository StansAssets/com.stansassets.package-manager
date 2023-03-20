#if UNITY_2019_4_OR_NEWER

using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class ManagerListItem
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            $"{PackageManagerConfig.WindowTabsPath}/" +
            "ManagerTab/" +
            "ManagerListItem.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "ManagerTab/" +
                                                     "ManagerTab";

        internal const int ItemHeight = 22;

        internal const string NameComponent = "package-name";
        internal const string VersionComponent = "package-version";
        internal const string DefaultEmptyValue = "None";
    }
}

#endif
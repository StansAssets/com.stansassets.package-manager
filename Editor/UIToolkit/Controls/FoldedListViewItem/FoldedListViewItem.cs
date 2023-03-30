#if UNITY_2019_4_OR_NEWER

using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class FoldedListViewItem
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            $"{PackageManagerConfig.ControlsPath}/" +
            "FoldedListViewItem/" +
            "FoldedListViewItem.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "FoldedListViewItem/" +
                                                     "FoldedListViewItem";

        internal const int ItemHeight = 21;

        internal const string NameComponent = "package-name";
        internal const string VersionComponent = "package-version";
        internal const string DefaultEmptyValue = "None";
    }
}

#endif
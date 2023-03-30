#if UNITY_2019_4_OR_NEWER

using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class FoldedListView
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase
            .LoadAssetAtPath<VisualTreeAsset>($"{PackageManagerConfig.ControlsPath}/"
                                              + "FoldedListView/"
                                              + "FoldedListView.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "FoldedListView/" +
                                                     "FoldedListView";

        internal const string ToolbarComponentName = "toggle-toolbar";
    }
}

#endif
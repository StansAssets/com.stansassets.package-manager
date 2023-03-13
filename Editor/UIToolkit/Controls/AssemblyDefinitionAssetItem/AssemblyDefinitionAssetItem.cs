using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class AssemblyDefinitionAssetItem
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            $"{PackageManagerConfig.ControlsPath}/" +
            "AssemblyDefinitionAssetItem/" +
            "AssemblyDefinitionAssetItem.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "AssemblyDefinitionAssetItem/" +
                                                     "AssemblyDefinitionAssetItem";

        internal const string ValueComponent = "item-value";
        internal const string LabelComponent = "item-label";
        internal const string DefaultEmptyValue = "None";
    }
}
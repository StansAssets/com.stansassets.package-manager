using UnityEditor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    static class PrecompiledAssemblyItem
    {
        internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            $"{PackageManagerConfig.ControlsPath}/" +
            "PrecompiledAssemblyItem/" +
            "PrecompiledAssemblyItem.uxml");

        internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                     "PrecompiledAssemblyItem/" +
                                                     "PrecompiledAssemblyItem";

        internal const string ValueComponent = "item-value";
        internal const string LabelComponent = "item-label";
        internal const string DefaultEmptyValue = "None";
        internal const string ErrorValue = "Not found";
    }
}
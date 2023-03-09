#if UNITY_2019_4_OR_NEWER
using StansAssets.PackageManager.Editor;
using UnityEditor;

namespace StansAssets.PackageManager
{
    static class PackageManagerEditorMenu
    {
        [MenuItem(PackageManagerConfig.RootMenu + "/Settings", false, 0)]
        internal static void OpenSettings()
        {
            var windowTitle = PackageManagerWindow.WindowTitle;
            PackageManagerWindow.ShowTowardsInspector(windowTitle.text, windowTitle.image);
        }
    }
}
#endif
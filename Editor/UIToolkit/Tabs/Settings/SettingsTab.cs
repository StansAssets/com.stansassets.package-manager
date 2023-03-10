#if UNITY_2019_4_OR_NEWER
using StansAssets.Plugins.Editor;

namespace StansAssets.PackageManager.Editor
{
    public class SettingsTab : BaseTab
    {
        public SettingsTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/Settings/SettingsTab")
        {
        }
    }
}
#endif
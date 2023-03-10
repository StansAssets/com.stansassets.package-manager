#if UNITY_2019_4_OR_NEWER

using StansAssets.Plugins.Editor;

namespace StansAssets.PackageManager.Editor
{
    class ManagerTab : BaseTab
    {
        internal ManagerTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/ManagerTab/ManagerTab")
        {
            var conf = PackConfigurationSettings.Instance.ActiveConfiguration;
            var assembliesTabs = new TabControl(Root);
          
            assembliesTabs.AddTab("settings", "Settings", new SettingsTab());
            assembliesTabs.AddTab("new-package", "New Package", new NewPackageTab(conf));
            
            assembliesTabs.ActivateTab("settings");
        }
    }
}

#endif
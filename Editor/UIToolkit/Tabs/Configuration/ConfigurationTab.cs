#if UNITY_2019_4_OR_NEWER
using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ConfigurationTab : BaseTab
    {
        internal ConfigurationTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/Configuration/ConfigurationTab")
        {
            var conf = PackConfigurationSettings.Instance.ActiveConfiguration;

            _ = new PackageTemplate(Root, conf);
            BindNamingConvention(Root, conf.NamingConvention);
            BindMinimalUnityVersion(Root, conf.UnityVersion);
        }

        static void BindNamingConvention(VisualElement root, NamingConvention namingConvention)
        {
            var displayPrefix = root.Q<TextField>("display-prefix");
            displayPrefix.SetValueWithoutNotify(namingConvention.DisplayPrefix);
            displayPrefix.RegisterValueChangedCallback(v =>
            {
                namingConvention.DisplayPrefix = v.newValue;
            });

            var namePrefix = root.Q<TextField>("name-prefix");
            namePrefix.SetValueWithoutNotify(namingConvention.NamePrefix);
            namePrefix.RegisterValueChangedCallback(v =>
            {
                namingConvention.NamePrefix = v.newValue;
            });
        }
        
        
        void BindMinimalUnityVersion(VisualElement root, MinimalUnityVersion unityVersion)
        {
            var unityVersionValue = root.Q<TextField>("unity-version");
            unityVersionValue.SetValueWithoutNotify(unityVersion.Unity);
            unityVersionValue.RegisterValueChangedCallback(v => { unityVersion.Unity = v.newValue; });

            var unityReleaseValue = root.Q<TextField>("unity-release");
            unityReleaseValue.SetValueWithoutNotify(unityVersion.UnityRelease);
            unityReleaseValue.RegisterValueChangedCallback(v => { unityVersion.UnityRelease = v.newValue; });
        }
    }
}
#endif
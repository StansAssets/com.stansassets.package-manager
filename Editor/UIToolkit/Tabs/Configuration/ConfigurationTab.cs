#if UNITY_2019_4_OR_NEWER
using StansAssets.Plugins.Editor;
using UnityEditor.UIElements;
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
                PackConfigurationSettings.Save();
            });

            var namePrefix = root.Q<TextField>("name-prefix");
            namePrefix.SetValueWithoutNotify(namingConvention.NamePrefix);
            namePrefix.RegisterValueChangedCallback(v =>
            {
                namingConvention.NamePrefix = v.newValue;
                PackConfigurationSettings.Save();
            });
            
            
            var convention = root.Q<EnumField>("naming-convention");
            convention.Init(namingConvention.ConventionType);
            convention.RegisterValueChangedCallback(evt =>
            {
                namingConvention.ConventionType = (NameConventionType) evt.newValue;
                PackConfigurationSettings.Save();
            });
        }
        
        
        void BindMinimalUnityVersion(VisualElement root, MinimalUnityVersion unityVersion)
        {
            var unityVersionValue = root.Q<TextField>("unity-version");
            unityVersionValue.SetValueWithoutNotify(unityVersion.Unity);
            unityVersionValue.RegisterValueChangedCallback(v =>
            {
                unityVersion.Unity = v.newValue;
                PackConfigurationSettings.Save();
            });
        }
    }
}
#endif
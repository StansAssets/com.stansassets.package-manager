#if UNITY_2019_4_OR_NEWER
using StansAssets.Plugins.Editor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ConfigurationTab : BaseTab
    {
        public ConfigurationTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/Configuration/ConfigurationTab")
        {
            var conf = PackConfigurationSettings.Instance.ActiveConfiguration;
            
            _ = new PackageTemplate(Root, conf);
            BindNamingConvention(Root, conf.NamingConvention);
        }

        static void BindNamingConvention(VisualElement root, NamingConvention namingConvention)
        {
            var prefix = root.Q<TextField>("prefix");
            prefix.SetValueWithoutNotify(namingConvention.m_Prefix);
            prefix.RegisterValueChangedCallback(v => namingConvention.m_Prefix = v.newValue);

            var postfix = root.Q<TextField>("postfix");
            postfix.SetValueWithoutNotify(namingConvention.m_Postfix);
            postfix.RegisterValueChangedCallback(v => namingConvention.m_Postfix = v.newValue);

            var nameConvention = root.Q<EnumField>("name-convention");
            nameConvention.SetValueWithoutNotify(namingConvention.m_ConventionType);
            nameConvention.RegisterValueChangedCallback(v => namingConvention.m_ConventionType = (NameConventionType)v.newValue);
        }
    }
}
#endif
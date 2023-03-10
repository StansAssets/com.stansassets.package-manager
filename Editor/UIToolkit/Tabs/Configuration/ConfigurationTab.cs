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

            UpdatePreview(Root, conf.NamingConvention);
        }

        static void BindNamingConvention(VisualElement root, NamingConvention namingConvention)
        {
            var prefix = root.Q<TextField>("prefix");
            prefix.SetValueWithoutNotify(namingConvention.Prefix);
            prefix.RegisterValueChangedCallback(v =>
            {
                namingConvention.Prefix = v.newValue;
                UpdatePreview(root, namingConvention);
            });

            var postfix = root.Q<TextField>("postfix");
            postfix.SetValueWithoutNotify(namingConvention.Postfix);
            postfix.RegisterValueChangedCallback(v =>
            {
                namingConvention.Postfix = v.newValue;
                UpdatePreview(root, namingConvention);
            });

            var nameConvention = root.Q<EnumField>("name-convention");
            nameConvention.SetValueWithoutNotify(namingConvention.ConventionType);
            nameConvention.RegisterValueChangedCallback(v =>
            {
                namingConvention.ConventionType = (NameConventionType)v.newValue;
                UpdatePreview(root, namingConvention);
            });

            var namingPreview = root.Q<TextField>("naming-preview-value");
            namingPreview.RegisterValueChangedCallback(evt => UpdatePreview(root, namingConvention));
        }

        static void UpdatePreview(VisualElement root, NamingConvention namingConvention)
        {
            var namingPreview = root.Q<TextField>("naming-preview");
            var namingPreviewValue = root.Q<TextField>("naming-preview-value");

            var name = NameConventionBuilder.BuildName(namingPreviewValue.value, namingConvention);
            namingPreview.SetValueWithoutNotify(name);
        }
    }
}
#endif
#if UNITY_2019_4_OR_NEWER

using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class NewPackageTab : BaseTab
    {
        readonly PackConfiguration m_PackConfiguration;

        VisualElement m_TemplateVisualElement;

        internal NewPackageTab(PackConfiguration packConfiguration)
            : base($"{PackageManagerConfig.WindowTabsPath}/NewPackageTab/NewPackageTab")
        {
            m_PackConfiguration = packConfiguration;
            var packageData = new PackageData(m_PackConfiguration.Copy());

            BindData(Root, packageData);
        }

        void BindData(VisualElement root, PackageData packageData)
        {
            m_TemplateVisualElement = new VisualElement();
            _ = new PackageTemplate(m_TemplateVisualElement, packageData.Configuration);
            root.Q<VisualElement>("template-container").Add(m_TemplateVisualElement);

            BindToolbar(root, packageData);
            BindNaming(root, packageData);
            UpdatePreview(root, packageData.Name, packageData.Configuration.NamingConvention);
        }

        void RebindData(VisualElement root, PackageData packageData)
        {
            m_PackConfiguration.CopyTo(packageData.Configuration);

            root.Q<VisualElement>("template-container").Remove(m_TemplateVisualElement);
            BindData(root, packageData);
        }

        void BindToolbar(VisualElement root, PackageData packageData)
        {
            var resetButton = root.Q<Button>("reset-button");
            resetButton.clicked += () => { RebindData(root, packageData); };
        }

        static void BindNaming(VisualElement root, PackageData package)
        {
            var nameValue = root.Q<TextField>("name-value");
            nameValue.RegisterValueChangedCallback(v =>
            {
                package.Name = v.newValue;
                UpdatePreview(root, v.newValue, package.Configuration.NamingConvention);
            });

            var displayNameValue = root.Q<TextField>("display-name-value");
            displayNameValue.RegisterValueChangedCallback(v => { package.DisplayName = v.newValue; });
        }

        static void UpdatePreview(VisualElement root, string newName, NamingConvention namingConvention)
        {
            var namingPreview = root.Q<TextField>("name-preview");
            var value = NameConventionBuilder.BuildName(newName, namingConvention);
            namingPreview.SetValueWithoutNotify(value);
        }
    }
}

#endif
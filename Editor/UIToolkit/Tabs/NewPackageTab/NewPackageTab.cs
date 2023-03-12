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

            var newPackageInfo = new NewPackageInfo(m_PackConfiguration.Copy());

            BindData(Root, newPackageInfo);

            var resetButton = Root.Q<Button>("create-button");
            resetButton.clicked += () => { PackageBuilder.BuildPackage(newPackageInfo); };
        }

        void BindData(VisualElement root, NewPackageInfo packageInfo)
        {
            m_TemplateVisualElement = new VisualElement();
            _ = new PackageTemplate(m_TemplateVisualElement, packageInfo.Configuration);
            root.Q<VisualElement>("template-container").Add(m_TemplateVisualElement);

            BindToolbar(root, packageInfo);
            BindNaming(root, packageInfo);
            UpdatePreview(root, packageInfo.Package.Name, packageInfo);
        }

        void RebindData(VisualElement root, NewPackageInfo packageInfo)
        {
            m_PackConfiguration.CopyTo(packageInfo.Configuration);

            root.Q<VisualElement>("template-container").Remove(m_TemplateVisualElement);
            BindData(root, packageInfo);
        }

        void BindToolbar(VisualElement root, NewPackageInfo packageInfo)
        {
            var resetButton = root.Q<Button>("reset-button");
            resetButton.clicked += () => { RebindData(root, packageInfo); };
        }

        static void BindNaming(VisualElement root, NewPackageInfo packageInfo)
        {
            var package = packageInfo.Package;

            var nameValue = root.Q<TextField>("name-value");
            nameValue.RegisterValueChangedCallback(v => { UpdatePreview(root, v.newValue, packageInfo); });

            var displayNameValue = root.Q<TextField>("display-name-value");
            displayNameValue.RegisterValueChangedCallback(v => { package.DisplayName = v.newValue; });
        }

        static void UpdatePreview(VisualElement root, string newName, NewPackageInfo packageInfo)
        {
            packageInfo.Package.Name = NameConventionBuilder.BuildName(newName, packageInfo.Configuration.NamingConvention);
            root.Q<TextField>("name-preview").SetValueWithoutNotify(packageInfo.Package.Name);

            packageInfo.BaseName = newName;
            var assemblyName = NameConventionBuilder
                .BuildAssemblyName(newName, packageInfo.Configuration.NamingConvention);
            root.Q<TextField>("assembly-preview").SetValueWithoutNotify(assemblyName);
        }
    }
}

#endif
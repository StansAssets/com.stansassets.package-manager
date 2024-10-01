#if UNITY_2019_4_OR_NEWER

using System;
using System.Collections.Generic;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class NewPackageTab : BaseTab
    {
        internal event Action<NewPackageInfo, bool> NewPackageCreated;
        internal event Action NewPackageCanceled;

        readonly PackConfiguration m_PackConfiguration;

        VisualElement m_TemplateVisualElement;

        internal NewPackageTab(PackConfiguration packConfiguration)
            : base($"{PackageManagerConfig.WindowTabsPath}/NewPackageTab/NewPackageTab")
        {
            m_PackConfiguration = packConfiguration;
            var packageInfo = new NewPackageInfo(m_PackConfiguration.Copy());

            BindData(Root, packageInfo);
            BindButtons(Root, packageInfo);
        }

        void BindButtons(VisualElement root, NewPackageInfo packageInfo)
        {
            var createButton = root.Q<Button>("create-button");
            createButton.clicked += () =>
            {
                PackageBuilder.BuildPackage(packageInfo, out var successful);
                NewPackageCreated?.Invoke(packageInfo, successful);
            };

            var cancelButton = root.Q<Button>("cancel-button");
            cancelButton.clicked += () => { NewPackageCanceled?.Invoke(); };

            var resetButton = root.Q<Button>("reset-button");
            resetButton.clicked += () => { RebindData(root, packageInfo); };
        }

        void BindData(VisualElement root, NewPackageInfo packageInfo)
        {
            m_TemplateVisualElement = new VisualElement();
            _ = new PackageTemplate(m_TemplateVisualElement, packageInfo.Configuration);
            root.Q<VisualElement>("template-container").Add(m_TemplateVisualElement);

            BindNaming(root, packageInfo);
            BindVersions(root, packageInfo);
            BindDependencies(root, packageInfo);
        }

        void RebindData(VisualElement root, NewPackageInfo packageInfo)
        {
            m_PackConfiguration.CopyTo(packageInfo.Configuration);

            root.Q<VisualElement>("template-container").Remove(m_TemplateVisualElement);
            BindData(root, packageInfo);
        }

        static void BindNaming(VisualElement root, NewPackageInfo packageInfo)
        {
            const string displayNameComponent = "display-name";
            const string packageNameComponent = "package-name";
            const string assemblyNameComponent = "assembly-name";
            
            const string packageDescriptionComponent = "package-description";

            var package = packageInfo.Package;

            var displayName = root.Q<TextField>(displayNameComponent);
            
            displayName.SetValueWithoutNotify(package.DisplayName);
            displayName.RegisterValueChangedCallback(v =>
            {
                UpdatePackageName(root, v.newValue, packageNameComponent, packageInfo);
                package.DisplayName = v.newValue;
            });
            
            var convention = root.Q<EnumField>("naming-convention");
            convention.Init(packageInfo.Configuration.NamingConvention.ConventionType);
            convention.RegisterValueChangedCallback(evt =>
            {
                packageInfo.Configuration.NamingConvention.ConventionType =  (NameConventionType) evt.newValue;
                UpdatePackageName(root, package.DisplayName, packageNameComponent, packageInfo);
            });

            var packageName = root.Q<TextField>(packageNameComponent);
            packageName.userData = false;
            packageName.SetValueWithoutNotify(package.Name);
            packageName.RegisterValueChangedCallback(v =>
            {
                UpdateAssemblyName(root, v.newValue, packageInfo);

                packageName.userData = true;
                package.Name = v.newValue;
            });

            var assemblyName = root.Q<TextField>(assemblyNameComponent);
            assemblyName.userData = false;
            assemblyName.SetValueWithoutNotify(packageInfo.AssemblyName);
            assemblyName.RegisterValueChangedCallback(v =>
            {
                assemblyName.userData = true;
                packageInfo.AssemblyName = v.newValue;
            });

            var descriptionValue = root.Q<TextField>(packageDescriptionComponent);
            descriptionValue.RegisterValueChangedCallback(v => { package.Description = v.newValue; });
        }

        static void UpdatePackageName(VisualElement root, string name, string elementName, NewPackageInfo packageInfo)
        {
            var textField = root.Q<TextField>(elementName);

            if ((bool)textField.userData == false || string.IsNullOrEmpty(textField.value))
            {
                var namingConvention = packageInfo.Configuration.NamingConvention;
                var formattedName = FormatName(name,
                    namingConvention.DisplayPrefix,
                    namingConvention.NamePrefix.ToLower(),
                    true,
                    NameConventionType.KebabkCase);

                textField.userData = false;
                packageInfo.Package.Name = formattedName;
                textField.SetValueWithoutNotify(formattedName);

                UpdateAssemblyName(root, formattedName, packageInfo);
            }
        }

        static void UpdateAssemblyName(VisualElement root, string name, NewPackageInfo packageInfo)
        {
            var textField = root.Q<TextField>("assembly-name");

            
            if ((bool)textField.userData == false || string.IsNullOrEmpty(textField.value))
            {
                var namingConvention = packageInfo.Configuration.NamingConvention;
                var formattedName = FormatName(name,
                    namingConvention.NamePrefix,
                    namingConvention.NamePrefix,
                    false,
                    namingConvention.ConventionType);

                textField.userData = false;
                packageInfo.AssemblyName = formattedName;
                textField.SetValueWithoutNotify(formattedName);
            }
        }

        static string FormatName(string name, string prefixLeft, string prefixRight, bool removeAbstractPrefix,
            NameConventionType conventionType)
        {
            if (!string.IsNullOrEmpty(prefixLeft))
            {
                name = NameConventionBuilder.RemovePrefix(name, prefixLeft, removeAbstractPrefix);
            }

            var point = "";
            if (!string.IsNullOrEmpty(prefixRight))
            {
                point = prefixRight[prefixRight.Length - 1].Equals('.')
                    ? ""
                    : ".";
            }

            var formattedName = NameConventionBuilder.FormatTextByConvention(name, conventionType);
            formattedName = $"{prefixRight}{point}{formattedName}";

            return formattedName;
        }

        static void BindVersions(VisualElement root, NewPackageInfo packageInfo)
        {
            var package = packageInfo.Package;
            var versionValue = root.Q<TextField>("version-value");
            versionValue.RegisterValueChangedCallback(v => { package.Version = v.newValue; });

            var unityVersionValue = root.Q<TextField>("unity-version");
            unityVersionValue.SetValueWithoutNotify(package.Unity);
            unityVersionValue.RegisterValueChangedCallback(v => { package.Unity = v.newValue; });
        }

        static void BindDependencies(VisualElement root, NewPackageInfo packageInfo)
        {
            var dependencies = new List<(string name, string version)>();
            var listViewMich = root.Q<ListViewMich>("package-dependencies-list");
            listViewMich.InitHeader(new[]
            {
                new HeaderColumn("Package name", 64f),
                new HeaderColumn("Version", 36f),
            });

            listViewMich.AddButton.clicked += () =>
            {
                var list = listViewMich.ListView;
                dependencies.Add(("", ""));

                if (!packageInfo.Package.Dependencies.ContainsKey(""))
                {
                    packageInfo.Package.Dependencies.Add("", "");
                }

                list.RebuildInCompatibleMode();
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                var list = listViewMich.ListView;

                if (list.selectedIndex == -1)
                {
                    return;
                }

                var element = dependencies[list.selectedIndex];
                if (packageInfo.Package.Dependencies.ContainsKey(element.name))
                {
                    packageInfo.Package.Dependencies.Remove(element.name);
                }

                dependencies.RemoveAt(list.selectedIndex);

                list.RebuildInCompatibleMode();
            };

            var listView = listViewMich.ListView;
            listView.itemHeight = 22;
            listView.itemsSource = dependencies;

            listView.makeItem += () =>
            {
                var element = DependencyItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, DependencyItem.ItemComponentStyle);

                var nameField = element.Q<TextField>(DependencyItem.NameComponent);
                nameField.value = "";

                var versionField = element.Q<TextField>(DependencyItem.VersionComponent);
                versionField.value = "";

                return element;
            };

            listView.bindItem = (element, i) =>
            {
                var item = ((string name, string version))listView.itemsSource[i];

                var nameField = element.Q<TextField>(DependencyItem.NameComponent);
                nameField.value = item.name;
                nameField.RegisterValueChangedCallback(evt =>
                {
                    var val = dependencies[i];

                    if (packageInfo.Package.Dependencies.ContainsKey(val.name))
                    {
                        packageInfo.Package.Dependencies.Remove(val.name);
                    }

                    val.name = evt.newValue;
                    dependencies[i] = val;

                    if (!packageInfo.Package.Dependencies.ContainsKey(val.name))
                    {
                        packageInfo.Package.Dependencies.Add(val.name, val.version);
                    }
                });

                var versionField = element.Q<TextField>(DependencyItem.VersionComponent);
                versionField.value = item.version;
                versionField.RegisterValueChangedCallback(evt =>
                {
                    var val = dependencies[i];
                    val.version = evt.newValue;
                    dependencies[i] = val;

                    if (packageInfo.Package.Dependencies.ContainsKey(val.name))
                    {
                        packageInfo.Package.Dependencies[val.name] = evt.newValue;
                    }
                });
            };
        }
    }
}

#endif
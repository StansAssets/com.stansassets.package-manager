#if UNITY_2019_4_OR_NEWER

using System.Collections.Generic;
using System.IO;
using System.Linq;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ManagerTab : BaseTab
    {
        internal ManagerTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/ManagerTab/ManagerTab")
        {
            Root.style.flexGrow = 1;
            Root.Q<Button>("create-package-button").clicked += () => { CreateNewPackageWindow.ShowTowardsInspector(); };

            Root.Q<Button>("discard-assets-button").clicked += () =>
            {
                PackManagerAssetSettings.Instance.PackagesList.Clear();
                BindUnityPackages(Root);
            };

            BindUnityPackages(Root);
        }

        static void BindUnityPackages(VisualElement root)
        {
            FetchDependencies();
            
            var unityList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageJson.name.StartsWith("com.unity."))
                .ToList();
            BindList(root, unityList, "unity");
            
            var fileList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalFile)
                .ToList();
            BindList(root, fileList, "file");

            var localList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalPackages)
                .ToList();
            BindList(root, localList, "local");

            // Display first element
            DisplayPackageDetails(root, unityList.FirstOrDefault());
        }

        static void BindList(VisualElement root, List<ManagerAssetItem> dependencies, string listName)
        {
            var list = root.Q<ListView>($"packages-list-{listName}");
            list.itemHeight = ManagerListItem.ItemHeight;
            list.itemsSource = dependencies;

            list.bindItem = (element, i) =>
            {
                var dependency = dependencies[i];

                var packageName = element.Q<Label>(ManagerListItem.NameComponent);
                packageName.text = dependency.PackageJson.displayName;

                var packageVersion = element.Q<Label>(ManagerListItem.VersionComponent);
                packageVersion.text = dependency.PackageJson.version;
            };

            list.makeItem = () =>
            {
                var element = ManagerListItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, ManagerListItem.ItemComponentStyle);

                var packageName = element.Q<Label>(ManagerListItem.NameComponent);
                packageName.text = ManagerListItem.DefaultEmptyValue;

                var packageVersion = element.Q<Label>(ManagerListItem.VersionComponent);
                packageVersion.text = ManagerListItem.DefaultEmptyValue;

                return element;
            };

            list.onSelectionChanged += objects =>
            {
                if (!objects.Any())
                {
                    return;
                }

                var selectedItem = objects.First() as ManagerAssetItem;
                DisplayPackageDetails(root, selectedItem);
            };

            var foldout = root.Q<Foldout>($"fd-{listName}");
            foldout.value = false;
            foldout.contentContainer.style.flexGrow = 1;
            foldout.contentContainer.style.marginLeft = 0;
            foldout.RegisterValueChangedCallback(evt => { foldout.style.flexGrow = evt.newValue ? 1 : 0; });
        }

        static void DisplayPackageDetails(VisualElement root, ManagerAssetItem managerAssetItem)
        {
            var displayName = root.Q<Label>("display-name");
            var packageName = root.Q<Label>("package-name-value");
            var version = root.Q<Label>("version");
            var description = root.Q<Label>("description");

            if (managerAssetItem == null)
            {
                displayName.text = "";
                packageName.text = "";
                version.text = "";
                description.text = "";
            }
            else
            {
                var package = managerAssetItem.PackageJson;
                displayName.text = package.displayName;
                packageName.text = package.name;
                version.text = package.version;
                description.text = package.description;
            }
        }

        static void FetchDependencies()
        {
            var manifest = new Manifest();
            manifest.Fetch();

            var dependencies = manifest.GetDependencies();
            foreach (var dependency in dependencies)
            {
                var exists = PackManagerAssetSettings.Instance
                    .PackagesList.Any(i => i.PackageJson.name.Equals(dependency.Name));

                if (exists) continue;
                
                var bindType = dependency.Version.StartsWith("file:")
                    ? PackageBindType.LocalFile
                    : PackageBindType.Manifest;

                PackManagerAssetSettings.Instance.PackagesList.Add(new ManagerAssetItem
                {
                    PackageBindType = bindType,
                    PackagePath = dependency.Name,
                    PackageJson = PackageManagerUtility.GetPackageInfo(dependency.Name),
                });
            }

            var packageFilePaths = Directory.GetFiles("Packages", "package.json", SearchOption.AllDirectories);

            foreach (var packageFilePath in packageFilePaths)
            {
                var package = PackageBuilder.UnpackPackage(packageFilePath);

                var exists = PackManagerAssetSettings.Instance
                    .PackagesList.Any(i => i.PackageJson.name.Equals(package.Name));

                if (!exists)
                {
                    PackManagerAssetSettings.Instance.PackagesList.Add(new ManagerAssetItem
                    {
                        PackageBindType = PackageBindType.LocalPackages,
                        PackagePath = package.Name,
                        PackageJson = PackageManagerUtility.GetPackageInfo(package.Name),
                    });
                }
            }
        }
    }
}

#endif
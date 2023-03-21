#if UNITY_2019_4_OR_NEWER

using System.Collections.Generic;
using System.IO;
using System.Linq;

using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ManagerTab : BaseTab
    {
        ManagerAssetItem m_SelectedAssetItem;

        internal ManagerTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/ManagerTab/ManagerTab")
        {
            Root.style.flexGrow = 1;

            BindButtons(Root);
            BindUnityPackages(Root);
        }

        void BindButtons(VisualElement root)
        {
            root.Q<Button>("create-package-button").clicked += () =>
            {
                CreateNewPackageWindow.ShowTowardsInspector();
            };

            root.Q<Button>("discard-assets-button").clicked += () =>
            {
                PackManagerAssetSettings.Instance.PackagesList.Clear();

                if (Directory.Exists(PackageBuilder.LocalPackagesCachePath))
                {
                    FileUtil.DeleteFileOrDirectory(PackageBuilder.LocalPackagesCachePath);
                }

                EditorApplication.ExecuteMenuItem("Assets/Refresh");

                BindUnityPackages(root);
            };

            var activityButton = root.Q<Button>("package-activity-button");
            activityButton.clicked += () =>
            {
                if (m_SelectedAssetItem == null)
                {
                    return;
                }

                if (m_SelectedAssetItem.PackageState == PackageAssetState.Enable)
                {
                    m_SelectedAssetItem.Disable();
                }
                else
                {
                    m_SelectedAssetItem.Enable();
                }

                BindUnityPackages(root);
            };

            root.Q<Button>("update-packages-list-button").clicked += () =>
            {
                BindUnityPackages(root);
            };
        }

        void BindUnityPackages(VisualElement root)
        {
            FetchDependencies();

            var unityList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageJson.name.StartsWith("com.unity."))
                .ToList();
            BindList(root, unityList, "unity");
            
            var gitList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.GitUrl)
                .ToList();
            BindList(root, gitList, "git");

            var fileList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalFile)
                .ToList();
            BindList(root, fileList, "file");

            var localList = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalPackages)
                .ToList();
            BindList(root, localList, "local");

            // Display first element
            m_SelectedAssetItem = m_SelectedAssetItem ?? unityList.FirstOrDefault();
            DisplayPackageDetails(root, m_SelectedAssetItem);
        }

        void BindList(VisualElement root, List<ManagerAssetItem> dependencies, string listName)
        {
            var list = root.Q<ListView>($"packages-list-{listName}");
            list.itemHeight = ManagerListItem.ItemHeight;
            list.itemsSource = dependencies;

            list.bindItem = (element, i) =>
            {
                var dependency = dependencies[i];

                var stateIcon = element.Q<Image>("state-icon");
                stateIcon.image = dependency.StatusIcon;

                var packageName = element.Q<Label>(ManagerListItem.NameComponent);
                packageName.text = $"{dependency.PackageJson.displayName}";

                var packageVersion = element.Q<Label>(ManagerListItem.VersionComponent);
                packageVersion.text = dependency.PackageJson.version;
            };

            list.makeItem = () =>
            {
                var element = ManagerListItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, ManagerListItem.ItemComponentStyle);

                var stateIcon = element.Q<Image>("state-icon");
                stateIcon.scaleMode = ScaleMode.ScaleToFit;

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

                m_SelectedAssetItem = selectedItem;
                DisplayPackageDetails(root, selectedItem);
            };

            var foldout = root.Q<Foldout>($"fd-{listName}");
            foldout.value = m_SelectedAssetItem != null && foldout.value;
            foldout.contentContainer.style.flexGrow = 1;
            foldout.contentContainer.style.marginLeft = 0;
            foldout.RegisterValueChangedCallback(evt =>
            {
                foldout.style.flexGrow = evt.newValue ? 1 : 0;
            });

            if (m_SelectedAssetItem != null && dependencies.Contains(m_SelectedAssetItem))
            {
                foldout.value = true;
                list.selectedIndex = list.itemsSource.IndexOf(m_SelectedAssetItem);
            }
        }

        void DisplayPackageDetails(VisualElement root, ManagerAssetItem managerAssetItem)
        {
            var displayName = root.Q<Label>("display-name");
            var packageName = root.Q<Label>("package-name-value");
            var version = root.Q<Label>("version");
            var description = root.Q<Label>("description");
            var activityButton = root.Q<Button>("package-activity-button");

            if (managerAssetItem == null)
            {
                displayName.text = "";
                packageName.text = "";
                version.text = "";
                description.text = "";
                activityButton.style.display = DisplayStyle.None;
            }
            else
            {
                var package = managerAssetItem.PackageJson;
                displayName.text = package.displayName;
                packageName.text = package.name;
                version.text = package.version;
                description.text = package.description;

                activityButton.style.display =
                    managerAssetItem.PackageState != PackageAssetState.NotFound
                        ? DisplayStyle.Flex
                        : DisplayStyle.None;

                activityButton.text = managerAssetItem
                    .PackageState != PackageAssetState.Disable
                    ? "Disable"
                    : "Enable";
            }
        }

        static void FetchDependencies()
        {
            var manifest = new Manifest();
            manifest.Fetch();

            var dependencies = manifest.GetDependencies().ToList();
            foreach (var dependency in dependencies)
            {
                var exists = PackManagerAssetSettings.Instance
                    .PackagesList.FirstOrDefault(i => i.PackageJson.name.Equals(dependency.Name));

                var bindType = PackageBindType.Manifest;

                if (dependency.Version.StartsWith("file:"))
                {
                    bindType = PackageBindType.LocalFile;
                }
                else if (dependency.Version.StartsWith("http"))
                {
                    bindType = PackageBindType.GitUrl;
                }

                if (exists != null)
                {
                    if (exists.PackageBindType == bindType)
                    {
                        continue;
                    }

                    PackManagerAssetSettings.Instance.PackagesList.Remove(exists);
                }

                PackManagerAssetSettings.Instance.PackagesList.Add(new ManagerAssetItem
                {
                    PackageState = PackageAssetState.Enable,
                    PackageBindType = bindType,
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
                        PackageState = PackageAssetState.Enable,
                        PackageBindType = PackageBindType.LocalPackages,
                        PackageJson = PackageManagerUtility.GetPackageInfo(package.Name),
                    });
                }
            }

            var localPackages = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalPackages)
                .ToList();

            foreach (var assetItem in localPackages)
            {
                var packageRootPath = PackageManagerUtility
                    .GetPackageRootPath(assetItem.PackageJson.name);

                var localExists = Directory.Exists(packageRootPath);
                var inCacheExists
                    = Directory.Exists($"{PackageBuilder.LocalPackagesCachePath}/{assetItem.PackageJson.name}");

                if (localExists)
                {
                    assetItem.PackageState = PackageAssetState.Enable;
                }
                else if ((assetItem.PackageState == PackageAssetState.Enable
                          || assetItem.PackageState == PackageAssetState.NotFound)
                         && inCacheExists)
                {
                    assetItem.PackageState = PackageAssetState.Disable;
                }
                else if (assetItem.PackageState == PackageAssetState.Disable && !inCacheExists)
                {
                    assetItem.PackageState = PackageAssetState.NotFound;
                }
            }

            var localFiles = PackManagerAssetSettings.Instance.PackagesList
                .Where(i => i.PackageBindType == PackageBindType.LocalFile)
                .ToList();

            foreach (var assetItem in localFiles)
            {
                var dirExists = Directory.Exists(assetItem.PackageJson.resolvedPath);

                if (!dirExists)
                {
                    assetItem.PackageState = PackageAssetState.NotFound;
                }
                else if (assetItem.PackageState == PackageAssetState.NotFound)
                {
                    assetItem.PackageState = dependencies.Any(i => i.Name.Equals(assetItem.PackageJson.name))
                        ? PackageAssetState.Enable
                        : PackageAssetState.Disable;
                }
            }
        }
    }
}

#endif
#if UNITY_2019_4_OR_NEWER

using System.Collections;
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

        (string listName, ICollection packages) m_SelectedObjects;
        (string listName, string displayName, List<ManagerAssetItem> packages)[] m_DefaultLists;

        List<ManagerAssetItem> SelectedPackages =>
            (from object o in m_SelectedObjects.packages select o as ManagerAssetItem).ToList();

        readonly Dictionary<string, VisualElement> m_FoldOuts;

        internal ManagerTab()
            : base($"{PackageManagerConfig.WindowTabsPath}/ManagerTab/ManagerTab")
        {
            Root.style.flexGrow = 1;
            m_FoldOuts = new Dictionary<string, VisualElement>();

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
                PackManagerAssetSettings.Instance.ManagerAssetLists.Clear();

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

                m_SelectedAssetItem.SetPackageState(m_SelectedAssetItem.PackageState == PackageAssetState.Enable
                        ? PackageAssetState.Disable
                        : PackageAssetState.Enable,
                    true);

                BindUnityPackages(root);
            };

            root.Q<Button>("update-packages-list-button").clicked += () =>
            {
                BindUnityPackages(root);
            };

            var enableBtn = root.Q<Button>("enable-all-btn");
            enableBtn.clicked += () => SetSelectedCollectionState(root, PackageAssetState.Enable);

            var disableBtn = root.Q<Button>("disable-all-btn");
            disableBtn.clicked += () => SetSelectedCollectionState(root, PackageAssetState.Disable);

            var createCustomListBtn = root.Q<Button>("create-custom-list-btn");
            createCustomListBtn.clicked += () => CreateCustomListBtn(root);

            var removeFromListBtn = root.Q<Button>("remove-from-list-btn");
            removeFromListBtn.clicked += () => RemoveFromListBtn(root);
        }

        void BindUnityPackages(VisualElement root)
        {
            FetchDependencies();

            m_DefaultLists = new (string listName, string displayName, List<ManagerAssetItem> packages)[]
            {
                ("unity", "Unity Technologies", PackManagerAssetSettings.Instance.PackagesList
                    .Where(i => i.PackageJson.name.StartsWith("com.unity.")).ToList()),
                ("git", "Gir URL", PackManagerAssetSettings.Instance.PackagesList
                    .Where(i => i.PackageBindType == PackageBindType.GitUrl).ToList()),
                ("file", "Local Disk", PackManagerAssetSettings.Instance.PackagesList
                    .Where(i => i.PackageBindType == PackageBindType.LocalFile).ToList()),
                ("local", "Local File", PackManagerAssetSettings.Instance.PackagesList
                    .Where(i => i.PackageBindType == PackageBindType.LocalPackages).ToList()),
            };

            foreach (var list in m_DefaultLists)
            {
                BindList(root, list.packages, list.listName, list.displayName);
            }

            // Custom lists
            var orderedCustomList = PackManagerAssetSettings.Instance.ManagerAssetLists
                .OrderBy(i => i.DisplayName).ToArray();

            for (var index = orderedCustomList.Length; index > 0; index--)
            {
                var managerAssetList = orderedCustomList[index - 1];

                if (string.IsNullOrEmpty(managerAssetList.Name))
                {
                    PackManagerAssetSettings.Instance.ManagerAssetLists.Remove(managerAssetList);
                    continue;
                }

                var customList = PackManagerAssetSettings.Instance.PackagesList
                    .Where(i => managerAssetList.Packages.Contains(i.PackageJson.name))
                    .ToList();
                BindList(root, customList, managerAssetList.Name, managerAssetList.DisplayName);

                var fdList = m_FoldOuts[managerAssetList.Name];
                var toolbar = fdList.Q<VisualElement>(FoldedListView.ToolbarComponentName);

                if (toolbar.userData != null && (int)toolbar.userData == 1)
                {
                    toolbar.style.display = DisplayStyle.Flex;
                    continue;
                }

                toolbar.userData = 1;
                toolbar.style.display = DisplayStyle.Flex;

                var enableBtn = new Button()
                {
                    text = "☑",
                    tooltip = "Enable listed packages",
                    style =
                    {
                        width = 16,
                        height = 16,
                    },
                };
                enableBtn.clicked += () =>
                {
                    var list = PackManagerAssetSettings.Instance.PackagesList
                        .Where(i => managerAssetList.Packages.Contains(i.PackageJson.name))
                        .ToList();
                    SetPackagesState(root, list, PackageAssetState.Enable);
                    BindUnityPackages(root);
                };
                toolbar.Add(enableBtn);

                var disableBtn = new Button()
                {
                    text = "☐",
                    tooltip = "Disable listed packages",
                    style =
                    {
                        width = 16,
                        height = 16,
                    },
                };
                disableBtn.clicked += () =>
                {
                    var list = PackManagerAssetSettings.Instance.PackagesList
                        .Where(i => managerAssetList.Packages.Contains(i.PackageJson.name))
                        .ToList();
                    SetPackagesState(root, list, PackageAssetState.Disable);
                    BindUnityPackages(root);
                };
                toolbar.Add(disableBtn);

                var removeListBtn = new Button()
                {
                    text = "☒",
                    tooltip = "Remove list",
                    style =
                    {
                        width = 16,
                        height = 16,
                    },
                };
                removeListBtn.clicked += () =>
                {
                    PackManagerAssetSettings.Instance.ManagerAssetLists.Remove(managerAssetList);
                    BindUnityPackages(root);
                };
                toolbar.Add(removeListBtn);
            }

            // Display first element
            DisplayPackageDetails(root, m_SelectedAssetItem);
            HideMultipleSelectionToolBar(root);

            var folds = PackManagerAssetSettings.Instance.ManagerAssetLists
                .Select(i => i.Name).ToList();
            folds.AddRange(m_DefaultLists.Select(s => s.listName));

            var toCheck = m_FoldOuts.Where(foldOut => !folds.Contains(foldOut.Key)).ToArray();
            foreach (var foldOut in toCheck)
            {
                foldOut.Value.RemoveFromHierarchy();
                m_FoldOuts.Remove(foldOut.Key);
            }
        }

        void BindList(VisualElement root, List<ManagerAssetItem> dependencies, string listName, string displayName)
        {
            VisualElement fdList;

            if (m_FoldOuts.ContainsKey(listName))
            {
                fdList = m_FoldOuts[listName];
            }
            else
            {
                var container = root.Q<VisualElement>("packages-list-container");
                fdList = FoldedListView.ItemComponent.CloneTree();

                m_FoldOuts.Add(listName, fdList);
                container.Add(fdList);

                UIToolkitEditorUtility.ApplyStyle(fdList, FoldedListView.ItemComponentStyle);
            }

            var list = fdList.Q<ListView>();
            list.itemsSource = dependencies;

            list.bindItem = (element, i) =>
            {
                if (dependencies.Count <= i)
                {
                    return;
                }

                var dependency = dependencies[i];

                var stateIcon = element.Q<Image>("state-icon");
                stateIcon.image = dependency.StatusIcon;

                var packageName = element.Q<Label>(FoldedListViewItem.NameComponent);
                packageName.text = $"{dependency.PackageJson.displayName}";

                var packageVersion = element.Q<Label>(FoldedListViewItem.VersionComponent);
                packageVersion.text = dependency.PackageJson.version;
            };

            list.makeItem = () =>
            {
                var element = FoldedListViewItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, FoldedListViewItem.ItemComponentStyle);

                var stateIcon = element.Q<Image>("state-icon");
                stateIcon.scaleMode = ScaleMode.ScaleToFit;

                var packageName = element.Q<Label>(FoldedListViewItem.NameComponent);
                packageName.text = FoldedListViewItem.DefaultEmptyValue;

                var packageVersion = element.Q<Label>(FoldedListViewItem.VersionComponent);
                packageVersion.text = FoldedListViewItem.DefaultEmptyValue;

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
                MultipleItemsSelected(root, listName, objects);
            };

            list.itemHeight = FoldedListViewItem.ItemHeight;
            list.selectionType = SelectionType.Multiple;

            var foldout = fdList.Q<Foldout>();
            foldout.text = $"{displayName} ˙{list.itemsSource.Count}";
            foldout.value = m_SelectedAssetItem != null && foldout.value;
            foldout.contentContainer.style.flexGrow = 1;
            foldout.contentContainer.style.marginLeft = 0;
            foldout.RegisterValueChangedCallback(evt =>
            {
                SetToggleHeight(evt.newValue);
            });
            SetToggleHeight(foldout.value);

            if (m_SelectedAssetItem != null && dependencies.Contains(m_SelectedAssetItem))
            {
                foldout.value = true;
                list.selectedIndex = list.itemsSource.IndexOf(m_SelectedAssetItem);
            }

            var toolbar = fdList.Q<VisualElement>(FoldedListView.ToolbarComponentName);
            toolbar.style.display = DisplayStyle.None;

            void SetToggleHeight(bool v)
            {
                foldout.style.height = v
                    ? list.itemHeight * list.itemsSource.Count + 24 + 21
                    : 24;
                foldout.style.flexGrow = v ? 1 : 0;
            }
        }

        void HideMultipleSelectionToolBar(VisualElement root)
        {
            var toolBar = root.Q<VisualElement>("multiple-selection");
            toolBar.style.display = DisplayStyle.None;
        }

        void MultipleItemsSelected(VisualElement root, string listName, ICollection objects)
        {
            m_SelectedObjects.listName = listName;
            m_SelectedObjects.packages = objects;

            var isDefaultList = m_DefaultLists.Any(i => i.listName.Equals(listName));

            var selectedCount = root.Q<Label>("selected-count");
            selectedCount.text = $"Selected: {objects.Count}";

            var toolBar = root.Q<VisualElement>("multiple-selection");
            toolBar.style.display = DisplayStyle.Flex;

            var removeFromListBtn = root.Q<Button>("remove-from-list-btn");
            removeFromListBtn.style.display = isDefaultList ? DisplayStyle.None : DisplayStyle.Flex;
        }

        void SetSelectedCollectionState(VisualElement rootElement, PackageAssetState state)
        {
            var customList = SelectedPackages;

            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            BindUnityPackages(rootElement);

            SetPackagesState(rootElement, customList, state);
        }

        void SetPackagesState(VisualElement root, List<ManagerAssetItem> customList, PackageAssetState state)
        {
            customList.ForEach(i => i.SetPackageState(state, false));
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            BindUnityPackages(root);
        }

        void CreateCustomListBtn(VisualElement root)
        {
            var packages = SelectedPackages.Select(i => i.PackageJson.name).ToList();

            FavoriteListDialog.ShowDialog("Select a list", assetList =>
            {
                if (assetList == null)
                {
                    return;
                }

                assetList.Packages.AddRange(packages.Where(i => !assetList.Packages.Contains(i)));
                BindUnityPackages(root);
            }, (modified) =>
            {
                if (modified)
                {
                    BindUnityPackages(root);
                }
            });
        }

        void RemoveFromListBtn(VisualElement root)
        {
            var listName = m_SelectedObjects.listName;
            var isDefaultList = m_DefaultLists.Any(i => i.listName.Equals(listName));

            if (isDefaultList)
            {
                return;
            }

            var list = PackManagerAssetSettings.Instance.ManagerAssetLists.First(i => i.Name.Equals(listName));
            foreach (var package in SelectedPackages)
            {
                list.Packages.Remove(package.PackageJson.name);
            }

            BindUnityPackages(root);
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
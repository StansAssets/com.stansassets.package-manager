#if UNITY_2019_4_OR_NEWER

using System;
using System.Linq;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class FavoriteListDialog : EditorWindow
    {
        internal event Action<CustomManagerAssetList> Confirmed;
        internal event Action<bool> Closed;

        bool m_Modified;

        void OnConfirm(CustomManagerAssetList value)
        {
            m_Modified = true;
            Confirmed?.Invoke(value);
        }

        void OnEnable()
        {
            UIToolkitEditorUtility.CloneTreeAndApplyStyle(rootVisualElement,
                $"{PackageManagerConfig.ControlsPath}/FavoriteListDialog/FavoriteListDialog");

            var nameTf = rootVisualElement.Q<TextField>("name-tf");
            var createBtn = rootVisualElement.Q<Button>("create-btn");
            var selectBtn = rootVisualElement.Q<Button>("select-btn");
            var listViewMich = rootVisualElement.Q<ListViewMich>();

            listViewMich.InitHeader("Existing lists");
            listViewMich.AddButton.style.display = DisplayStyle.None;

            var existingLists = listViewMich.ListView;
            var lists = PackManagerAssetSettings.Instance.ManagerAssetLists.ToList();

            existingLists.fixedItemHeight = 16;
            existingLists.itemsSource = lists;
            existingLists.makeItem += () => new Label();
            existingLists.bindItem += (element, i) =>
            {
                if (i == -1)
                {
                    element.Q<Label>().text = $"No data";
                    return;
                }

                var item = existingLists.itemsSource[i] as CustomManagerAssetList;
                Assert.IsNotNull(item);
                element.Q<Label>().text = $"{item.DisplayName} ˙{item.Packages.Count}";
            };

            nameTf.RegisterValueChangedCallback(evt =>
            {
                existingLists.itemsSource = string.IsNullOrEmpty(evt.newValue)
                    ? lists
                    : lists.FindAll(i => i.DisplayName.ToLower().Contains(evt.newValue.ToLower()));
            });

            createBtn.clicked += () =>
            {
                var listName = nameTf.value;
                var formattedName = NameConventionBuilder
                    .FormatTextByConvention(listName, NameConventionType.KebabkCase);
                var existedList = PackManagerAssetSettings.Instance.ManagerAssetLists
                    .FirstOrDefault(i => i.Name.Equals(formattedName));

                if (existedList != null)
                {
                    OnConfirm(existedList);
                }
                else
                {
                    var customList = new CustomManagerAssetList(formattedName, listName);
                    PackManagerAssetSettings.Instance.ManagerAssetLists.Add(customList);
                    OnConfirm(customList);
                }
            };

            selectBtn.clicked += () =>
            {
                if (!currentSelectedItem(out var item)) return;

                OnConfirm(item);
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                if (!currentSelectedItem(out var item)) return;

                PackManagerAssetSettings.Instance.ManagerAssetLists.Remove(item);
                existingLists.itemsSource.Remove(item);
                existingLists.RebuildInCompatibleMode();
                m_Modified = true;
            };

            bool currentSelectedItem(out CustomManagerAssetList item)
            {
                if (existingLists.selectedIndex == -1)
                {
                    item = null;
                    return false;
                }

                item = existingLists.itemsSource[existingLists.selectedIndex] as CustomManagerAssetList;
                Assert.IsNotNull(item);
                return true;
            }
        }

        void OnDisable()
        {
            Closed?.Invoke(m_Modified);
        }

        internal static void ShowDialog(string label, Action<CustomManagerAssetList> confirmed = null,
            Action<bool> modifiedNClosed = null)
        {
            var dialog = CreateInstance<FavoriteListDialog>();

            var windowSize = new Vector2(300, 200);
            dialog.minSize = windowSize;
            dialog.maxSize = windowSize;

            dialog.titleContent.text = label;

            dialog.Confirmed += value => dialog.Close();
            dialog.Closed += modifiedNClosed;

            if (confirmed != null)
            {
                dialog.Confirmed += confirmed;
            }

            dialog.ShowUtility();
        }
    }
}

#endif
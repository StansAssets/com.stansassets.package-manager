using System;
using System.Collections.Generic;
using System.Linq;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;

namespace StansAssets.PackageManager.Editor
{
    class AssembliesTab : BaseTab
    {
        public AssembliesTab(AssemblyDefinitions assemblyDefinitions)
            : base($"{PackageManagerConfig.WindowTabsPath}/Assemblies/AssembliesTab")
        {
            BindUseGuids(Root, assemblyDefinitions);

            BindAssemblyDefinitionAssets(Root,
                "asm-references",
                assemblyDefinitions.AssemblyDefinitionAssets);

            BindAssemblyDefinitionAssets(Root,
                "internal-asm-references",
                assemblyDefinitions.InternalVisibleToAssemblyDefinitionAssets);

            BindPrecompiledAssemblies(Root,
                "asm-precompiled",
                assemblyDefinitions.PrecompiledAssemblies);
        }

        static void BindUseGuids(VisualElement root, AssemblyDefinitions assemblyDefinitions)
        {
            var allowUnsafeCode = root.Q<Toggle>("asd-references-use-guids");
            allowUnsafeCode.SetValueWithoutNotify(assemblyDefinitions.UseGuids);
            allowUnsafeCode.RegisterValueChangedCallback(v => assemblyDefinitions.UseGuids = v.newValue);
        }

        static void BindAssemblyDefinitionAssets(VisualElement root, string listViewName,
            List<AssemblyDefinitionAsset> assemblyDefinitionAssets)
        {
            var listViewMich = root.Q<ListViewMich>(listViewName);

            listViewMich.AddButton.clicked += () =>
            {
                var list = listViewMich.ListView;
                assemblyDefinitionAssets.Add(null);
                list.Refresh();
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                var list = listViewMich.ListView;

                if (list.selectedIndex == -1)
                {
                    return;
                }

                assemblyDefinitionAssets.RemoveAt(list.selectedIndex);
                list.Refresh();
            };

            var listView = listViewMich.ListView;
            listView.itemHeight = 22;
            listView.itemsSource = assemblyDefinitionAssets;

            listView.makeItem += () =>
            {
                var element = AssemblyDefinitionAssetItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, AssemblyDefinitionAssetItem.ItemComponentStyle);

                var label = element.Q<Label>(AssemblyDefinitionAssetItem.LabelComponent);
                label.text = AssemblyDefinitionAssetItem.DefaultEmptyValue;

                var field = element.Q<ObjectField>(AssemblyDefinitionAssetItem.ValueComponent);
                field.objectType = typeof(AssemblyDefinitionAsset);
                field.value = null;

                return element;
            };

            listView.bindItem = (e, i) =>
            {
                var item = listView.itemsSource[i] as AssemblyDefinitionAsset;

                var field = e.Q<ObjectField>(AssemblyDefinitionAssetItem.ValueComponent);
                field.value = item;
                field.RegisterValueChangedCallback(evt =>
                {
                    assemblyDefinitionAssets[i] = evt?.newValue as AssemblyDefinitionAsset;
                    listView.Refresh();
                });

                var label = e.Q<Label>(AssemblyDefinitionAssetItem.LabelComponent);
                label.text = item != null ? item.name : AssemblyDefinitionAssetItem.DefaultEmptyValue;
            };
        }

        static void BindPrecompiledAssemblies(VisualElement root, string listViewName,
            List<string> precompiledAssemblies)
        {
            var playerAssemblies = CompilationPipeline.GetPrecompiledAssemblyNames().ToList();
            playerAssemblies.Insert(0, PrecompiledAssemblyItem.DefaultEmptyValue);

            var assembliesEnum = AssemblyBuilderUtils.GenerateEnumType(
                "PrecompiledAssembly", playerAssemblies.ToArray());
            var assembliesEnumNames = Enum.GetNames(assembliesEnum).ToList();
            var assembliesEnumValues = Enum.GetValues(assembliesEnum);

            var listViewMich = root.Q<ListViewMich>(listViewName);

            listViewMich.AddButton.clicked += () =>
            {
                var list = listViewMich.ListView;
                precompiledAssemblies.Add(PrecompiledAssemblyItem.DefaultEmptyValue);
                list.Refresh();
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                var list = listViewMich.ListView;

                if (list.selectedIndex == -1)
                {
                    return;
                }

                precompiledAssemblies.RemoveAt(list.selectedIndex);
                list.Refresh();
            };

            var listView = listViewMich.ListView;
            listView.itemHeight = 22;
            listView.itemsSource = precompiledAssemblies;

            listView.makeItem += () =>
            {
                var element = PrecompiledAssemblyItem.ItemComponent.CloneTree();
                UIToolkitEditorUtility.ApplyStyle(element, PrecompiledAssemblyItem.ItemComponentStyle);

                var label = element.Q<Label>(PrecompiledAssemblyItem.LabelComponent);
                label.text = PrecompiledAssemblyItem.DefaultEmptyValue;

                var field = element.Q<EnumField>();
                field.Init(assembliesEnumValues.GetValue(0) as Enum, false);

                return element;
            };

            listView.bindItem = (e, i) =>
            {
                var field = e.Q<EnumField>(PrecompiledAssemblyItem.ValueComponent);

                field.RegisterValueChangedCallback(evt =>
                {
                    precompiledAssemblies[i] = evt?.newValue.ToString();
                    listView.Refresh();
                });

                var item = listView.itemsSource[i];

                if (item == null)
                {
                    e.Q<Label>(PrecompiledAssemblyItem.LabelComponent)
                        .text = PrecompiledAssemblyItem.DefaultEmptyValue;
                    return;
                }

                var itemValue = assembliesEnumValues
                    .GetValue(assembliesEnumNames.IndexOf(item.ToString())) as Enum;

                if (itemValue == null)
                {
                    e.Q<Label>(PrecompiledAssemblyItem.LabelComponent)
                        .text = PrecompiledAssemblyItem.ErrorValue;
                    return;
                }

                field.value = itemValue;

                var label = e.Q<Label>(PrecompiledAssemblyItem.LabelComponent);
                label.text = itemValue.ToString();
            };
        }

        static class AssemblyDefinitionAssetItem
        {
            internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{PackageManagerConfig.ControlsPath}/" +
                "AssemblyDefinitionAssetItem/" +
                "AssemblyDefinitionAssetItem.uxml");

            internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                         "AssemblyDefinitionAssetItem/" +
                                                         "AssemblyDefinitionAssetItem";

            internal const string ValueComponent = "item-value";
            internal const string LabelComponent = "item-label";
            internal const string DefaultEmptyValue = "None";
        }

        static class PrecompiledAssemblyItem
        {
            internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{PackageManagerConfig.ControlsPath}/" +
                "PrecompiledAssemblyItem/" +
                "PrecompiledAssemblyItem.uxml");

            internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                         "PrecompiledAssemblyItem/" +
                                                         "PrecompiledAssemblyItem";

            internal const string ValueComponent = "item-value";
            internal const string LabelComponent = "item-label";
            internal const string DefaultEmptyValue = "None";
            internal const string ErrorValue = "Not found";
        }
    }
}
#if UNITY_2019_4_OR_NEWER

using System;
using System.Collections.Generic;
using System.Linq;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Label = UnityEngine.UIElements.Label;

namespace StansAssets.PackageManager.Editor
{
    class AssembliesTab : BaseTab
    {
        internal AssembliesTab(AssemblyDefinitions assemblyDefinitions)
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
                assemblyDefinitions.PrecompiledReferences);
        }

        static void BindUseGuids(VisualElement root, AssemblyDefinitions assemblyDefinitions)
        {
            var allowUnsafeCode = root.Q<Toggle>("asd-references-use-guids");
            allowUnsafeCode.SetValueWithoutNotify(assemblyDefinitions.UseGuids);
            allowUnsafeCode.RegisterValueChangedCallback(v =>
            {
                assemblyDefinitions.UseGuids = v.newValue;
                PackConfigurationSettings.Save();
            });
        }

        static void BindAssemblyDefinitionAssets(VisualElement root, string listViewName,
            List<AssemblyDefinitionAsset> assemblyDefinitionAssets)
        {
            var listViewMich = root.Q<ListViewMich>(listViewName);

            listViewMich.AddButton.clicked += () =>
            {
                var list = listViewMich.ListView;
                assemblyDefinitionAssets.Add(null);
                list.RebuildInCompatibleMode();
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                var list = listViewMich.ListView;

                if (list.selectedIndex == -1)
                {
                    return;
                }

                assemblyDefinitionAssets.RemoveAt(list.selectedIndex);
                list.RebuildInCompatibleMode();
                PackConfigurationSettings.Save();
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
                field.SetValueWithoutNotify(item);
                
                
                //That will generate double callback, but we will leave with it for a while
                field.RegisterValueChangedCallback(evt =>
                {
                    assemblyDefinitionAssets[i] = evt?.newValue as AssemblyDefinitionAsset;
                    listView.RebuildInCompatibleMode();
                    PackConfigurationSettings.Save();
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
                list.RebuildInCompatibleMode();
            };

            listViewMich.RemoveButton.clicked += () =>
            {
                var list = listViewMich.ListView;

                if (list.selectedIndex == -1)
                {
                    return;
                }

                precompiledAssemblies.RemoveAt(list.selectedIndex);
                list.RebuildInCompatibleMode();
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

                //That will generate double callback, but we will leave with it for a while
                field.RegisterValueChangedCallback(evt =>
                {
                    precompiledAssemblies[i] = evt?.newValue.ToString();
                    listView.RebuildInCompatibleMode();
                    PackConfigurationSettings.Save();
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

                field.SetValueWithoutNotify(itemValue);

                var label = e.Q<Label>(PrecompiledAssemblyItem.LabelComponent);
                label.text = itemValue.ToString();
            };
        }
    }
}

#endif
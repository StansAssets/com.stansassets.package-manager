using System.Collections.Generic;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class AssembliesTab : BaseTab
    {
        static class AssemblyDefinitionAssetItem
        {
            internal static VisualTreeAsset ItemComponent => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                $"{PackageManagerConfig.ControlsPath}/" +
                "AssemblyDefinitionAssetItem/" +
                "AssemblyDefinitionAssetItem.uxml");

            internal static string ItemComponentStyle => $"{PackageManagerConfig.ControlsPath}/" +
                                                         $"{nameof(AssemblyDefinitionAssetItem)}/" +
                                                         $"{nameof(AssemblyDefinitionAssetItem)}";

            internal const string ValueComponent = "item-value";
            internal const string LabelComponent = "item-label";
            internal const string DefaultEmptyValue = "None";
        }

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

                var field = element.Q<ObjectField>();
                field.objectType = typeof(AssemblyDefinitionAsset);
                field.value = null;

                field.RegisterValueChangedCallback(evt =>
                {
                    if (listView.selectedIndex == -1)
                    {
                        return;
                    }

                    assemblyDefinitionAssets[listView.selectedIndex] = evt?.newValue as AssemblyDefinitionAsset;
                    listView.Refresh();
                });

                return element;
            };

            listView.bindItem = (e, i) =>
            {
                var item = listView.itemsSource[i] as AssemblyDefinitionAsset;

                var field = e.Q<ObjectField>(AssemblyDefinitionAssetItem.ValueComponent);
                field.value = item;

                var label = e.Q<Label>(AssemblyDefinitionAssetItem.LabelComponent);
                label.text = item != null ? item.name : AssemblyDefinitionAssetItem.DefaultEmptyValue;
            };
        }
    }

    // public static class AssemblyLister
    // {
    //     [MenuItem("Tools/List Assemblies in Console")]
    //     public static void PrintAssemblyNames()
    //     {
    //         var playerAssemblies = CompilationPipeline.GetPrecompiledAssemblyNames();
    //
    //         foreach (var assembly in playerAssemblies)
    //         {
    //             UnityEngine.Debug.Log(assembly.ToString());
    //         }
    //     }
    // }
}
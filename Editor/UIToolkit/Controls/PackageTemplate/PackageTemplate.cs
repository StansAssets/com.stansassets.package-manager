﻿#if UNITY_2019_4_OR_NEWER
using StansAssets.Foundation.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class PackageTemplate : VisualElement
    {
        internal PackageTemplate(VisualElement root, PackConfiguration packConfiguration)
        {
            UIToolkitEditorUtility.CloneTreeAndApplyStyle(root,
                $"{PackageManagerConfig.ControlsPath}/PackageTemplate/PackageTemplate");

            BindFoldersFields(root, packConfiguration.Folders);
            BindGeneralFields(root, packConfiguration.General);

            var assemblies = packConfiguration.AssemblyDefinitions;
            var assembliesTabs = new TabControl(root);

            assembliesTabs.AddTab("runtime", "▪ Runtime", new AssembliesTab(assemblies.RuntimeAssemblies));
            assembliesTabs.AddTab("runtime-tests", "▪ Runtime tests", new AssembliesTab(assemblies.RuntimeTestsAssemblies));
            
            assembliesTabs.AddTab("editor", "▪ Editor", new AssembliesTab(assemblies.EditorAssemblies));
            assembliesTabs.AddTab("editor-tests", "▪ Editor tests", new AssembliesTab(assemblies.EditorTestsAssemblies));

            assembliesTabs.ActivateTab("runtime");
        }

        static void BindGeneralFields(VisualElement root, GeneralSpecification general)
        {
            var allowUnsafeCode = root.Q<Toggle>("allow-unsafe-code");
            allowUnsafeCode.SetValueWithoutNotify(general.AllowUnsafeCode);
            allowUnsafeCode.RegisterValueChangedCallback(v =>
            {
                general.AllowUnsafeCode = v.newValue;
                PackConfigurationSettings.Save();
            });

            var noEngineReferences = root.Q<Toggle>("no-engine-references");
            noEngineReferences.SetValueWithoutNotify(general.NoEngineReferences);
            noEngineReferences.RegisterValueChangedCallback(v =>
            {
                general.NoEngineReferences = v.newValue;
                PackConfigurationSettings.Save();
            });

            var overrideReferenced = root.Q<Toggle>("override-references");
            overrideReferenced.SetValueWithoutNotify(general.OverrideReferences);
            overrideReferenced.RegisterValueChangedCallback(v =>
            {
                general.OverrideReferences = v.newValue;
                PackConfigurationSettings.Save();
            });

            var autoReferenced = root.Q<Toggle>("auto-referenced");
            autoReferenced.SetValueWithoutNotify(general.AutoReferenced);
            autoReferenced.RegisterValueChangedCallback(v =>
            {
                general.AutoReferenced = v.newValue;
                PackConfigurationSettings.Save();
            });
        }

        static void BindFoldersFields(VisualElement root, FoldersSpecification folders)
        {
            var runtimeTests = root.Q<Toggle>("runtime-tests");
            runtimeTests.SetEnabled(folders.Runtime);
            runtimeTests.SetValueWithoutNotify(folders.RuntimeTests);
            runtimeTests.RegisterValueChangedCallback(v =>
            {
                folders.RuntimeTests = v.newValue;
                PackConfigurationSettings.Save();
            });

            var runtime = root.Q<Toggle>("runtime");
            runtime.SetValueWithoutNotify(folders.Runtime);
            runtime.RegisterValueChangedCallback(v =>
            {
                folders.Runtime = v.newValue;
                runtimeTests.SetEnabled(folders.Runtime);
                PackConfigurationSettings.Save();
            });

            var editorTests = root.Q<Toggle>("editor-tests");
            editorTests.SetEnabled(folders.Editor);
            editorTests.SetValueWithoutNotify(folders.EditorTests);
            editorTests.RegisterValueChangedCallback(v =>
            {
                folders.EditorTests = v.newValue;
                PackConfigurationSettings.Save();
            });

            var editor = root.Q<Toggle>("editor");
            editor.SetValueWithoutNotify(folders.Editor);
            editor.RegisterValueChangedCallback(v =>
            {
                folders.Editor = v.newValue;
                editorTests.SetEnabled(folders.Editor);
                PackConfigurationSettings.Save();
            });
        }
    }
}
#endif
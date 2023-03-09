using System.Collections.Generic;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    public class PackageManagerSettingsProvider : SettingsProvider
    {
        readonly string m_WindowUIFilesRootPath = $"{PluginsDevKitPackage.UIToolkitPath}/SettingsWindow";

        TabControl m_TabControl;

        PackageManagerSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            UIToolkitEditorUtility.CloneTreeAndApplyStyle(rootElement,
                $"{m_WindowUIFilesRootPath}/PackageSettingsWindow");

            // Hide our search bar. In preferences we already have search bar
            // and it's value in "searchContext" parameter
            var searchBar = rootElement.Q<ToolbarSearchField>();
            if (searchBar != null)
            {
                searchBar.style.visibility = Visibility.Hidden;
            }

            var packageInfo = PackageManagerUtility.GetPackageInfo(PackageManagerConfig.PackageName);
            rootElement.Q<Label>("display-name").text = packageInfo.displayName.Remove(0, "Stans Assets - ".Length);
            rootElement.Q<Label>("description").text = packageInfo.description;
            rootElement.Q<Label>("version").text = $"Version: {packageInfo.version}";

            m_TabControl = new TabControl(rootElement);

            m_TabControl.AddTab("configuration", "Configuration", new ConfigurationTab());
            m_TabControl.AddTab("about", "About", new AboutTab());

            m_TabControl.ActivateTab("configuration");
        }

        [SettingsProvider]
        static SettingsProvider RegisterSettingsProvider()
        {
            var provider = new PackageManagerSettingsProvider(
                PackageManagerConfig.RootMenu,
                SettingsScope.Project,
                new[] { "Plugin", "Manager", "Package", "Stan", "Assets", "Tool" })
            {
                label = PackageManagerConfig.DisplayName
            };

            return provider;
        }
    }
}
#if UNITY_2019_4_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class CreateNewPackageWindow : EditorWindow
    {
        static GUIContent WindowTitle => new GUIContent("New Package",
            EditorGUIUtility.IconContent("d_AS Badge New").image);

        void OnEnable()
        {
            // This is a workaround due to a very weird bug.
            // During OnEnable we may need to accesses singleton scriptable object associated with the package.
            // And looks like AssetDatabase could be not ready and we will recreate new empty settings objects
            // instead of getting existing one.
            EditorApplication.delayCall += () =>
            {
                var conf = PackConfigurationSettings.Instance.ActiveConfiguration;

                var newPackageTab = new NewPackageTab(conf);
                newPackageTab.NewPackageCanceled += Close;
                newPackageTab.NewPackageCreated += (info, successful) =>
                {
                    if (successful)
                    {
                        Close();
                    }
                };

                var scrollView = new ScrollView();
                scrollView.Add(newPackageTab);

                rootVisualElement.Add(scrollView);
            };
        }

        /// <summary>
        /// Method will show and doc window next to the Inspector Window.
        /// </summary>
        /// <returns>
        /// Returns the first EditorWindow which is currently on the screen.
        /// If there is none, creates and shows new window and returns the instance of it.
        /// </returns>
        internal static CreateNewPackageWindow ShowTowardsInspector()
        {
            var inspectorType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor.dll");
            var window = GetWindow<CreateNewPackageWindow>(inspectorType);
            window.Show();

            window.titleContent = new GUIContent(WindowTitle);
            window.minSize = new Vector2(350, 100);

            return window;
        }
    }
}

#endif
#if UNITY_2019_4_OR_NEWER

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager
{
    class TextDialog : EditorWindow
    {
        internal event Action<string> Confirmed;

        void OnEnable()
        {
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignContent = Align.Stretch,
                    marginTop = 12,
                    marginLeft = 6,
                },
            };

            var textField = new TextField
            {
                label = "Enter value",
                style =
                {
                    flexGrow = 1
                },
            };
            root.Add(textField);

            var confirmBtn = new Button()
            {
                text = "Confirm",
                style =
                {
                    width = 60,
                },
            };
            confirmBtn.clicked += () =>
            {
                Confirmed?.Invoke(textField.text);
            };
            root.Add(confirmBtn);

            rootVisualElement.Add(root);
        }

        internal void ShowDialog(string label)
        {
            minSize = new Vector2(200, 100);
            titleContent.text = label;
            ShowModal();
        }
    }
}

#endif
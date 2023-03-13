#if UNITY_2019_4_OR_NEWER

using JetBrains.Annotations;
using StansAssets.Foundation.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class ListViewMich : VisualElement
    {
        [UsedImplicitly]
        internal new class UxmlFactory : UxmlFactory<ListViewMich, UxmlTraits>
        {
        }

        public ListViewMich()
        {
            UIToolkitEditorUtility.CloneTreeAndApplyStyle(this,
                $"{PackageManagerConfig.ControlsPath}/ListViewMich/ListViewMich");

            Header.style.visibility = Visibility.Hidden;
        }

        internal void InitHeader(string header)
        {
            Header.Add(new HeaderColumn(header).Element);
            Header.style.visibility = Visibility.Visible;
        }

        internal void InitHeader(string[] headerColumns)
        {
            foreach (var column in headerColumns)
            {
                Header.Add(new HeaderColumn(column).Element);
            }
            Header.style.visibility = Visibility.Visible;
        }

        internal void InitHeader(HeaderColumn[] headerColumns)
        {
            foreach (var column in headerColumns)
            {
                Header.Add(column.Element);
            }
            Header.style.visibility = Visibility.Visible;
        }

        internal ListView ListView => this.Q<ListView>();
        internal Button AddButton => this.Q<Button>("add-item");
        internal Button RemoveButton => this.Q<Button>("remove-item");
        internal VisualElement Header => this.Q<VisualElement>("list-header");
    }
}

#endif
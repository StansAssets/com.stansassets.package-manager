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
        }

        internal ListView ListView => this.Q<ListView>();
        internal Button AddButton => this.Q<Button>("add-item");
        internal Button RemoveButton => this.Q<Button>("remove-item");
    }
}

#endif
using JetBrains.Annotations;
using StansAssets.Foundation.Editor;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    public class ListViewMich : VisualElement
    {
        [UsedImplicitly]
        public new class UxmlFactory : UxmlFactory<ListViewMich, UxmlTraits>
        {
        }

        public ListViewMich()
        {
            UIToolkitEditorUtility.CloneTreeAndApplyStyle(this,
                $"{PackageManagerConfig.ControlsPath}/ListViewMich/ListViewMich");
        }

        public ListView ListView => this.Q<ListView>();
        public Button AddButton => this.Q<Button>("add-item");
        public Button RemoveButton => this.Q<Button>("remove-item");
    }
}
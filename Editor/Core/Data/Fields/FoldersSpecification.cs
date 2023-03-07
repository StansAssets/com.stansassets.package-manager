using System;

namespace StansAssets.PackageManager
{
    [Serializable]
    class FoldersSpecification
    {
        bool m_Runtime;
        bool m_RuntimeTests;
        bool m_Editor;
        bool m_EditorTests;

        internal bool Runtime
        {
            get => m_Runtime;
            set => m_Runtime = value;
        }

        internal bool RuntimeTests
        {
            get => m_RuntimeTests;
            set => m_RuntimeTests = value;
        }

        internal bool Editor
        {
            get => m_Editor;
            set => m_Editor = value;
        }

        internal bool EditorTests
        {
            get => m_EditorTests;
            set => m_EditorTests = value;
        }
    }
}
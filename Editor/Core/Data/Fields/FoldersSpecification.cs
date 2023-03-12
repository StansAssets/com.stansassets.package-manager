using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class FoldersSpecification
    {
        [SerializeField] bool m_Runtime;
        [SerializeField] bool m_RuntimeTests;
        [SerializeField] bool m_Editor;
        [SerializeField] bool m_EditorTests;

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

        internal FoldersSpecification Copy()
        {
            return new FoldersSpecification
            {
                Runtime = Runtime,
                RuntimeTests = RuntimeTests,
                Editor = Editor,
                EditorTests = EditorTests,
            };
        }
    }
}
#if UNITY_2019_4_OR_NEWER
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class HeaderColumn
    {
        internal VisualElement Element { get; }

        /// <summary>
        /// Header column of <see cref="ListViewMich"/>
        /// </summary>
        /// <param name="text">text value</param>
        /// <param name="space">Space in %..100%</param>
        internal HeaderColumn(string text, float space = -1f)
        {
            if (space > 100f)
            {
                space = 100f;
            }

            Element = new Label(text)
            {
                style =
                {
                    flexShrink = 1f,
                    flexGrow = 1f,
                    width = new StyleLength(new Length(space, LengthUnit.Percent)),
                }
            };
        }
    }
}
#endif
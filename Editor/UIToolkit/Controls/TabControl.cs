#if UNITY_2019_4_OR_NEWER
using System;
using System.Collections.Generic;
using System.Linq;
using StansAssets.Foundation.UIElements;
using UnityEngine.UIElements;

namespace StansAssets.PackageManager.Editor
{
    class TabControl
    {
        readonly Dictionary<string, (string label, VisualElement element)> m_Tabs =
            new Dictionary<string, (string label, VisualElement element)>();

        readonly ButtonStrip m_TabsButtons;
        readonly ScrollView m_TabsContainer;

        internal TabControl(VisualElement root)
        {
            m_TabsContainer = root.Q<ScrollView>("tabs-container");

            m_TabsButtons = root.Q<ButtonStrip>();
            m_TabsButtons.CleanUp();
            m_TabsButtons.OnButtonClick += ActivateTab;

            ActivateTab();
        }

        void ActivateTab()
        {
            if (string.IsNullOrEmpty(m_TabsButtons.Value))
            {
                return;
            }

            foreach (var tab in m_Tabs)
            {
                tab.Value.element.RemoveFromHierarchy();
            }

            if (!m_Tabs.Any())
            {
                return;
            }

            var (_, element) = m_Tabs.First(i => i.Value.label.Equals(m_TabsButtons.Value)).Value;
            m_TabsContainer.Add(element);
        }

        /// <summary>
        ///     Add tab to the window top bar.
        /// </summary>
        /// <param name="name">Tab name</param>
        /// <param name="label">Tab label.</param>
        /// <param name="content">Tab content.</param>
        /// <exception cref="ArgumentException">Will throw tab with the same label was already added.</exception>
        internal void AddTab(string name, string label, VisualElement content)
        {
            if (!m_Tabs.ContainsKey(label))
            {
                m_TabsButtons.AddChoice(label, label);
                m_Tabs.Add(name, (label, content));
                content.viewDataKey = label;
            }
            else
            {
                throw new ArgumentException($"Tab '{label}'[{name}] already added", nameof(label));
            }
        }

        /// <summary>
        ///     Activate tab by name
        /// </summary>
        /// <param name="name">Early specified tab name</param>
        internal void ActivateTab(string name)
        {
            if (!m_Tabs.ContainsKey(name))
            {
                return;
            }

            var tab = m_Tabs[name];
            m_TabsButtons.SetValue(tab.label);
        }
    }
}
#endif
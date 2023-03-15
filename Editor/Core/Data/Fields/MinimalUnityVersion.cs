using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class MinimalUnityVersion
    {
        [SerializeField] string m_Unity = "2019.4";
        [SerializeField] string m_UnityRelease = "40f1";

        /// <summary>
        /// Indicates the lowest Unity version the package is compatible with. If omitted,
        /// the Package Manager considers the package compatible with all Unity versions.
        ///
        /// The expected format is “<MAJOR>.<MINOR>” (for example, 2018.3). To point to a specific patch,
        /// use the unityRelease property as well.
        ///
        /// Note: A package that isn’t compatible with Unity doesn't appear in the Package Manager window.
        /// </summary>
        internal string Unity
        {
            get => m_Unity;
            set => m_Unity = value;
        }

        /// <summary>
        /// Part of a Unity version indicating the specific release of Unity that the package is compatible with.
        /// You can use this property when an updated package requires changes made during
        /// the Unity alpha/beta development cycle. This might be the case if the package needs newly introduced APIs,
        /// or uses existing APIs that have changed in a non-backward-compatible way without API Updater rules.
        ///
        /// The expected format is “<UPDATE><RELEASE>” (for example, 0b4).
        ///
        /// Note: If you omit the recommended unity property, this property has no effect.
        /// </summary>
        internal string UnityRelease
        {
            get => m_UnityRelease;
            set => m_UnityRelease = value;
        }
    }
}
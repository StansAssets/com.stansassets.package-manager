using System;
using UnityEngine;

namespace StansAssets.PackageManager
{
    [Serializable]
    class MinimalUnityVersion
    {
        [SerializeField] string m_Unity = "2019.4";

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
    }
}
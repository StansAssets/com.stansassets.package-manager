using System;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace StansAssets.PackageManager
{
    [Serializable]
    class ManagerAssetItem
    {
        [SerializeField] PackageBindType m_PackageBindType;
        [SerializeField] PackageInfo m_PackageJson;
        [SerializeField] PackageAssetState m_PackageState;

        internal PackageBindType PackageBindType
        {
            get => m_PackageBindType;
            set => m_PackageBindType = value;
        }

        internal PackageInfo PackageJson
        {
            get => m_PackageJson;
            set => m_PackageJson = value;
        }

        internal PackageAssetState PackageState
        {
            get => m_PackageState;
            set => m_PackageState = value;
        }

        public Texture StatusIcon
        {
            get
            {
                var pro = EditorGUIUtility.isProSkin ? "d_" : "";
                string icon;

                switch (PackageState)
                {
                    case PackageAssetState.Enable:
                        icon = "greenLight";

                        break;
                    case PackageAssetState.Disable:
                        icon = "orangeLight";

                        break;
                    case PackageAssetState.NotFound:
                    default:
                        icon = "redLight";

                        break;
                }

                return EditorGUIUtility.IconContent($"{pro}{icon}").image;
            }
        }

        internal void Disable(bool refreshEditor)
        {
            switch (PackageState)
            {
                case PackageAssetState.Disable:
                    return;
                case PackageAssetState.NotFound:
                    throw new ArgumentNullException($"Unable to disable package. Fix the error first.");
            }

            PackageBuilder.RemoveFromProjectDependencies(this);
            PackageState = PackageAssetState.Disable;

            if (refreshEditor)
            {
                EditorApplication.ExecuteMenuItem("Assets/Refresh");
            }
        }
        
        internal void SetPackageState(PackageAssetState newState, bool refreshEditor)
        {
            if (newState == PackageState)
            {
                return;
            }

            if (newState == PackageAssetState.NotFound)
            {
                throw new ArgumentNullException($"Unable to enable package. Fix the error first.");
            }


            switch (newState)
            {
                case PackageAssetState.Enable:
                    PackageBuilder.AddToProjectDependencies(this);
                    break;
                case PackageAssetState.Disable:
                    PackageBuilder.RemoveFromProjectDependencies(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
            
            PackageState = newState;

            if (refreshEditor)
            {
                EditorApplication.ExecuteMenuItem("Assets/Refresh");
            }
        }
    }
}
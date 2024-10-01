using System.Management.Instrumentation;
using UnityEngine;

namespace StansAssets.PackageManager
{
    /// <summary>
    /// Main endpoint for Package manager APIs
    /// </summary>
    public static class PackageManagerAPI
    {
        /// <summary>
        /// Package Name to disable
        /// </summary>
        /// <param name="packageName"></param>
        public static void RestoreProjectDependency(string packageName)
        {
            foreach (var assetItem in PackManagerAssetSettings.Instance.PackagesList)
            {
               Debug.Log(assetItem.PackageJson.name);
               Debug.Log(assetItem.PackageJson.displayName);
            }
        }

        static void SetPackageState(string packageName, PackageAssetState newState)
        {
            var packageAsset = GetAssetInfo(packageName);
            packageAsset.SetPackageState(newState, false);
        }

        static ManagerAssetItem GetAssetInfo(string packageName)
        {
            foreach (var assetItem in PackManagerAssetSettings.Instance.PackagesList)
            {
                if (assetItem.PackageJson.name.Equals(packageName))
                {
                    return assetItem;
                }
            }
            
            throw new InstanceNotFoundException("Package asset not found. " +
                "Please open the Package Manager settings inside the Projects settings window and check if requested package is listed there.");
        }
    }
}

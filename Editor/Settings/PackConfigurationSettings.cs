using System.Collections.Generic;
using System.Linq;
using StansAssets.Plugins;
using UnityEngine;

namespace StansAssets.PackageManager
{
    class PackConfigurationSettings : PackageScriptableSettingsSingleton<PackConfigurationSettings>
    {
        [SerializeField] int m_ActiveConfigurationIndex;
        [SerializeField] List<PackConfiguration> m_Configurations = new List<PackConfiguration>();

        public override string PackageName => PackageManagerConfig.PackageName;
        protected override bool IsEditorOnly => true;

        internal List<PackConfiguration> Configurations => m_Configurations;

        internal int ActiveConfigurationIndex
        {
            get => m_ActiveConfigurationIndex;
            set => m_ActiveConfigurationIndex = value;
        }

        internal PackConfiguration ActiveConfiguration
        {
            get
            {
                if (ActiveConfigurationIndex < 0)
                {
                    ActiveConfigurationIndex = 0;
                }

                if (Configurations.Count > ActiveConfigurationIndex)
                {
                    return Configurations[ActiveConfigurationIndex];
                }

                if (!Configurations.Any())
                {
                    Configurations.Add(new PackConfiguration());
                }

                ActiveConfigurationIndex = 0;
                return Configurations.First();
            }
        }
    }
}
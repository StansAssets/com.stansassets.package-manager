namespace StansAssets.PackageManager
{
    /// <summary>
    /// Package details. Used to create a new package 
    /// </summary>
    class PackageData
    {
        internal string Name { get; set; }
        internal string DisplayName { get; set; }
        internal PackConfiguration Configuration { get; }

        internal PackageData(PackConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
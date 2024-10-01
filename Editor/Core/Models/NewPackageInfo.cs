using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;

namespace StansAssets.PackageManager
{
    class NewPackageInfo
    {
        internal string AssemblyName { get; set; }
        
        internal PackConfiguration Configuration { get; }

        internal PackageJson Package { get; } = new PackageJson();

        internal FoldersSpecification Folders => Configuration.Folders.Copy();

        internal AssemblyDefinitionInfo EditorAssemblyDefinition => FetchEditorAssemblyDefinitionInfo();
        internal AssemblyDefinitionInfo EditorTestsAssemblyDefinition => FetchEditorTestsAssemblyDefinitionInfo();

        internal AssemblyDefinitionInfo RuntimeAssemblyDefinition => FetchRuntimeAssemblyDefinitionInfo();
        internal AssemblyDefinitionInfo RuntimeTestsAssemblyDefinition => FetchRuntimeTestsAssemblyDefinitionInfo();

        internal NewPackageInfo(PackConfiguration configuration)
        {
            Configuration = configuration;
            Package.DisplayName = configuration.NamingConvention.DisplayPrefix;
            Package.Name = configuration.NamingConvention.NamePrefix.ToLower();
            AssemblyName = configuration.NamingConvention.NamePrefix;
            Package.Unity = configuration.UnityVersion.Unity;
        }

        AssemblyDefinitionInfo FetchEditorAssemblyDefinitionInfo()
        {
            if (!Configuration.Folders.Editor)
            {
                return null;
            }

            var name = NameConventionBuilder.BuildAssemblyName(AssemblyName, Configuration.NamingConvention);
            var info = FetchAssemblyDefinitionInfo(
                $"{name}.Editor",
                Configuration.General,
                Configuration.AssemblyDefinitions.EditorAssemblies);

            var platforms = info.IncludePlatforms.ToList();
            platforms.Add("Editor");

            info.IncludePlatforms = platforms.ToArray();

            return info;
        }

        AssemblyDefinitionInfo FetchEditorTestsAssemblyDefinitionInfo()
        {
            if (!Configuration.Folders.Editor | !Configuration.Folders.EditorTests)
            {
                return null;
            }

            var name = NameConventionBuilder.BuildAssemblyName(AssemblyName, Configuration.NamingConvention);
            var info = FetchAssemblyDefinitionInfo(
                $"{name}.EditorTests",
                Configuration.General,
                Configuration.AssemblyDefinitions.EditorTestsAssemblies);

            var platforms = info.IncludePlatforms.ToList();
            platforms.Add("Editor");

            info.IncludePlatforms = platforms.ToArray();

            return info;
        }

        AssemblyDefinitionInfo FetchRuntimeAssemblyDefinitionInfo()
        {
            if (!Configuration.Folders.Runtime)
            {
                return null;
            }

            var name = NameConventionBuilder.BuildAssemblyName(AssemblyName, Configuration.NamingConvention);
            var info = FetchAssemblyDefinitionInfo(
                $"{name}.Runtime",
                Configuration.General,
                Configuration.AssemblyDefinitions.RuntimeAssemblies);

            return info;
        }

        AssemblyDefinitionInfo FetchRuntimeTestsAssemblyDefinitionInfo()
        {
            if (!Configuration.Folders.Runtime | !Configuration.Folders.RuntimeTests)
            {
                return null;
            }

            var name = NameConventionBuilder.BuildAssemblyName(AssemblyName, Configuration.NamingConvention);
            var info = FetchAssemblyDefinitionInfo(
                $"{name}.RuntimeTests",
                Configuration.General,
                Configuration.AssemblyDefinitions.RuntimeTestsAssemblies);

            return info;
        }

        static AssemblyDefinitionInfo FetchAssemblyDefinitionInfo(
            string name,
            GeneralSpecification generalSpecification,
            AssemblyDefinitions assemblyDefinitions)
        {
            var info = new AssemblyDefinitionInfo()
            {
                Name = name,
                RootNamespace = name,
                References = assemblyDefinitions.AssemblyDefinitionAssets.Select(r => r.name).ToArray(),
                // IncludePlatforms =
                // ExcludePlatforms = 
                AllowUnsafeCode = generalSpecification.AllowUnsafeCode,
                OverrideReferences = generalSpecification.OverrideReferences,
                PrecompiledReferences = assemblyDefinitions.PrecompiledReferences.ToArray(),
                AutoReferenced = generalSpecification.AutoReferenced,
                // DefineConstraints =
                // VersionDefines = 
                NoEngineReferences = generalSpecification.NoEngineReferences
            };

            return info;
        }
    }

    [Serializable]
    class PackageJson
    {
        /// <summary>
        /// A unique identifier that conforms to the Unity Package Manager naming convention,
        /// which uses reverse domain name notation
        /// </summary>
        [JsonProperty("name")]
        internal string Name { get; set; } = "";

        /// <summary>
        /// The package version number (MAJOR.MINOR.PATCH)
        /// For example, “3.2.1” indicates that this is the 3rd major release,
        /// the 2nd minor release, and the first patch.
        /// </summary>
        [JsonProperty("version")]
        internal string Version { get; set; } = "0.0.1";

        /// <summary>
        /// A brief description of the package.
        /// This is the text that appears in the details view of the Package Manager window.
        /// This field supports UTF–8 character codes. This means that you can use special formatting character codes,
        /// such as line breaks (\n) and bullets (\u25AA).
        /// </summary>
        [JsonProperty("description")]
        internal string Description { get; set; } = "";

        /// <summary>
        /// A user-friendly name to appear in the Unity Editor
        /// (for example, in the Project Browser, the Package Manager window, etc.).
        /// 
        /// For example, Unity Timeline, ProBuilder, In App Purchasing.
        /// </summary>
        [JsonProperty("displayName")]
        internal string DisplayName { get; set; } = "";

        /// <summary>
        /// Indicates the lowest Unity version the package is compatible with. If omitted,
        /// the Package Manager considers the package compatible with all Unity versions.
        ///
        /// The expected format is “<MAJOR>.<MINOR>” (for example, 2018.3). To point to a specific patch,
        /// use the unityRelease property as well.
        ///
        /// Note: A package that isn’t compatible with Unity doesn't appear in the Package Manager window.
        /// </summary>
        [JsonProperty("unity")]
        internal string Unity { get; set; } = "";

        /// <summary>
        /// The author of the package.
        ///
        /// This property contains one required field, name, and two optional fields, email and url.
        /// </summary>
        [JsonProperty("author")]
        internal AuthorInfo Author { get; set; } = new AuthorInfo();

        /// <summary>
        /// Custom location for this package’s changelog specified as a URL.
        /// </summary>
        [JsonProperty("changelogUrl")]
        internal string ChangelogUrl { get; set; } = "";

        /// <summary>
        /// A map of package dependencies. Keys are package names, and values are specific versions.
        /// They indicate other packages that this package depends on.
        ///
        /// Note: The Package Manager doesn't support range syntax, only SemVer versions.
        /// </summary>
        [JsonProperty("dependencies")]
        internal Dictionary<string, string> Dependencies { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Custom location for this package’s documentation specified as a URL.
        /// </summary>
        [JsonProperty("documentationUrl")]
        internal string DocumentationUrl { get; set; } = "";

        /// <summary>
        /// Package Manager hides most packages automatically (the implicit value is “true”),
        /// but you can set this property to “false” to make sure that your package and its assets are always visible.
        /// </summary>
        [JsonProperty("hideInEditor")]
        internal bool HideInEditor { get; set; } = true;

        /// <summary>
        /// An array of keywords used by the Package Manager search APIs. This helps users find relevant packages.
        /// </summary>
        [JsonProperty("keywords")]
        internal string[] Keywords { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Identifier for an OSS license using the SPDX identifier format, or a string such as “See LICENSE.md file”.
        ///
        /// Note: If you omit this property in your package manifest, your package must contain a LICENSE.md file.
        /// </summary>
        [JsonProperty("license")]
        internal string License { get; set; } = "MIT";

        /// <summary>
        /// Custom location for this package’s license information specified as a URL.
        /// </summary>
        [JsonProperty("licensesUrl")]
        internal string LicensesUrl { get; set; } = "https://mit-license.org/";
    }

    [Serializable]
    class AuthorInfo
    {
        [JsonProperty("name")] internal string Name { get; set; } = "";
        [JsonProperty("email")] internal string Email { get; set; } = "";
        [JsonProperty("url")] internal string Url { get; set; } = "";
    }

    [Serializable]
    class AssemblyDefinitionInfo
    {
        [JsonProperty("name")] internal string Name { get; set; }

        [JsonProperty("rootNamespace")] internal string RootNamespace { get; set; }

        [JsonProperty("references")] internal string[] References { get; set; } = Array.Empty<string>();

        [JsonProperty("includePlatforms")] internal string[] IncludePlatforms { get; set; } = Array.Empty<string>();

        [JsonProperty("excludePlatforms")] internal string[] ExcludePlatforms { get; set; } = Array.Empty<string>();

        [JsonProperty("allowUnsafeCode")] internal bool AllowUnsafeCode { get; set; }

        [JsonProperty("overrideReferences")] internal bool OverrideReferences { get; set; }

        [JsonProperty("precompiledReferences")]
        internal string[] PrecompiledReferences { get; set; } = Array.Empty<string>();

        [JsonProperty("autoReferenced")] internal bool AutoReferenced { get; set; }

        [JsonProperty("defineConstraints")] internal string[] DefineConstraints { get; set; } = Array.Empty<string>();

        [JsonProperty("versionDefines")]
        internal VersionDefine[] VersionDefines { get; set; } = Array.Empty<VersionDefine>();

        [JsonProperty("noEngineReferences")] internal bool NoEngineReferences { get; set; }
    }

    /// <summary>
    /// Example:
    ///     "name": "com.unity.render-pipelines.high-definition",
    ///     "expression": "7.1.0",
    ///     "define": "HDRP_7_1_0_OR_NEWER"
    /// </summary>
    [Serializable]
    class VersionDefine
    {
        [JsonProperty("name")] internal string Name { get; set; } = "";
        [JsonProperty("expression")] internal string Expression { get; set; } = "";
        [JsonProperty("define")] internal string Define { get; set; } = "";
    }
}
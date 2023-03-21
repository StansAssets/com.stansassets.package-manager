using System;
using System.IO;
using System.Linq;
using System.Text;

using StansAssets.Foundation.Editor;

using Unity.Plastic.Newtonsoft.Json;

using UnityEditor;

using UnityEngine;

namespace StansAssets.PackageManager
{
    static class PackageBuilder
    {
        internal static readonly string LocalPackagesCachePath
            = $"{Application.temporaryCachePath}/{PackageManagerConfig.PackageName}/cache";

        internal static void BuildPackage(NewPackageInfo packageInfo, out bool successful)
        {
            Verification(packageInfo);

            EditorApplication.LockReloadAssemblies();

            try
            {
                CreateFolders(packageInfo);
                CreateAssemblyDefinitions(packageInfo);
                CreateAssembliesInfo(packageInfo);
                CreatePackageJson(packageInfo);
            }
            catch (Exception e)
            {
                successful = false;
                Console.WriteLine(e);

                throw;
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }

            successful = true;
            Debug.Log($"Package created: {packageInfo.Package.Name}");

            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }

        static void Verification(NewPackageInfo packageInfo)
        {
            var package = packageInfo.Package;

            if (string.IsNullOrEmpty(package.Name))
            {
                throw new ArgumentException("Package name is empty!");
            }

            if (Directory.Exists(package.Name))
            {
                throw new ArgumentException($"Directory with the same name already exists!\n[{package.Name}]");
            }
        }

        static void CreateFolders(NewPackageInfo packageInfo)
        {
            var path = $"Packages/{packageInfo.Package.Name}";

            Directory.CreateDirectory(path);

            var folders = packageInfo.Folders;

            if (folders.Editor)
            {
                Directory.CreateDirectory($"{path}/Editor");

                if (folders.EditorTests)
                {
                    Directory.CreateDirectory($"{path}/Tests/Editor");
                }
            }

            if (folders.Runtime)
            {
                Directory.CreateDirectory($"{path}/Runtime");

                if (folders.RuntimeTests)
                {
                    Directory.CreateDirectory($"{path}/Tests/Runtime");
                }
            }
        }

        static void CreateAssemblyDefinitions(NewPackageInfo packageInfo)
        {
            var path = $"Packages/{packageInfo.Package.Name}";

            var editor = packageInfo.EditorAssemblyDefinition;
            if (editor != null)
            {
                WriteAssembly($"{path}/Editor", editor.Name, editor);

                var editorTests = packageInfo.EditorTestsAssemblyDefinition;
                if (editorTests != null)
                {
                    WriteAssembly($"{path}/Tests/Editor", editorTests.Name, editorTests);
                }
            }

            var runtime = packageInfo.RuntimeAssemblyDefinition;
            if (runtime != null)
            {
                WriteAssembly($"{path}/Runtime", runtime.Name, runtime);

                var runtimeTests = packageInfo.RuntimeTestsAssemblyDefinition;
                if (runtimeTests != null)
                {
                    WriteAssembly($"{path}/Tests/Runtime", runtimeTests.Name, runtimeTests);
                }
            }
        }

        static void CreateAssembliesInfo(NewPackageInfo packageInfo)
        {
            var folders = packageInfo.Configuration.Folders;
            var path = $"Packages/{packageInfo.Package.Name}";

            if (folders.Editor)
            {
                var assemblies = packageInfo.Configuration.AssemblyDefinitions.EditorAssemblies
                    .InternalVisibleToAssemblyDefinitionAssets.Select(a => a.name).ToList();

                if (folders.EditorTests)
                {
                    // Add internal visible to tests assembly
                    var testsName = packageInfo.EditorTestsAssemblyDefinition.Name;
                    assemblies.Add(testsName);

                    // Tests
                    var testsAssemblies = packageInfo.Configuration.AssemblyDefinitions.EditorTestsAssemblies
                        .InternalVisibleToAssemblyDefinitionAssets.Select(a => a.name).ToList();

                    if (testsAssemblies.Any())
                    {
                        AssemblyBuilderUtils.BuildAssemblyInfo($"{path}/Tests/Editor", testsAssemblies);
                    }
                }

                AssemblyBuilderUtils.BuildAssemblyInfo($"{path}/Editor", assemblies);
            }

            if (folders.Runtime)
            {
                var assemblies = packageInfo.Configuration.AssemblyDefinitions.RuntimeAssemblies
                    .InternalVisibleToAssemblyDefinitionAssets.Select(a => a.name).ToList();

                if (folders.RuntimeTests)
                {
                    // Add internal visible to tests assembly
                    var testsName = packageInfo.RuntimeTestsAssemblyDefinition.Name;
                    assemblies.Add(testsName);

                    // Tests
                    var testsAssemblies = packageInfo.Configuration.AssemblyDefinitions.RuntimeTestsAssemblies
                        .InternalVisibleToAssemblyDefinitionAssets.Select(a => a.name).ToList();

                    if (testsAssemblies.Any())
                    {
                        AssemblyBuilderUtils.BuildAssemblyInfo($"{path}/Tests/Runtime", testsAssemblies);
                    }
                }

                AssemblyBuilderUtils.BuildAssemblyInfo($"{path}/Runtime", assemblies);
            }
        }

        static void WriteAssembly(string directory, string name, object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = $"{directory}/{name}.asmdef";

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        static void CreatePackageJson(NewPackageInfo packageInfo)
        {
            var path = $"Packages/{packageInfo.Package.Name}";
            var json = JsonConvert.SerializeObject(packageInfo.Package);

            File.WriteAllText($"{path}/package.json", json);
        }

        internal static PackageJson UnpackPackage(string path)
        {
            var data = File.ReadAllText(path);
            var package = JsonConvert.DeserializeObject<PackageJson>(data);

            return package;
        }

        internal static void AddToProjectDependencies(ManagerAssetItem assetItem)
        {
            switch (assetItem.PackageBindType)
            {
                case PackageBindType.Manifest:
                case PackageBindType.GitUrl:
                {
                    var manifest = new Manifest();
                    manifest.Fetch();
                    manifest.SetDependency(assetItem.PackageJson.name, assetItem.PackageJson.version);
                    manifest.ApplyChanges();

                    break;
                }
                case PackageBindType.LocalPackages:
                {
                    if (!Directory.Exists(LocalPackagesCachePath))
                    {
                        break;
                    }

                    FileUtil.MoveFileOrDirectory($"{LocalPackagesCachePath}/{assetItem.PackageJson.name}",
                        assetItem.PackageJson.resolvedPath);

                    break;
                }
                case PackageBindType.LocalFile:
                {
                    var manifest = new Manifest();
                    manifest.Fetch();
                    manifest.SetDependency(assetItem.PackageJson.name, $"file:{assetItem.PackageJson.resolvedPath}");
                    manifest.ApplyChanges();

                    break;
                }
            }

            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }

        internal static void RemoveFromProjectDependencies(ManagerAssetItem assetItem)
        {
            switch (assetItem.PackageBindType)
            {
                case PackageBindType.Manifest:
                case PackageBindType.GitUrl:
                {
                    var manifest = new Manifest();
                    manifest.Fetch();
                    manifest.RemoveDependency(assetItem.PackageJson.name);
                    manifest.ApplyChanges();

                    break;
                }
                case PackageBindType.LocalPackages:
                {
                    if (!Directory.Exists(LocalPackagesCachePath))
                    {
                        Directory.CreateDirectory(LocalPackagesCachePath);
                    }

                    FileUtil.MoveFileOrDirectory(assetItem.PackageJson.resolvedPath,
                        $"{LocalPackagesCachePath}/{assetItem.PackageJson.name}");

                    break;
                }
                case PackageBindType.LocalFile:
                {
                    var manifest = new Manifest();
                    manifest.Fetch();
                    manifest.RemoveDependency(assetItem.PackageJson.name);
                    manifest.ApplyChanges();

                    break;
                }
            }

            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
    }
}
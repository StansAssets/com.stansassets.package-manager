using System;
using System.IO;
using System.Text;
using StansAssets.Foundation.Editor;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;

namespace StansAssets.PackageManager
{
    static class PackageBuilder
    {
        internal static void BuildPackage(NewPackageInfo packageInfo)
        {
            Verification(packageInfo);

            EditorApplication.LockReloadAssemblies();

            try
            {
                CreateFolders(packageInfo);
                CreateAssemblyDefinitions(packageInfo);
                CreatePackageJson(packageInfo);
                AddToProjectDependencies(packageInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
                EditorApplication.update();
            }

            EditorApplication.update();
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
            var path = packageInfo.Package.Name;

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
            var path = packageInfo.Package.Name;
            
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

        static void WriteAssembly(string directory, string name, object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = $"{directory}/{name}.asmdef";

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        static void CreatePackageJson(NewPackageInfo packageInfo)
        {
            var path = packageInfo.Package.Name;
            var json = JsonConvert.SerializeObject(packageInfo.Package, Formatting.Indented);

            File.WriteAllText($"{path}/package.json", json, Encoding.UTF8);
        }

        static void AddToProjectDependencies(NewPackageInfo packageInfo)
        {
            var manifest = new Manifest();
            manifest.Fetch();
            manifest.SetDependency(packageInfo.Package.Name, $"file:../{packageInfo.Package.Name}");
            manifest.ApplyChanges();
        }
    }
}
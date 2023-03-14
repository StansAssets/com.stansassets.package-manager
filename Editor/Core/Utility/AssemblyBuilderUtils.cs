using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder;

namespace StansAssets.PackageManager
{
    static class AssemblyBuilderUtils
    {
        internal static Type GenerateEnumType(string enumName, string[] enumValues)
        {
            var assemblyName = new AssemblyName("DynamicEnums");

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicEnumsModule");
            var enumBuilder = moduleBuilder.DefineEnum(enumName, TypeAttributes.Public, typeof(int));

            foreach (var value in enumValues)
            {
                enumBuilder.DefineLiteral(value, Array.IndexOf(enumValues, value));
            }

            return enumBuilder.CreateType();
        }

        internal static void BuildAssemblyInfo(string path, IEnumerable<string> internalVisibleAssemblies)
        {
            using (var writer = new StreamWriter($"{path}/AssemblyInfo.cs"))
            {
                writer.WriteLine("using System.Runtime.CompilerServices;");
                writer.WriteLine("");

                foreach (var assembly in internalVisibleAssemblies)
                {
                    writer.WriteLine("[assembly: InternalsVisibleTo(\"{0}\")]", assembly);
                }
            }
        }
    }
}
using System;
using System.Reflection;
using System.Reflection.Emit;
using AssemblyBuilder = System.Reflection.Emit.AssemblyBuilder;

namespace StansAssets.PackageManager.Editor
{
    class AssemblyBuilderUtils
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
    }
}
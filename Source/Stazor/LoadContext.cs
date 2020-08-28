using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Stazor
{
    public sealed class LoadContext : AssemblyLoadContext
    {
        readonly AssemblyDependencyResolver _resolver;

        public LoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
        }
    }
}
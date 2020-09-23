using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Stazor
{
    /// <summary>
    /// Custom AssemblyLoadContext implementation.
    /// </summary>
    sealed class LoadContext : AssemblyLoadContext
    {
        readonly AssemblyDependencyResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadContext"/> class.
        /// </summary>
        /// <param name="pluginPath">Path to the assembly's DLL.</param>
        public LoadContext(string pluginPath)
            => _resolver = new AssemblyDependencyResolver(pluginPath);

        /// <inheritdoc/>
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        /// <inheritdoc/>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
        }
    }
}
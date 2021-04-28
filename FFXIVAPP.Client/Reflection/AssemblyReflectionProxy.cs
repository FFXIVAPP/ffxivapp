// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyReflectionProxy.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AssemblyReflectionProxy.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Reflection {
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal class AssemblyReflectionProxy : MarshalByRefObject {
        private string _assemblyPath;

        public void LoadAssembly(string assemblyPath) {
            try {
                this._assemblyPath = assemblyPath;
                Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            catch (FileNotFoundException) {
                // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain.
            }
        }

        public TResult Reflect<TResult>(Func<Assembly, TResult> func) {
            DirectoryInfo directory = new FileInfo(this._assemblyPath).Directory;
            ResolveEventHandler resolveEventHandler = (s, e) => {
                return this.OnReflectionOnlyResolve(e, directory);
            };

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

            Assembly assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(a => a.Location.CompareTo(this._assemblyPath) == 0);

            TResult result = func(assembly);

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;

            return result;
        }

        private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory) {
            Assembly loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(asm => string.Equals(asm.FullName, args.Name, Constants.InvariantComparer));
            if (loadedAssembly != null) {
                return loadedAssembly;
            }

            var assemblyName = new AssemblyName(args.Name);
            var dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");

            if (File.Exists(dependentAssemblyFilename)) {
                return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
            }

            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyReflectionManager.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   AssemblyReflectionManager.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Reflection {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Policy;

    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;

    using NLog;

    internal class AssemblyReflectionManager : IDisposable {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, AppDomain> _loadedAssemblies = new Dictionary<string, AppDomain>();

        private Dictionary<string, AppDomain> _mapDomains = new Dictionary<string, AppDomain>();

        private Dictionary<string, AssemblyReflectionProxy> _proxies = new Dictionary<string, AssemblyReflectionProxy>();

        ~AssemblyReflectionManager() {
            this.Dispose(false);
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool LoadAssembly(string assemblyPath, string domainName) {
            // if the assembly file does not exist then fail
            if (!File.Exists(assemblyPath)) {
                return false;
            }

            // if the assembly was already loaded then fail
            if (this._loadedAssemblies.ContainsKey(assemblyPath)) {
                return false;
            }

            // check if the appdomain exists, and if not create a new one
            AppDomain appDomain = null;
            if (this._mapDomains.ContainsKey(domainName)) {
                appDomain = this._mapDomains[domainName];
            }
            else {
                appDomain = this.CreateChildDomain(AppDomain.CurrentDomain, domainName);
                this._mapDomains[domainName] = appDomain;
            }

            // load the assembly in the specified app domain
            try {
                Type proxyType = typeof(AssemblyReflectionProxy);
                if (proxyType.Assembly != null) {
                    var proxy = (AssemblyReflectionProxy) appDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName).Unwrap();
                    proxy.LoadAssembly(assemblyPath);
                    this._loadedAssemblies[assemblyPath] = appDomain;
                    this._proxies[assemblyPath] = proxy;
                    return true;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            return false;
        }

        public TResult Reflect<TResult>(string assemblyPath, Func<Assembly, TResult> func) {
            // check if the assembly is found in the internal dictionaries
            if (this._loadedAssemblies.ContainsKey(assemblyPath) && this._proxies.ContainsKey(assemblyPath)) {
                return this._proxies[assemblyPath].Reflect(func);
            }

            return default;
        }

        public bool UnloadAssembly(string assemblyPath) {
            if (!File.Exists(assemblyPath)) {
                return false;
            }

            // check if the assembly is found in the internal dictionaries
            if (this._loadedAssemblies.ContainsKey(assemblyPath) && this._proxies.ContainsKey(assemblyPath)) {
                // check if there are more assemblies loaded in the same app domain; in this case fail
                AppDomain appDomain = this._loadedAssemblies[assemblyPath];
                var count = this._loadedAssemblies.Values.Count(a => a == appDomain);
                if (count != 1) {
                    return false;
                }

                try {
                    // remove the appdomain from the dictionary and unload it from the process
                    this._mapDomains.Remove(appDomain.FriendlyName);
                    AppDomain.Unload(appDomain);

                    // remove the assembly from the dictionaries
                    this._loadedAssemblies.Remove(assemblyPath);
                    this._proxies.Remove(assemblyPath);
                    return true;
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            return false;
        }

        public bool UnloadDomain(string domainName) {
            // check the appdomain name is valid
            if (string.IsNullOrEmpty(domainName)) {
                return false;
            }

            // check we have an instance of the domain
            if (this._mapDomains.ContainsKey(domainName)) {
                try {
                    AppDomain appDomain = this._mapDomains[domainName];

                    // check the assemblies that are loaded in this app domain
                    List<string> assemblies = (from kvp in this._loadedAssemblies
                                               where kvp.Value == appDomain
                                               select kvp.Key).ToList();

                    // remove these assemblies from the internal dictionaries
                    foreach (var assemblyName in assemblies) {
                        this._loadedAssemblies.Remove(assemblyName);
                        this._proxies.Remove(assemblyName);
                    }

                    // remove the appdomain from the dictionary
                    this._mapDomains.Remove(domainName);

                    // unload the appdomain
                    AppDomain.Unload(appDomain);
                    return true;
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            return false;
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposing) {
                return;
            }

            foreach (AppDomain appDomain in this._mapDomains.Values) {
                AppDomain.Unload(appDomain);
            }

            this._loadedAssemblies.Clear();
            this._proxies.Clear();
            this._mapDomains.Clear();
        }

        private AppDomain CreateChildDomain(AppDomain parentDomain, string domainName) {
            var evidence = new Evidence(parentDomain.Evidence);
            AppDomainSetup setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain(domainName, evidence, setup);
        }
    }
}
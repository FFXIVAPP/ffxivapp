// FFXIVAPP.Client
// AssemblyReflectionManager.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Reflection
{
    [DoNotObfuscate]
    public class AssemblyReflectionManager : IDisposable
    {
        private Dictionary<string, AppDomain> _loadedAssemblies = new Dictionary<string, AppDomain>();
        private Dictionary<string, AppDomain> _mapDomains = new Dictionary<string, AppDomain>();
        private Dictionary<string, AssemblyReflectionProxy> _proxies = new Dictionary<string, AssemblyReflectionProxy>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool LoadAssembly(string assemblyPath, string domainName)
        {
            // if the assembly file does not exist then fail
            if (!File.Exists(assemblyPath))
            {
                return false;
            }
            // if the assembly was already loaded then fail
            if (_loadedAssemblies.ContainsKey(assemblyPath))
            {
                return false;
            }
            // check if the appdomain exists, and if not create a new one
            AppDomain appDomain = null;
            if (_mapDomains.ContainsKey(domainName))
            {
                appDomain = _mapDomains[domainName];
            }
            else
            {
                appDomain = CreateChildDomain(AppDomain.CurrentDomain, domainName);
                _mapDomains[domainName] = appDomain;
            }
            // load the assembly in the specified app domain
            try
            {
                var proxyType = typeof (AssemblyReflectionProxy);
                if (proxyType.Assembly != null)
                {
                    var proxy = (AssemblyReflectionProxy) appDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName)
                                                                   .Unwrap();
                    proxy.LoadAssembly(assemblyPath);
                    _loadedAssemblies[assemblyPath] = appDomain;
                    _proxies[assemblyPath] = proxy;
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool UnloadAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                return false;
            }
            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey(assemblyPath) && _proxies.ContainsKey(assemblyPath))
            {
                // check if there are more assemblies loaded in the same app domain; in this case fail
                var appDomain = _loadedAssemblies[assemblyPath];
                var count = _loadedAssemblies.Values.Count(a => a == appDomain);
                if (count != 1)
                {
                    return false;
                }
                try
                {
                    // remove the appdomain from the dictionary and unload it from the process
                    _mapDomains.Remove(appDomain.FriendlyName);
                    AppDomain.Unload(appDomain);
                    // remove the assembly from the dictionaries
                    _loadedAssemblies.Remove(assemblyPath);
                    _proxies.Remove(assemblyPath);
                    return true;
                }
                catch(Exception ex)
                {
                }
            }
            return false;
        }

        public bool UnloadDomain(string domainName)
        {
            // check the appdomain name is valid
            if (string.IsNullOrEmpty(domainName))
            {
                return false;
            }
            // check we have an instance of the domain
            if (_mapDomains.ContainsKey(domainName))
            {
                try
                {
                    var appDomain = _mapDomains[domainName];
                    // check the assemblies that are loaded in this app domain
                    var assemblies = (from kvp in _loadedAssemblies where kvp.Value == appDomain select kvp.Key).ToList();
                    // remove these assemblies from the internal dictionaries
                    foreach (var assemblyName in assemblies)
                    {
                        _loadedAssemblies.Remove(assemblyName);
                        _proxies.Remove(assemblyName);
                    }
                    // remove the appdomain from the dictionary
                    _mapDomains.Remove(domainName);
                    // unload the appdomain
                    AppDomain.Unload(appDomain);
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }

        public TResult Reflect<TResult>(string assemblyPath, Func<Assembly, TResult> func)
        {
            // check if the assembly is found in the internal dictionaries
            if (_loadedAssemblies.ContainsKey(assemblyPath) && _proxies.ContainsKey(assemblyPath))
            {
                return _proxies[assemblyPath].Reflect(func);
            }
            return default(TResult);
        }

        ~AssemblyReflectionManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            foreach (var appDomain in _mapDomains.Values)
            {
                AppDomain.Unload(appDomain);
            }
            _loadedAssemblies.Clear();
            _proxies.Clear();
            _mapDomains.Clear();
        }

        private AppDomain CreateChildDomain(AppDomain parentDomain, string domainName)
        {
            var evidence = new Evidence(parentDomain.Evidence);
            var setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain(domainName, evidence, setup);
        }
    }
}

// FFXIVAPP.Client
// AssemblyReflectionProxy.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FFXIVAPP.Client.Reflection
{
    public class AssemblyReflectionProxy : MarshalByRefObject
    {
        private string _assemblyPath;

        public void LoadAssembly(String assemblyPath)
        {
            try
            {
                _assemblyPath = assemblyPath;
                Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }
            catch (FileNotFoundException)
            {
                // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain.
            }
        }

        public TResult Reflect<TResult>(Func<Assembly, TResult> func)
        {
            var directory = new FileInfo(_assemblyPath).Directory;
            ResolveEventHandler resolveEventHandler = (s, e) => { return OnReflectionOnlyResolve(e, directory); };

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;

            var assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                                    .FirstOrDefault(a => a.Location.CompareTo(_assemblyPath) == 0);

            var result = func(assembly);

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;

            return result;
        }

        private Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
        {
            var loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                                          .FirstOrDefault(asm => string.Equals(asm.FullName, args.Name, Constants.InvariantComparer));
            if (loadedAssembly != null)
            {
                return loadedAssembly;
            }
            var assemblyName = new AssemblyName(args.Name);
            var dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");

            if (File.Exists(dependentAssemblyFilename))
            {
                return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
            }
            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}

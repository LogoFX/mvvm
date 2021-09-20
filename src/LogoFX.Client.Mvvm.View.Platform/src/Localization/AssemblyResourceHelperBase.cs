using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.View.Localization
{
    /// <summary>
    /// Provides services of loading assemblies into an app domain.
    /// </summary>
    /// <seealso cref="System.MarshalByRefObject" />
    public abstract class AssemblyResourceHelperBase : MarshalByRefObject
    {
        #region Fields

        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        protected readonly AssemblyName _assemblyName;

        private static readonly Dictionary<AssemblyResourceHelperBase, AppDomain> s_domains =
            new Dictionary<AssemblyResourceHelperBase, AppDomain>();
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="AssemblyResourceHelperBase"/> using the provided assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        protected AssemblyResourceHelperBase(AssemblyName assemblyName)
        {
            _assemblyName = assemblyName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the specified type in a new app domain.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        protected static T CreateInNewDomainInternal<T>(params object[] args)
            where T : AssemblyResourceHelperBase
        {
            AppDomain domain = AppDomain.CreateDomain("Temp Assembly Domain");
            Type type = typeof(T);
            string typeAssemblyName = type.Assembly.GetName().Name;
            string typeName = type.FullName;
            T instance = (T) domain.CreateInstanceAndUnwrap(typeAssemblyName,
                                                            typeName,
                                                            false,
                                                            BindingFlags.Default,
                                                            null,
                                                            args,
                                                            null,
                                                            null);

            lock (s_domains)
            {
                s_domains.Add(instance, domain);
            }

            return instance;
        }

        /// <summary>
        /// Destroys the domain.
        /// </summary>
        /// <param name="assemblyResourceLoader">The assembly resource loader.</param>
        public static void DestroyDomain(AssemblyResourceHelperBase assemblyResourceLoader)
        {
            AppDomain domain;
            
            lock (s_domains)
            {
                domain = s_domains[assemblyResourceLoader];
            }

            AppDomain.Unload(domain);
        }

        #endregion
    }
}
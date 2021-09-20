using System;
using System.Reflection;

namespace LogoFX.Client.Mvvm.View.Localization
{
    /// <summary>
    /// Defines meaning for loading resources from an assembly name.
    /// </summary>
    public sealed class AssemblyResourceLoader : AssemblyResourceHelperBase
    {
        #region Fields

        private readonly Assembly _assembly;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="AssemblyResourceLoader"/> using the provided assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        public AssemblyResourceLoader(AssemblyName assemblyName)
            : base(assemblyName)
        {
            try
            {
                _assembly = Assembly.Load(assemblyName);
            }
            catch (Exception)
            {
                _assembly = null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="AssemblyResourceLoader"/> in a new domain.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static AssemblyResourceLoader CreateInNewDomain(AssemblyName assemblyName)
        {
            return CreateInNewDomainInternal<AssemblyResourceLoader>(assemblyName);
        }

        /// <summary>
        /// Extracts the resources.
        /// </summary>
        /// <returns></returns>
        public ResourceSetCollection ExtractResources()
        {
            ResourceSetCollection result = new ResourceSetCollection();

            if (ReferenceEquals(_assembly, null))
            {
                return result;
            }

            result = new WinRes().EnumStringResources(_assembly.Location) ?? // read resources from Win32 DLL.
                     AssemblyResourceUtility.ExtractResources(_assembly);    // read resources from managed assembly.

            return result;
        }

        #endregion
    }
}

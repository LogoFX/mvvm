using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.View.Localization
{ 
    /// <summary>
    /// Represents local assembly collection.
    /// </summary>
    public sealed class LocalAssemblyCollection : Dictionary<AssemblyName, ResourceSetCollection>
    {
        /// <summary>
        /// Gets or sets the local path.
        /// </summary>
        public string Path { get; set; }
    }
}

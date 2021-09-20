using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.View.Localization
{
    /// <summary>
    /// Represents a collection of assemblies.
    /// </summary>
    [Serializable]   
    public sealed class AssemblyCollection : Dictionary<string, LocalAssemblyCollection>
    {
    }
}

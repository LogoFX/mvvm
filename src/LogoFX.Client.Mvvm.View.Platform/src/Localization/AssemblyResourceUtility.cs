using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace LogoFX.Client.Mvvm.View.Localization
{
    /// <summary>
    /// Provides various resource utilities.
    /// </summary>
    public static class AssemblyResourceUtility
    {
        #region Fields

        private const string ExtensionPart = ".resources";

        private const string PathPart = ".Properties.";

        #endregion

        #region Constructors

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Extracts resources from the provided assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ResourceSetCollection ExtractResources(Assembly assembly)
        {
            ResourceSetCollection result = new ResourceSetCollection();

            //search all resource names
            string[] resourceNames = assembly.GetManifestResourceNames()
                .Where(x => x.ToLower().EndsWith(ExtensionPart))
                .ToArray();

            //iterate on resources
            foreach (string resourceName in resourceNames)
            {
                //try gets resource stream by it's name
                Stream stream = assembly.GetManifestResourceStream(resourceName);

                if (ReferenceEquals(stream, null))
                {
                    continue;
                }

                ResourceCollection resource = new ResourceCollection();

                //trying found resource
                try
                {
                    using (ResourceReader rr = new ResourceReader(stream))
                    {
                        foreach (DictionaryEntry entry in rr)
                        {
                            if (entry.Key is string && entry.Value is string)
                            {
                                resource.Add((string)entry.Key, (string)entry.Value);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                if (resource.Count > 0)
                {
                    result.Add(resourceName, resource);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the shortened version of resource name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FullResourceNameToShortOne(string value)
        {
            value = RemoveResourceExtension(value);

            int index = value.IndexOf(PathPart);
            if (index >= 0)
            {
                value = value.Substring(index + PathPart.Length);
            }

            return value;
        }

        /// <summary>
        /// Provides default resource set name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LocalResourceSetNameToDefaultOne(string value)
        {
            string tmp = RemoveResourceExtension(value);

            int index = tmp.LastIndexOf('.');

            if (index > 0)
            {
                string localSign = tmp.Substring(index + 1);
                CultureInfo cultureInfo;

                try
                {
                    cultureInfo = CultureInfo.GetCultureInfoByIetfLanguageTag(localSign);
                }
                catch (Exception)
                {
                    cultureInfo = null;
                }

                if (!ReferenceEquals(cultureInfo, null))
                {
                    value = value.Remove(index, localSign.Length + 1);
                }
            }

            return value;
        }

        /// <summary>
        /// Localizes the default resource set name.
        /// </summary>
        /// <param name="defaultName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string DefaultResourceSetNameToLocalOne(string defaultName, CultureInfo culture)
        {
            string tmp = RemoveResourceExtension(defaultName);
            tmp += "." + culture.IetfLanguageTag;
            tmp += ExtensionPart;

            return tmp;
        }

        /// <summary>
        /// Compares two assemblies' names.
        /// </summary>
        /// <param name="an1"></param>
        /// <param name="an2"></param>
        /// <returns></returns>
        public static bool CompareAssemblyNames(AssemblyName an1, AssemblyName an2)
        {
            bool result = ReferenceEquals(an1, an2);

            if (result)
            {
                return true;
            }

            result = AssemblyName.ReferenceMatchesDefinition(an1, an2);

            if (result)
            {
                result = an1.CultureInfo.Equals(an2.CultureInfo);
            }

            return result;
        }

        /// <summary>
        /// Creates new assembly based on its name and culture.
        /// </summary>
        /// <param name="baseAssemblyName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static AssemblyName CreateNewLocalAssembly(AssemblyName baseAssemblyName, CultureInfo culture)
        {
            AssemblyName result = new AssemblyName(baseAssemblyName.Name + ExtensionPart);

            result.CultureInfo = culture;
            result.ProcessorArchitecture = baseAssemblyName.ProcessorArchitecture;
            result.Flags = baseAssemblyName.Flags;
            result.HashAlgorithm = baseAssemblyName.HashAlgorithm;
            result.KeyPair = baseAssemblyName.KeyPair;
            result.Version = baseAssemblyName.Version;
            result.VersionCompatibility = baseAssemblyName.VersionCompatibility;

            return result;
        }

        #endregion

        #region Private Members

        private static string RemoveResourceExtension(string value)
        {
            int index = value.LastIndexOf(ExtensionPart);
            string result = value.Remove(index, ExtensionPart.Length);

            return result;
        }

        /// <summary>
        /// Creates local assembly file name.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static string CreateLocalAssemblyFileName(string path, AssemblyName assemblyName)
        {
            if (assemblyName.CultureInfo == CultureInfo.InvariantCulture)
            {
                throw new SystemException("Cannot create base assembly.");
            }

            string fileNameWithoutExtension = assemblyName.Name;
            string fileExtension = ".dll";

            path = Path.Combine(path, assemblyName.CultureInfo.IetfLanguageTag);
            string fileName = fileNameWithoutExtension + fileExtension;

            string fullName = Path.Combine(path, fileName);

            return fullName;
        }

        /// <summary>
        /// Creates local assembly file name.
        /// </summary>
        /// <param name="baseAssemblyName"></param>
        /// <returns></returns>
        public static string CreateLocalAssemblyFileName(AssemblyName baseAssemblyName)
        {
            string fileNameWithoutExtension = baseAssemblyName.Name;
            string fileExtension = ".dll";

            string fileName = fileNameWithoutExtension + ExtensionPart + fileExtension;

            return fileName;
        }

        #endregion
    }
}

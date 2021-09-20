using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Threading;
#endif
#if NETFX_CORE || WINDOWS_UWP
using Windows.UI.Xaml;
#endif
using LogoFX.Client.Mvvm.View.Annotations;

namespace LogoFX.Client.Mvvm.View.Localization
{
    /// <summary>
    /// Represents the localization manager.
    /// </summary>
    public sealed class LocalizationManager : INotifyPropertyChanged
    {
#region Fields

        private static readonly Lazy<LocalizationManager> s_instance =
            new Lazy<LocalizationManager>(() => new LocalizationManager());

        private readonly Dictionary<string, string> _defaultCache =
            new Dictionary<string, string>();

        private readonly Dictionary<string, string> _localCache =
            new Dictionary<string, string>();

        private readonly Dictionary<CultureInfo, List<AssemblyName>> _assemblies =
            new Dictionary<CultureInfo, List<AssemblyName>>();

        private CultureInfo _currentCulture;

        private readonly DispatcherTimer _timer;

        private const int MaxStringLength = 1024;

#endregion

#region Constructors

        private LocalizationManager()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) =>
            {
                if (!Thread.CurrentThread.CurrentCulture.Equals(_currentCulture))
                {
                    CurrentCulture = Thread.CurrentThread.CurrentCulture;
                }
            };
            _timer.Start();

            UpdateAssemblies();
            CurrentCulture = Thread.CurrentThread.CurrentCulture;
        }

#endregion

#region Public Properties

        /// <summary>
        /// Gets the instance of <see cref="LocalizationManager"/>
        /// </summary>
        public static LocalizationManager Instance => s_instance.Value;

        /// <summary>
        /// Gets the collection of available cultures.
        /// </summary>
        public IEnumerable<CultureInfo> AvailableCultures => _assemblies.Keys.ToArray();

        /// <summary>
        /// Gets or sets the current culture.
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != null && _currentCulture.Equals(value))
                {
                    return;
                }

                _currentCulture = _assemblies.ContainsKey(value) ? value : CultureInfo.InvariantCulture;

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = _currentCulture;
                RebuildCache(value);

                OnPropertyChanged();
                OnPropertyChanged("FlowDirection");
            }
        }

        /// <summary>
        /// AssemblyNames accessor
        /// </summary>
        public static Func<IEnumerable<string>> GetAssemblyNames = () => new[] { Assembly.GetEntryAssembly().CodeBase };

        /// <summary>
        /// Gets the current flow direction.
        /// </summary>
        public FlowDirection FlowDirection => CurrentCulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        #endregion

#region Public Methods

        /// <summary>
        /// Gets resource by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            string result = null;

            if (!CurrentCulture.Equals(CultureInfo.InvariantCulture))
            {
                if (!_localCache.TryGetValue(key, out result))
                {
                    result = null;
                }
            }

            if (ReferenceEquals(result, null))
            {
                if (!_defaultCache.TryGetValue(key, out result))
                {
                    //result = "{" + key + "}";
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the resource by its key and optional list of arguments.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetString(string key, params object[] args)
        {
            string format = GetString(key);

            if ((args == null) || (args.Length <= 0))
            {
                return format;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string stringArg = args[i] as string;

                if ((stringArg != null) && (stringArg.Length > MaxStringLength))
                {
                    args[i] = stringArg.Substring(0, MaxStringLength - 3) + "...";
                }
            }

            return String.Format(CultureInfo.CurrentCulture, format ?? String.Empty, args);
        }

#endregion

#region Private Members

        private void UpdateAssemblies()
        {
            _assemblies.Clear();

            string folder = Assembly.GetExecutingAssembly().Location;
            folder = Path.GetDirectoryName(folder);

            if (String.IsNullOrEmpty(folder))
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                return;
            }

            //string[] validNames = Settings.Default.ResourceFiles.OfType<string>().ToArray();

            foreach (string validName in GetAssemblyNames())
            {
                string fileName = Path.IsPathRooted(validName) ? validName : Path.Combine(folder, Path.GetFileName(validName));

                if (!File.Exists(fileName))
                {
                    continue;
                }

                IEnumerable<AssemblyName> localAssemblyCollection =
                    AssemblyLoaderService.Instance.GenerateLocalAssemblyCollection(fileName);

                if (ReferenceEquals(localAssemblyCollection, null))
                {
                    continue;
                }

                AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);
                AddAssembly(CultureInfo.InvariantCulture, assemblyName);

                foreach (AssemblyName localAssemblyName in localAssemblyCollection)
                {
                    AddAssembly(localAssemblyName.CultureInfo, localAssemblyName);
                }
            }

            RebuildCache(CultureInfo.InvariantCulture);
        }

        private void AddAssembly(CultureInfo cultureInfo, AssemblyName assemblyName)
        {
            List<AssemblyName> assemblyNames;

            bool found = _assemblies.TryGetValue(cultureInfo, out assemblyNames);

            if (!found)
            {
                assemblyNames = new List<AssemblyName>();
                _assemblies.Add(cultureInfo, assemblyNames);
            }

            assemblyNames.Add(assemblyName);
        }

        private void RebuildCache(CultureInfo cultureInfo)
        {
            Dictionary<string, string> cache =
                Equals(cultureInfo, CultureInfo.InvariantCulture)
                    ? _defaultCache
                    : _localCache;

            cache.Clear();

            if (_assemblies.ContainsKey(cultureInfo))
            {
                foreach (AssemblyName assemblyName in _assemblies[cultureInfo])
                {
                    AssemblyResourceLoader assemblyResourceLoader =
                        AssemblyResourceLoader.CreateInNewDomain(assemblyName);
                    ResourceSetCollection result = assemblyResourceLoader.ExtractResources();
                    AssemblyResourceLoader.DestroyDomain(assemblyResourceLoader);

                    foreach (KeyValuePair<string, string> pair in result.SelectMany(x => x.Value))
                    {
                        cache[pair.Key] = pair.Value;
                    }
                }
            }
        }

#endregion

#region INotifyPropertyChanged

        /// <summary>
        /// Defines event for <see cref="INotifyPropertyChanged"/> default implementation.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#endregion
    }
}
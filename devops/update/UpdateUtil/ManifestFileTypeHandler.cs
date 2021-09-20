using System.IO;
using System.Xml;

namespace UpdateUtil
{
    internal sealed class ManifestFileTypeHandler : FileTypeHandlerBase
    {
        private readonly ManifestFileTypeHandlerOptions _options;

        public ManifestFileTypeHandler(ManifestFileTypeHandlerOptions options)
        {
            _options = options;
        }

        public ManifestFileTypeHandler()
            :this(new ManifestFileTypeHandlerOptions())
        {}

        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            NavigationHelper.GoUp(3);
            NavigationHelper.Cd("pack");
            var version = versionInfo.ToString();
            var manifestFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), "*.nuspec", SearchOption.AllDirectories);
            foreach (var manifestFile in manifestFiles)
            {
                var doc = new XmlDocument();
                doc.Load(manifestFile);
                if (_options.UpdatePackageVersion)
                {
                    var versionElement = doc.GetElementsByTagName("package")[0]["metadata"]["version"];
                    versionElement.InnerText = version;
                }
                if (_options.UpdateDependencyVersion)
                {
                    var dependenciesElement = doc.GetElementsByTagName("package")[0]["metadata"]["dependencies"];
                    if (dependenciesElement != null)
                    {
                        var dependencies = dependenciesElement.GetElementsByTagName("dependency");
                        foreach (XmlNode dependencyElement in dependencies)
                        {
                            if (dependencyElement.Attributes["id"].Value.StartsWith(prefix))
                            {
                                dependencyElement.Attributes["version"].Value = version;
                            }
                        }
                    }
                }
                
                doc.Save(manifestFile);
            }
            NavigationHelper.GoUp(1);
            NavigationHelper.NavigateToBin();
        }
    }    
}
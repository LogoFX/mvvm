using System.IO;
using System.Xml;

namespace UpdateUtil
{
    internal sealed class SdkProjectFileTypeHandler : FileTypeHandlerBase
    {
        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            NavigationHelper.GoUp(4);
            var version = versionInfo.VersionCore;
            var projectFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.AllDirectories);
            NavigationHelper.Cd("devops");
            NavigationHelper.NavigateToBin();

            foreach (var projectFile in projectFiles)
            {
                if (projectFile.Contains("templatepack"))
                {
                    continue;
                }
                var doc = new XmlDocument();
                doc.Load(projectFile);
                var firstPropertyGroup = doc.GetElementsByTagName("Project")[0]["PropertyGroup"];
                var targetFrameworkElement = firstPropertyGroup?.GetElementsByTagName("TargetFramework")[0];
                if (targetFrameworkElement == null)
                {
                    continue;
                }

                if (!targetFrameworkElement.InnerText.StartsWith("netcoreapp") &&
                    !targetFrameworkElement.InnerText.StartsWith("net6") &&
                    !targetFrameworkElement.InnerText.StartsWith("netstandard"))
                {
                    continue;
                }
                var versionElement = doc.GetElementsByTagName("Project")[0]["PropertyGroup"].GetElementsByTagName("Version")[0];
                if (versionElement != null)
                {
                    versionElement.InnerText = version;
                }
                doc.Save(projectFile);
            }
        }
    }
}
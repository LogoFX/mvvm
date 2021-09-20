using System.IO;
using YamlDotNet.RepresentationModel;

namespace UpdateUtil
{
    internal sealed class CIFileTypeHandler : FileTypeHandlerBase
    {
        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            NavigationHelper.GoUp(4);
            var ciFile = "appveyor.yml";
            var yaml = new YamlStream();
            using (var reader = new StreamReader(ciFile))
            {
                // Load the stream
                yaml.Load(reader);
                var nodesEnumerator = yaml.Documents[0].RootNode.AllNodes.GetEnumerator();
                nodesEnumerator.MoveNext();
                var rootNode = nodesEnumerator.Current as YamlMappingNode;
                var versionNode = rootNode.Children[new YamlScalarNode("version")] as YamlScalarNode;
                versionNode.Value = $"{versionInfo.VersionCore}.{{build}}";
            }

            using (var writer = new StreamWriter(ciFile))
            {
                yaml.Save(writer, assignAnchors: false);
            }
            NavigationHelper.Cd("devops");
            NavigationHelper.NavigateToBin();
        }
    }
}
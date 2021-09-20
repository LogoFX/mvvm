using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UpdateUtil
{
    internal sealed class AssemblyInfoFileTypeHandler : FileTypeHandlerBase
    {
        public override void UpdateFiles(string prefix, VersionInfo versionInfo)
        {
            NavigationHelper.GoUp(4);
            var assemblyInfoFiles =
                Directory.GetFiles(Directory.GetCurrentDirectory(), "AssemblyInfo.cs", SearchOption.AllDirectories)
                    .Where(t => !t.Contains("obj"));
            NavigationHelper.Cd("devops");
            NavigationHelper.NavigateToBin();
            var ps1File = @"..\..\patch-assembly-info.ps1";

            foreach (var assemblyInfoFile in assemblyInfoFiles)
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{ps1File}\" \"{assemblyInfoFile}\" \"{versionInfo.VersionCore}\"",
                    UseShellExecute = false
                };
                Process.Start(startInfo).WaitForExit();
            }
        }
    }
}
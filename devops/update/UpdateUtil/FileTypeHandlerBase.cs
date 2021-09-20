namespace UpdateUtil
{
    abstract class FileTypeHandlerBase
    {
        public abstract void UpdateFiles(string prefix, VersionInfo versionInfo);
    }
}
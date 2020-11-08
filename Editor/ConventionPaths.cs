using HexUN.Data;

namespace HexUN.Convention
{
    /// <summary>
    /// IMportant paths for convention scripts
    /// </summary>
    public static class ConventionPaths
    {
        private const string cConventionConfigPath = "Assets/Config/Convention.yaml";

        private static UnityPath _conventionConfigPath;

        public static UnityPath ConventionConfigPath
        {
            get
            {
                if (_conventionConfigPath == null) _conventionConfigPath = new UnityPath(cConventionConfigPath);
                return _conventionConfigPath;
            }
        }
    }
}
using System.Collections.Generic;

namespace HexUN.Convention
{
    /// <summary>
    /// Runtime deserialized type of convention yaml, which defines the
    /// project conventions
    /// </summary>
    public class ConventionYaml
    {
        public Dictionary<string, string> Folders;
        public Dictionary<string, string> Names;
        public List<FolderOperation> FolderOps;
        
        public class FolderOperation
        {
            public string Target;
            public string Destination;
            public EFolderOperation FolderOp;
            public EFileOperation FileOp;
        }
    }
}
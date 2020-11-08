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
        public List<OperationArgs> Operations;
        
        public class OperationArgs
        {
            public string Target;
            public string Destination;
            public string Operation;
            public ECrawlMode CrawlMode;
            public EMirrorMode MirrorMode;
            public EKeyMode KeyMode;
        }
    }
}
using Hex.Paths;

namespace HexUN.Convention
{
    /// <summary>
    /// File operations perform some action on a source file and writes to
    /// the destination file
    /// </summary>
    public abstract class AFileOperation
    {
        /// <summary>
        /// Perform a file operation. Returns operation logs describing outcome of operation. 
        /// Operations never fail, but errors will be logged. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public abstract OperationLog[] Operate(PathString source, PathString destination, string key = null);
    }
}

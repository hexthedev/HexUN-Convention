using Hex.Paths;
using HexUN.Data;

namespace HexUN.Convention
{
    /// <summary>
    /// An operation to be performed on a folder. Folder operations define how the child files and folders
    /// are crawled so that file operation can occur. 
    /// </summary>
    public abstract class AFolderOperation
    {
        /// <summary>
        /// Performs an operation on the given folder
        /// </summary>
        public abstract OperationLog[] Operate(PathString target, PathString destination, EFileOperation operation);
    }
}
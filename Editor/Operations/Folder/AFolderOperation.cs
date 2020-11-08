using Hex.Paths;
using HexUN.Data;

namespace HexUN.Convention
{
    /// <summary>
    /// An operation to be performed based on a folder
    /// </summary>
    public abstract class AFolderOperation
    {
        /// <summary>
        /// Performs an operation on the given folder
        /// </summary>
        public abstract void Operate(PathString target, PathString destination);

        public AFolderOperation GetOperation(EOperation operation)
        {
            if (operation == EOperation.ConvertToSo) return new ConvertToSo();
            return default;
        }

        public enum EOperation
        {
            ConvertToSo = 0
        }
    }
}
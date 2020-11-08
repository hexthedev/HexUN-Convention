using Hex.Paths;

namespace HexUN
{
    /// <summary>
    /// File operations perform some action on a destination file using some target file
    /// </summary>
    public abstract class AFileOperation
    {
        public abstract void Operate(PathString target, PathString destination);
    }
}

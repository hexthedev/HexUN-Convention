namespace HexUN.Convention
{
    public struct OperationLog
    {
        public EOperationLogCategory Category;
        public string Name;
        public string Description;

        public string ToString()
        {
            return $"[{Category}] {Name} : {Description}";
        }
    }
}
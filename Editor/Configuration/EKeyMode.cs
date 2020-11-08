namespace HexUN.Convention
{
    /// <summary>
    /// In operations that require a key, how should the key be determined
    /// </summary>
    public enum EKeyMode
    {
        /// <summary>
        /// No key should be determined
        /// </summary>
        NoKey = 0,

        /// <summary>
        /// The name of the shallowest folder is taken as key
        /// </summary>
        ShallowFolder = 1,

        /// <summary>
        /// The deepest folder should be taken as the key
        /// </summary>
        DeepestFolder = 2
    }
}

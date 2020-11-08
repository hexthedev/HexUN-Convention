namespace HexUN.Convention
{
    /// <summary>
    /// When a folder operation occurs the folder can be crawled. Crawling means
    /// that sub folder structures will be used to apply batches separetly. 
    /// </summary>
    public enum ECrawlMode
    {
        /// <summary>
        /// All operations occur at the root folder level
        /// </summary>
        None = 0,

        /// <summary>
        /// The direct children of the root folder are passed as
        /// groups to the folder operation
        /// </summary>
        DirectChildren = 0
    }
}

namespace NorthwindApiApp
{
    /// <summary>
    /// Enum OperationMode.
    /// </summary>
    internal enum OperationMode
    {
        /// <summary>
        /// Represents an operation mode with EntityFramework.
        /// </summary>
        EntityFramework = 1,

        /// <summary>
        /// Represents an operation mode in memory.
        /// </summary>
        InMemory = 2,

        /// <summary>
        /// Represents an operation mode with SQL.
        /// </summary>
        Sql = 3,
    }
}

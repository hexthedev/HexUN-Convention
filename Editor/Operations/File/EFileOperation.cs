using UnityEngine;

namespace HexUN.Convention
{
    /// <summary>
    /// Represents a file operation
    /// </summary>
    public enum EFileOperation
    {
        ConvertToGameDataSo = 0
    }

    /// <summary>
    /// Utilities for EFileOperations
    /// </summary>
    public static class UTEFileOperation
    {
        /// <summary>
        /// Returns the file operation based on enum
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static AFileOperation GetOperation(EFileOperation operation)
        {
            switch (operation) 
            {
                case EFileOperation.ConvertToGameDataSo: return new ConvertToGameDataSo();
            }

            return default;
        }
    }
}
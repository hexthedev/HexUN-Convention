using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Convention
{
    public enum EFolderOperation
    {
        RootKeyMirror = 0
    }

    public static class UTEFolderOperation
    {
        public static AFolderOperation GetOperation(EFolderOperation operation)
        {
            switch (operation) 
            {
                case EFolderOperation.RootKeyMirror: return new RootKeyMirror();
            }

            return default;
        }
    }
}
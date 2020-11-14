using Hex.Paths;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HexUN.Convention
{
    /// <summary>
    /// A root key mirror operation expects the first level below the target folder to be a set of folders. 
    /// The name of each folder is used as the key in the file operation. All further nested folders are copied
    /// to the destination structure. 
    /// </summary>
    public class RootKeyMirror : AFolderOperation
    {
        private const string cOperationName = nameof(RootKeyMirror);

        public override OperationLog[] Operate(PathString target, PathString destination, EFileOperation operation)
        {
            List<OperationLog> logs = new List<OperationLog>();
            AFileOperation op = UTEFileOperation.GetOperation(operation);

            if (!target.TryAsDirectoryInfo(out DirectoryInfo info)) return Error("Failed to get target as directory");

            foreach (DirectoryInfo child in info.GetDirectories())
            {
                string key = child.Name;
                PathString operationDest = destination + child.Name;
                logs.AddRange(SingleDepthOperation(child, operationDest, op, key));
            }

            OperationLog[] SingleDepthOperation(DirectoryInfo folder, PathString dest, AFileOperation innerOp, string key)
            {
                List<OperationLog> innerLogs = new List<OperationLog>();
   
                dest.CreateIfNotExistsDirectory();

                foreach (DirectoryInfo i in folder.GetDirectories())
                    innerLogs.AddRange(SingleDepthOperation(i, dest + i.Name, innerOp, key));

                foreach (FileInfo i in folder.GetFiles())
                {
                    PathString targ = i.FullName;
                    if (targ.Extension.Contains("meta")) continue;

                    PathString child = dest + i.Name;
                    innerLogs.Add(Info($"Performing {operation} with key:{key} on target {targ} to destination {child.RemoveAtEnd()}"));
                    innerLogs.AddRange(innerOp.Operate(targ, dest + i.Name, key));
                }

                return innerLogs.ToArray();
            }

            return logs.ToArray();
        }

        private OperationLog[] Error(string description)
        {
            return new OperationLog[] {
                new OperationLog()
                {
                    Name = cOperationName,
                    Category = EOperationLogCategory.ERRO,
                    Description = description
                }
            };
        }

        private OperationLog Info(string description)
        {
            return new OperationLog()
            {
                Name = cOperationName,
                Category = EOperationLogCategory.INFO,
                Description = description
            };
        }
    }
}
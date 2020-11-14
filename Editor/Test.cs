using HexUN.Data;
using UnityEngine;
using YamlDotNet.Serialization;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Hex.Paths;
using System;
using HexCS.Data.Persistence;
using System.Text;
using HexCS.Core;

namespace HexUN.Convention {
    public class Test : MonoBehaviour
    {
        private const string cOperationLogName = nameof(Test);

        [MenuItem("Hex/Convention/PerformOperations")]
        public static void PerformOperations()
        {
            UnityPath convention = ConventionPaths.ConventionConfigPath;

            Deserializer de = new Deserializer();
            ConventionYaml yaml = default;

            using (StreamReader sr = new StreamReader(convention.AbsolutePath))
            {
                yaml = de.Deserialize<ConventionYaml>(sr);
            }

            List<OperationLog> logs = new List<OperationLog>();
            foreach(ConventionYaml.FolderOperation op in yaml.FolderOps)
            {
                logs.Add(Info($"Performing folder operation {op.FolderOp} using file operation {op.FileOp} on target {op.Target} to destination {op.Destination}"));
                AFolderOperation operation =  UTEFolderOperation.GetOperation(op.FolderOp);
                logs.AddRange(operation.Operate(yaml.Folders[op.Target], yaml.Folders[op.Destination], op.FileOp));
            }

            UnityPath logPath = Files.GetPath(EFileLocationType.Assets, ECommonFileType.Logs);
            logPath.AbsolutePath.CreateIfNotExistsDirectory();

            PathString writePath = logPath.AbsolutePath + $"Convention_PerformOperations_Log{DateTime.Now.ToString("yyyy_MM_dd_hh_mm")}.txt";
            writePath.CreateIfNotExistsFile();

            if(writePath.TryAsFileInfo(out FileInfo fi))
            {
                StringBuilder sb = new StringBuilder();

                foreach(OperationLog log in logs)
                    sb.AppendLine(log.ToString());

                fi.WriteString(sb.ToString(), Encoding.UTF8);
            }

            AssetDatabase.Refresh();
        }

        private static OperationLog Info(string description)
        {
            return new OperationLog()
            {
                Category = EOperationLogCategory.INFO,
                Name = cOperationLogName,
                Description = description
            };
        }
    }
}
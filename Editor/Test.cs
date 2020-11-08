using HexUN.Data;
using UnityEngine;
using YamlDotNet.Serialization;
using UnityEditor;
using System.IO;

namespace HexUN.Convention {
    public class Test : MonoBehaviour
    {
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




        }
    }
}
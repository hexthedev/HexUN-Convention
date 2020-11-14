using Hex.Paths;
using HexCS.Reflection;
using HexUN.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YamlDotNet.Serialization;

namespace HexUN.Convention
{
    /// <summary>
    /// Takes a target yaml file and updates/creates an so file of the same
    /// name the the destination. Requires a key, which is the type of scriptable object to write to.
    /// </summary>
    public class ConvertToGameDataSo : AFileOperation
    {
        private const string cOperationName = nameof(ConvertToGameDataSo);

        /// <inhertidoc />
        public override OperationLog[] Operate(PathString source, PathString destination, string key)
        {
            List<OperationLog> logs = new List<OperationLog>();

            PathString validDest = destination.Extension == "asset" ? destination : destination.AddExtension("asset");

            // Try open file
            if (!source.TryAsFileStream(FileMode.Open, out FileStream stream)) return Error($"Failed to get as stream");

            // Try to deserialize the yaml file
            GameData gd;
            using (stream)
            {
                using (TextReader r = new StreamReader(stream))
                {
                    if (!DeserializeGameDataYaml(r, out gd)) return Error($"Failed to deserialize");
                }
            }

            // Check that the So exists, if it dosen't, create it
            if (!destination.TryAsFileInfo(out FileInfo so)) return Error($"destination path is not a file");

            // Load the scriptable object
            ScriptableObject inst = UTScriptableObject.LoadOrCreateSoAsset(destination, key);
            AGameDataSo gdso = inst as AGameDataSo;

            if (gdso == null) return Error($"Provided key '{key}' does not derive from AGameDataSo");

            GenericPopulate(gd, ref gdso);
            logs.Add(new OperationLog() { Name = cOperationName, Category = EOperationLogCategory.Success, Description = "CovertToSo succeeded" });

            return logs.ToArray();
        }

        /// <summary>
        /// Tries to deserialize string as yaml and return GameData. 
        /// Returns null on fail.
        /// </summary>
        /// <param name="yaml"></param>
        /// <returns></returns>
        private bool DeserializeGameDataYaml(TextReader yaml, out GameData data)
        {
            try
            {    
                data = new Deserializer().Deserialize<GameData>(yaml);
                return true;
            }
            catch (Exception)
            {
                data = default;
                return false;
            }
        }

        /// <summary>
        /// Uses reflection to generically populate a ScritableObject that inherits AGameDataSo. It is expected that
        /// the keys in Properties and Collections correspond to fields on the given ScritableObject Type. This function
        /// looks up the field and, if it exists, analyzes the type. If the type is a string the value is added directly. If
        /// it is a primative, a parse is performed. If the type is an enum, then a parse is performed. 
        /// If the field is AGameDataSo reference, then given string is interpreted as a key
        /// and the references array is searched for the reference. In any case, failure or error leads to skipping
        /// populating the field. 
        /// </summary>
        /// <param name="populate"></param>
        /// <param name="references"></param>
        /// <param name="gamedataAssembly"></param>
        public void GenericPopulate(GameData data, ref AGameDataSo populate)
        {
            AGameDataSo[] references = UTScriptableObject.GetAllInstances<AGameDataSo>();
            
            // Get the type and it's fields
            Type t = populate.GetType();
            FieldInfo[] fields = t.GetFields();

            // populate the key
            populate.Key = data.Key;

            if (data.Properties != null)
            {
                // populate the properties
                foreach (KeyValuePair<string, string> kv in data.Properties)
                {
                    // get the field with the same key
                    FieldInfo f = fields.FirstOrDefault(e => e.Name == kv.Key);
                    if (f == default) continue;

                    // if the field exists, populate it based on it's type
                    if (f.FieldType == typeof(string)) f.SetValue(populate, kv.Value);
                    if (f.FieldType.IsPrimitive) HandlePrimative(f, populate, kv.Value);// do convert
                    else if (UTType.GetTest(UTType.ETypeTest.ENUM)(f.FieldType)) HandleEnum(f, populate, kv.Value); // do enum
                    else if (f.FieldType.IsSubclassOf(typeof(AGameDataSo))) HandleSo(f, populate, kv.Value, references);// do a refernece handle
                }
            }

            if (data.Collections != null)
            {
                // populate the collections
                foreach (KeyValuePair<string, string[]> kv in data.Collections)
                {
                    // get the field with the same key
                    FieldInfo f = fields.FirstOrDefault(e => e.Name == kv.Key);
                    if (f == default) continue;

                    Type elType = f.FieldType.GetElementType();
                    Array a = Array.CreateInstance(elType, kv.Value.Length);

                    // if the field exists, populate it based on it's type
                    if (elType == typeof(string)) f.SetValue(populate, kv.Value);
                    if (elType.IsPrimitive) HandlePrimativeArray(f, populate, kv.Value, a);// do convert
                    else if (UTType.GetTest(UTType.ETypeTest.ENUM)(elType)) HandleEnumArray(f, populate, kv.Value, a); // do enum
                    else if (elType.IsSubclassOf(typeof(AGameDataSo))) HandleSoArray(f, populate, kv.Value, references, a);// do a refernece handle
                }
            }

            // Singles
            void HandleSingle(FieldInfo fi, AGameDataSo obj, string value, Func<Type, string, object> converter)
            {
                try
                {
                    fi.SetValue(obj, converter(fi.FieldType, value));
                }
                catch (Exception) { }
            }

            void HandlePrimative(FieldInfo fi, AGameDataSo obj, string value)
                => HandleSingle(fi, obj, value, (ty, v) => Convert.ChangeType(v, ty));

            void HandleEnum(FieldInfo fi, AGameDataSo obj, string value)
                => HandleSingle(fi, obj, value, (ty, v) => Enum.Parse(ty, v, true));


            // Array
            void HandleArray(FieldInfo fi, AGameDataSo obj, string[] value, Array pop, Func<Type, string, object> converter)
            {
                for (int i = 0; i < pop.Length; i++)
                {
                    try
                    {
                        pop.SetValue(converter(fi.FieldType, value[i]), i);
                    }
                    catch (Exception) { }
                }

                fi.SetValue(obj, pop);
            }

            void HandlePrimativeArray(FieldInfo fi, AGameDataSo obj, string[] value, Array pop)
                => HandleArray(fi, obj, value, pop, (ty, v) => Convert.ChangeType(v, ty));

            void HandleEnumArray(FieldInfo fi, AGameDataSo obj, string[] value, Array pop)
                => HandleArray(fi, obj, value, pop, (ty, v) => Enum.Parse(ty, v, true));

            // Set an so generically
            void HandleSo(FieldInfo fi, AGameDataSo obj, string value, AGameDataSo[] refs)
            {
                AGameDataSo soRef = refs.FirstOrDefault(e => e.Key == value);

                if (soRef != default)
                {
                    fi.SetValue(obj, soRef);
                }
            }

            // set an so array
            void HandleSoArray(FieldInfo fi, AGameDataSo obj, string[] value, AGameDataSo[] refs, Array pop)
            {
                for (int i = 0; i < pop.Length; i++)
                {
                    try
                    {
                        pop.SetValue(refs.FirstOrDefault(e => e.Key == value[i]), i);
                    }
                    catch (Exception) { }
                }

                fi.SetValue(obj, pop);
            }
        }

        private OperationLog[] Error(string description)
        {
            return new OperationLog[] {
                new OperationLog()
                {
                    Name = cOperationName,
                    Category = EOperationLogCategory.Error,
                    Description = description
                }
            };
        }
    }
}

using HexCS.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;

namespace HexUN.Convention
{
    /// <summary>
    /// Generic game data that can be deserialized from game data yamls
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// Key used to reference the gamedata
        /// </summary>
        public string Key;

        /// <summary>
        /// Any single value properties of the game data
        /// </summary>
        public Dictionary<string, string> Properties;
        
        /// <summary>
        /// Any multi value properties of the game data
        /// </summary>
        public Dictionary<string, string[]> Collections;
    }
}
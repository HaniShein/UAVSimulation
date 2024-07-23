using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using INIParser;
using Newtonsoft.Json;


//using INIParser;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Entities
{
    public class IniInputReader : IInputReader
    {
        public IniInputReader()
        {
        }

        public static SimParams Read()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent + "\\SimParams.txt";

            IniData data = new FileIniDataParser().ReadFile(path);
            
            Dictionary<string, string>  dict = new Dictionary<string, string>();
            foreach (KeyData keyData in data.Global)
            {
                dict.Add(keyData.KeyName, keyData.Value);
            }

            var json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            SimParams simParams = JsonConvert.DeserializeObject<SimParams>(json);

            return simParams;
        }
    }
}

using Eluant;
using Eluant.ObjectBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin
{
    public class EnvParam
    {
        [LuaMember("scriptDir")]
        public string ScriptDir;

        [LuaMember("scriptName")]
        public string ScriptName { get; set; }

        [LuaMember("tempDir")]
        public string TempDir { get; set; }

        [LuaMember("apiVersion")]
        public string ApiVersion { get; set; }
    }
}

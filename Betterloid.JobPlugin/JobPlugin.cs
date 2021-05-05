using Betterloid;
using System.IO;
using JobPlugin.Lua;
using Eluant;
using System.Diagnostics;



#if VOCALOID6
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.MusicalEditor;
using Yamaha.VOCALOID.VSM;
using VOCALOID = Yamaha.VOCALOID;
#elif VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
using Yamaha.VOCALOID.VSM;
using VOCALOID = Yamaha.VOCALOID.VOCALOID5;
#endif

namespace JobPlugin
{
    public class JobPlugin : IPlugin
    {
        public static JobPlugin Instance;
        public LuaLoader Lua { get; private set; }
        public JobManifest Manifest { get; set; }
        public string LUA => "C:/Program Files/VOCALOID5/Editor/JobPlugins/hello.lua";
        public string LUAFolder => Path.GetDirectoryName(LUA).Replace("\\", "/");
        public string LUAFilename => Path.GetFileName(LUA);
        public void Startup()
        {
            Instance = this;
            try
            {
                using (Lua = new LuaLoader())
                {
                    Lua.RunScript();
                    Manifest = Lua.GetManifest();
                    Lua.RegisterCommands();
                    ProcessParam processParam = new ProcessParam();
                    EnvParam envParam = new EnvParam { ApiVersion = "3.0.1.0", ScriptDir = LUAFolder, ScriptName = LUAFilename, TempDir = Path.GetTempPath().Replace("\\","/")};
                    int result = (int)Lua.Main(processParam, envParam);
                }
            }
            catch (LuaException ex)
            {
                MessageBoxDeliverer.GeneralError("An error occurred within the job plugin !");
                Debug.WriteLine($"{ex}");
            }
            catch (NoActivePartException ex)
            {
                MessageBoxDeliverer.GeneralError("There is no active part ! The job plugin cannot run.");
            }
        }
    }
}

/** TODO : Implement the following functions :
 * 
 * 
 * 
 * 
 * 
 * 
 **/
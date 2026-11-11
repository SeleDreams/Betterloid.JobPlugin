using System;
using System.Diagnostics;
using Eluant;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6 || VOCALOID7
using Yamaha.VOCALOID;
#endif


namespace JobPlugin.Lua.Commands
{
    public class VSGetSequencePathCommand
    {
        private static string VSGetSequencePath()
        {
            Debug.WriteLine("VSGetSequencePath called.");
            //var musicalEditor = JobPlugin.Instance.MusicalEditor;
            //var sequence = musicalEditor.ActiveTrack.Parent;
            return ""; // App.Shared.Document.DocumentUri.ToString().Replace("\\","/") ?? "";
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string>(VSGetSequencePath)))
            {
                lua.Globals["VSGetSequencePath"] = fn;
            }
        }
    }
}

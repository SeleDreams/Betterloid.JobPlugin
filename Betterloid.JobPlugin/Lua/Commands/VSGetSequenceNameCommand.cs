using System;
using Eluant;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSGetSequenceNameCommand
    {
        private static string VSGetSequenceName()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;
            return App.Shared.Document.FileName;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string>(VSGetSequenceName)))
            {
                lua.Globals["VSGetSequenceName"] = fn;
            }
        }
    }
}

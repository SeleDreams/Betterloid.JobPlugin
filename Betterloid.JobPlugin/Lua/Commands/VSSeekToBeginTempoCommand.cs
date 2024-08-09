using Eluant;
using System;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginTempoCommand
    {
        public static void VSSeekToBeginTempo()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.Sequence;
            JobPlugin.Instance.CurrentTempo = musicalEditor.ActiveTrack.Parent.Tempos[0];
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action(VSSeekToBeginTempo)))
            {
                lua.Globals["VSSeekToBeginTempo"] = fn;
            }
        }
    }
}

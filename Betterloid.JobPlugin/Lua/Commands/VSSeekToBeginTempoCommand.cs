using Eluant;
using System;
using System.Diagnostics;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginTempoCommand
    {
        public static void VSSeekToBeginTempo()
        {
            Debug.WriteLine("VSSeekToBeginTempo called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.Sequence;
            JobPlugin.Instance.CurrentTempo = musicalEditor.ActiveTrack.Parent.Tempos[0];
            JobPlugin.Instance.TempoSeek = 0;
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

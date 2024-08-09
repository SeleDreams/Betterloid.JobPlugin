using Eluant;
using System;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginTimeSigCommand
    {
        public static void VSSeekToBeginTimeSig()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.Sequence;

            JobPlugin.Instance.CurrentTimeSig = musicalEditor.ActiveTrack.Parent.TimeSigs[0];
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action(VSSeekToBeginTimeSig)))
            {
                lua.Globals["VSSeekToBeginTimeSig"] = fn;
            }
        }
    }
}

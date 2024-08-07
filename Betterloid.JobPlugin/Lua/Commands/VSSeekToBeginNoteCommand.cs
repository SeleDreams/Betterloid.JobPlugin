using System;
using System.Runtime.CompilerServices;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginNoteCommand
    {
        public static void VSSeekToBeginNote()
        {
            JobPlugin.Instance.CurrentNoteId = 0;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action(VSSeekToBeginNote)))
            {
                lua.Globals["VSSeekToBeginNote"] = fn;
            }
        }
    }
}

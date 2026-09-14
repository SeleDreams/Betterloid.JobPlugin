using Eluant;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginNoteCommand
    {
        public static void VSSeekToBeginNote()
        {
            Debug.WriteLine("VSSeekToBeginNote called.");
            var part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            JobPlugin.Instance.CurrentNote = null;
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

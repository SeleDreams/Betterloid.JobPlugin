using Eluant;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetTempoAtCommand
    {
        public static LuaVararg VSGetTempoAt(long ticks)
        {
            Debug.WriteLine("VSGetTempoAt called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.Sequence;
            VSMAbsTick absTicks;
#if VOCALOID5
            absTicks = new VSMAbsTick((int)ticks);
#elif VOCALOID6 || VOCALOID7
            absTicks = new VSMAbsTick(ticks);
#endif
            var tempo = musicalEditor.ActiveTrack.Parent.GetTempo(absTicks, true, false);
            return new LuaVararg(new LuaValue[] { new LuaNumber(1),new LuaNumber((float)tempo.Value * 0.01f) }, false);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<long,LuaVararg>(VSGetTempoAt)))
            {
                lua.Globals["VSGetTempoAt"] = fn;
            }
        }
    }
}

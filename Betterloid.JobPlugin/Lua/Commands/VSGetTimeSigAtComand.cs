using Eluant;
using System;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetTimeSigAtCommand
    {
        public static LuaVararg VSGetTimeSigAt(long ticks)
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.Sequence;
            VSMAbsTick absTicks;
#if VOCALOID5
            absTicks = new VSMAbsTick((int)ticks);
#elif VOCALOID6
            absTicks = new VSMAbsTick(ticks);
#endif
            var tempo = musicalEditor.ActiveTrack.Parent.GetTimeSig(absTicks, true, false);
            return new LuaVararg(new LuaValue[] { new LuaNumber(1),new LuaNumber(tempo.Numer),new LuaNumber(tempo.Denom) }, false);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<long,LuaVararg>(VSGetTimeSigAt)))
            {
                lua.Globals["VSGetTimeSigAt"] = fn;
            }
        }
    }
}

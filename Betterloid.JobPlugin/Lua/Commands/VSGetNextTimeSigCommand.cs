using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextTimeSigCommand
    {
        public static LuaVararg VSGetNextTimeSig()
        {
            Debug.WriteLine("VSGetNextTimeSig called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;

            VSLuaTimeSig luaTimeSig = new VSLuaTimeSig();
            WIVSMTimeSig timesig;
            var previousCurrentTimeSig = JobPlugin.Instance.CurrentTimeSig;
            bool isFirstTimeSig = JobPlugin.Instance.TimeSigSeek == 0;
            JobPlugin.Instance.TimeSigSeek += 1;
            if (sequence.TimeSigs.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }
            timesig = JobPlugin.Instance.CurrentTimeSig.Next ?? sequence.TimeSigs.Last();
            JobPlugin.Instance.CurrentTimeSig = timesig;
            luaTimeSig.Numerator = timesig.Numer;
            luaTimeSig.Denominator = timesig.Denom;
#if VOCALOID5
            luaTimeSig.PosTick = sequence.GetTickFromBar(timesig.PosBar).Tick;
#elif VOCALOID6 || VOCALOID7
            luaTimeSig.PosTick = sequence.GetTickFromBar(timesig.PosBar).Value;
#endif


            if (previousCurrentTimeSig != null && previousCurrentTimeSig.Next == null && !isFirstTimeSig)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaTimeSig.ToTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaTimeSig.ToTable() }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextTimeSig)))
            {
                lua.Globals["VSGetNextTimeSig"] = fn;
            }
        }
    }
}

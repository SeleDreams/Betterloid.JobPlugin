using Eluant;
using System;
using System.Linq;
using Yamaha.VOCALOID.VSM;
using JobPlugin.Lua.Types;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextTimeSigCommand
    {
        public static LuaVararg VSGetNextTimeSig()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;

            VSLuaTimeSig luaTimeSig = new VSLuaTimeSig();
            WIVSMTimeSig timesig;
            var previousCurrentTimeSig = JobPlugin.Instance.CurrentTimeSig;
            if (sequence.TimeSigs.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }
            timesig = JobPlugin.Instance.CurrentTimeSig.Next ?? sequence.TimeSigs.Last();
            JobPlugin.Instance.CurrentTimeSig = timesig;
            luaTimeSig.Numerator = timesig.Numer;
            luaTimeSig.Denominator = timesig.Denom;
            luaTimeSig.PosTick = sequence.GetTickFromBar(timesig.PosBar).Tick;

            if (previousCurrentTimeSig != null && previousCurrentTimeSig.Next == null)
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

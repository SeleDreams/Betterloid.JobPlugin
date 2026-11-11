using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextTempoCommand
    {
        public static LuaVararg VSGetNextTempo()
        {
            Debug.WriteLine("VSGetNextTempo called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;

            VSLuaTempo luaTempo = new VSLuaTempo();
            WIVSMTempo tempo;
            var previousCurrentTempo = JobPlugin.Instance.CurrentTempo;
            bool isFirstTempo = JobPlugin.Instance.TempoSeek == 0;
            JobPlugin.Instance.TempoSeek += 1;
            if (sequence.Tempos.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }

            tempo = JobPlugin.Instance.CurrentTempo.Next ?? sequence.Tempos.Last();
            JobPlugin.Instance.CurrentTempo = tempo;
            luaTempo.Tempo = tempo.Value * 0.01f;
#if VOCALOID5
            luaTempo.PosTick = tempo.RelPosition.Tick;
#elif VOCALOID6 || VOCALOID7
            luaTempo.PosTick = tempo.RelPosTick.Value;
#endif
            if (!isFirstTempo && previousCurrentTempo != null && previousCurrentTempo.Next == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaTempo.ToTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaTempo.ToTable() }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextTempo)))
            {
                lua.Globals["VSGetNextTempo"] = fn;
            }
        }
    }
}

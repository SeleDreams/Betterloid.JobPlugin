using Eluant;
using System;
using System.Linq;
using Yamaha.VOCALOID.VSM;
using JobPlugin.Lua.Types;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextTempoCommand
    {
        public static LuaVararg VSGetNextTempo()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;

            VSLuaTempo luaTempo = new VSLuaTempo();
            WIVSMTempo tempo;
            var previousCurrentTempo = JobPlugin.Instance.CurrentTempo;

            tempo = JobPlugin.Instance.CurrentTempo.Next ?? sequence.Tempos.Last();
            JobPlugin.Instance.CurrentTempo = tempo;
            luaTempo.Tempo = tempo.Value;
            
            luaTempo.PosTick = tempo.RelPosition.Tick;

            if (previousCurrentTempo != null && previousCurrentTempo.Next == null)
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

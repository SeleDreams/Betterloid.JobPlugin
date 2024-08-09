using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eluant;
using JobPlugin.Lua.Types;

namespace JobPlugin.Lua.Commands
{
    public class VSGetMusicalPartCommand
    {
        public static LuaVararg VSGetMusicalPart()
        {
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();
            VSLuaMusicalPart luaPart = new VSLuaMusicalPart();
            luaPart.ObjID = (ulong)part.Sequence.MidiParts.IndexOf(part);
            var firstNote = part.GetNote(0);
            int tick = 0;
            if (firstNote != null)
            {
                tick = (int)firstNote.RelPosition.Tick;
            }
            luaPart.PosTick = part.AbsPosition.Tick;
            var info = new ProcessParam();
            luaPart.DurTick = part.Duration.Tick;
            if (part.Notes.Count > 0)
            {
                var lastNote = part.Notes.Last();
                luaPart.PlayTime = (lastNote.RelPosition.Tick + lastNote.Duration.Tick) - firstNote.RelPosition.Tick;
            }
            else
            {
                luaPart.PlayTime = 0;
            }
            luaPart.Name = part.Name;
            luaPart.Comment = "";
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaPart.ToTable() }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetMusicalPart)))
            {
                lua.Globals["VSGetMusicalPart"] = fn;
            }
        }
    }
}

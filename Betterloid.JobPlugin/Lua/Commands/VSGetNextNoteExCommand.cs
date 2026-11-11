using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextNoteExCommand
    {
        public static LuaVararg VSGetNextNoteEx()
        {
            Debug.WriteLine("VSGetNextNoteEx called.");
            VSLuaNoteEx luaNote = new VSLuaNoteEx();
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();
            WIVSMNote note;
            var previousCurrentNote = JobPlugin.Instance.CurrentNote;
            if (part.Notes.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }
            if (JobPlugin.Instance.CurrentNote == null)
            {
                JobPlugin.Instance.CurrentNote = part.Notes.First();
                note = JobPlugin.Instance.CurrentNote;
            }
            else
            {
                note = JobPlugin.Instance.CurrentNote.Next ?? part.Notes.Last();
                JobPlugin.Instance.CurrentNote = note;
            }

            luaNote.ObjID = (ulong)part.Notes.IndexOf(note);
            luaNote.NoteNum = note.NoteNumber;
            luaNote.PhonemeLock = note.IsProtected;
            luaNote.Phonemes = note.Phonemes;
            luaNote.Lyric = note.Lyric;
            luaNote.Velocity = note.NoteVelocity;
           
#if VOCALOID5
            luaNote.DurTick = note.Duration.Tick;
            luaNote.PosTick = note.RelPosition.Tick;
#elif VOCALOID6 || VOCALOID7
            luaNote.DurTick = note.DurationTick.Value;
            luaNote.PosTick = note.RelPosTick.Value;
#endif

            var expression = note.GetNoteExpression();
            luaNote.Accent = expression.Accent;
            luaNote.BendDepth = expression.BendDepth;
            luaNote.FallPort = expression.FallPort ? 1 : 0;
            luaNote.BendLength = expression.BendLength;
            luaNote.Decay = expression.Decay;
            luaNote.Opening = expression.Opening;
            luaNote.RisePort = expression.RisePort ? 1 : 0;
#if VOCALOID6 || VOCALOID7
            long vibTicks = note.VibratoDurationTick.Value;
            if (luaNote.DurTick > 0)
            {
                luaNote.VibratoLength = (int)((vibTicks * 100) / luaNote.DurTick);
            }
            else
            {
                luaNote.VibratoLength = 0;
            }
#elif VOCALOID5
            // If V5 VibratoDuration is already percent, keep it.
            // If it is ticks, use the same ratio as V6.
            if (luaNote.DurTick > 0)
             {
              luaNote.VibratoLength = (int)((note.VibratoDuration * 100) / note.Duration.Tick);
             }
             else {
                luaNote.VibratoLength = 0;
             }
           
#endif
            if (luaNote.VibratoLength < 0) luaNote.VibratoLength = 0;
            if (luaNote.VibratoLength > 100) luaNote.VibratoLength = 100;
            // Update the current position

            LuaTable table = luaNote.ToLuaTable();
            Debug.WriteLine("note table:");
            foreach (var key in table.Keys)
            {
                Debug.WriteLine("  " + key.ToString() + " = " + table[key].ToString());
            }
            Debug.WriteLine("posTick via indexer = " + table["posTick"]);
            if (previousCurrentNote != null && previousCurrentNote.Next == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), table }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), table }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextNoteEx)))
            {
                lua.Globals["VSGetNextNoteEx"] = fn;
            }
        }
    }
}

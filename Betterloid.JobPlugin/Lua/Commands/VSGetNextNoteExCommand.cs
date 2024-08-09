using System;
using System.Linq;
using Eluant;
using JobPlugin.Lua.Types;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextNoteExCommand
    {
        public static LuaVararg VSGetNextNoteEx()
        {
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
            luaNote.DurTick = note.Duration.Tick;
            luaNote.PosTick = note.RelPosition.Tick;

            var expression = note.GetNoteExpression();
            luaNote.Accent = expression.Accent;
            luaNote.BendDepth = expression.BendDepth;
            luaNote.FallPort = expression.FallPort ? 1 : 0;
            luaNote.BendLength = expression.BendLength;
            luaNote.Decay = expression.Decay;
            luaNote.Opening = expression.Opening;
            luaNote.RisePort = expression.RisePort ? 1 : 0;
#if VOCALOID6
            luaNote.VibratoLength = note.VibratoDuration.Tick;
#elif VOCALOID5
                luaNote.VibratoLength = note.VibratoDuration;
#endif
            luaNote.VibratoType = (int)note.VibratoType;

            // Update the current position
            if (previousCurrentNote != null && previousCurrentNote.Next == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaNote.ToLuaTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaNote.ToLuaTable() }, true);
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

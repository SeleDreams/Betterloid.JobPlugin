using System;
using Eluant;
using JobPlugin.Lua.Types;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextNoteExCommand
    {
        public static LuaVararg VSGetNextNoteEx()
        {
            VSLuaNoteEx luaNote = new VSLuaNoteEx();
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();
            ulong noteId = JobPlugin.Instance.CurrentNoteId;
            LuaVararg returnValue;
            if (noteId < part.NumNotes) // Verify that the current note is within the part
            {
                // Populate the lua note's infos
                var note = part.GetNote(noteId);
                luaNote.ObjID = noteId;
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
                JobPlugin.Instance.CurrentNoteId++;

                returnValue = new LuaVararg(new LuaValue[]{ 1, luaNote.ToLuaTable() },true);
            }
            else
            {
                returnValue = new LuaVararg(new LuaValue[] { 0, LuaNil.Instance }, true);
            }
            return returnValue;
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

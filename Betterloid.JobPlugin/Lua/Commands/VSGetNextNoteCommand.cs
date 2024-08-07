using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextNoteCommand
    {
        public static LuaVararg VSGetNextNote()
        {
            VSLuaNote luaNote = new VSLuaNote();
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

                // Update the current position
                JobPlugin.Instance.CurrentNoteId++;

                returnValue = new LuaVararg(new LuaValue[]{ 1, luaNote.ToLuaTable() },true);
            }
            else
            {
                returnValue = new LuaVararg(new LuaValue[] { 0, luaNote.ToLuaTable() }, true);
            }
            return returnValue;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextNote)))
            {
                lua.Globals["VSGetNextNote"] = fn;
            }
        }
    }
}

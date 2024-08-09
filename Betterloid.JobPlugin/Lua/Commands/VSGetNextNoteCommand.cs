using System;
using System.Collections.Generic;
using System.Linq;
using Eluant;
using JobPlugin.Lua.Types;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextNoteCommand
    {
        public static LuaVararg VSGetNextNote()
        {
            VSLuaNote luaNote = new VSLuaNote();
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
            // Update the current position
            if (previousCurrentNote != null && previousCurrentNote.Next == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaNote.ToLuaTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaNote.ToLuaTable() }, true);
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

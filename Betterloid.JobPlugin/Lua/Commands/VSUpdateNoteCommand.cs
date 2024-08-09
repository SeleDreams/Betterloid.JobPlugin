using System;
using Eluant;
using Yamaha.VOCALOID.VSM;
using JobPlugin.Lua.Types;
using Yamaha.VOCALOID.VDM;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSUpdateNoteCommand
    {
        public static int VSUpdateNote(LuaTable noteTable)
        {
            VSLuaNote luaNote = new VSLuaNote(noteTable);
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();

            ulong noteId = luaNote.ObjID;
            if ((int)noteId >= part.Notes.Count)
            {
                return 0;
            }
            var note = part.Notes[(int)noteId];
            note.IsProtected = luaNote.PhonemeLock;
            note.SetNoteNumber(luaNote.NoteNum);
            note.Lyric = luaNote.Lyric;
            note.NoteVelocity = luaNote.Velocity;
#if VOCALOID6
            note.SetPhonemes(luaNote.Phonemes, true, note.LangID);
            note.SetDuration(new VSMRelTick(luaNote.DurTick));
            part.MoveNote(new VSMRelTick(luaNote.PosTick), note);
                    
#elif VOCALOID5
            note.SetPhonemes(luaNote.Phonemes, true);
            note.SetDuration((int)luaNote.DurTick);
            part.MoveNote(new VSMRelTick((int)luaNote.PosTick), note);
#endif
        JobPlugin.Instance.Modified = true;
        return 1;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateNote)))
            {
                lua.Globals["VSUpdateNote"] = fn;
            }
        }
    }
}

using System;
using Eluant;
using Yamaha.VOCALOID.VSM;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSInsertNoteCommand
    {
        public static int VSInsertNote(LuaTable noteTable)
        {
            VSLuaNote luaNote = new VSLuaNote(noteTable);
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();
#if VOCALOID6
            var note = part.InsertNote(new VSMRelTick(luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), new VSMNoteExpression(), new VSMAiNoteExpression(), luaNote.Lyric, luaNote.Phonemes, true, part.LangID);
#elif VOCALOID5
            var note = part.InsertNote(new VSMRelTick((int)luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), new VSMNoteExpression(), luaNote.Lyric, luaNote.Phonemes, true);
#endif
            if (note == null)
            {
                return 0;
            }
            note.IsProtected = luaNote.PhonemeLock;
            JobPlugin.Instance.Modified = true;
            return 1;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSInsertNote)))
            {
                lua.Globals["VSInsertNote"] = fn;
            }
        }
    }
}

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
    public class VSInsertNoteCommand
    {
        public static int VSInsertNote(LuaTable noteTable)
        {
            VSLuaNote luaNote = new VSLuaNote(noteTable);
            if (string.IsNullOrEmpty(luaNote.Phonemes) || luaNote.Phonemes == "nil")
            {
                return 0;
            }
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();

#if VOCALOID6
            VoiceBank vb = part.VoiceBank();
            var note = part.InsertNote(new VSMRelTick(luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), part.GetDefaultNoteExpression(), part.GetDefaultAiNoteExpression(), luaNote.Lyric, luaNote.Phonemes, true, vb.NativeLangID);
#elif VOCALOID5
            var note = part.InsertNote(new VSMRelTick((int)luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), part.GetDefaultNoteExpression(), luaNote.Lyric, luaNote.Phonemes, true);
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

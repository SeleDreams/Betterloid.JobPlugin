using System;
using Eluant;
using Yamaha.VOCALOID.VSM;
using JobPlugin.Lua.Types;

#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSInsertNoteExCommand
    {
        public static int VSInsertNoteEx(LuaTable noteTable)
        {
            VSLuaNoteEx luaNote = new VSLuaNoteEx(noteTable);

            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();
#if VOCALOID6
            var note = part.InsertNote(new VSMRelTick(luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), new VSMNoteExpression(luaNote.Accent,luaNote.Decay,luaNote.BendDepth,luaNote.BendLength,luaNote.Opening,luaNote.RisePort > 0, luaNote.FallPort > 0), part.GetDefaultAiNoteExpression(), luaNote.Lyric, luaNote.Phonemes, true, part.LangID);
#elif VOCALOID5
            var note = part.InsertNote(new VSMRelTick((int)luaNote.PosTick), new VSMNoteEvent((int)luaNote.DurTick, luaNote.NoteNum, luaNote.Velocity), new VSMNoteExpression(luaNote.Accent,luaNote.Decay,luaNote.BendDepth,luaNote.BendLength,luaNote.Opening,luaNote.RisePort > 0,luaNote.FallPort > 0), luaNote.Lyric, luaNote.Phonemes, true);
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
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSInsertNoteEx)))
            {
                lua.Globals["VSInsertNoteEx"] = fn;
            }
        }
    }
}

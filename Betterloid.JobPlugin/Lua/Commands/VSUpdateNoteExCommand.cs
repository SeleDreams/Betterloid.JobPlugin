using System;
using Eluant;
using Yamaha.VOCALOID.VSM;
using JobPlugin.Lua.Types;
using System.Windows.Media;


#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSUpdateNoteExCommand
    {
        public static int VSUpdateNoteEx(LuaTable noteTable)
        {
            VSLuaNoteEx luaNote = new VSLuaNoteEx(noteTable);
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();

            ulong noteId = luaNote.ObjID;
            if ((int)noteId >= part.Notes.Count)
            {
                return 0;
            }
            var note = part.Notes[(int)noteId];
            
            note.SetNoteNumber(luaNote.NoteNum);
            note.Lyric = luaNote.Lyric;
            note.NoteVelocity = luaNote.Velocity;
            if (note.IsProtected)
            {
                note.IsProtected = false;
            }
#if VOCALOID6
            note.SetPhonemes(luaNote.Phonemes, true, note.LangID);
            note.SetDuration(new VSMRelTick(luaNote.DurTick));
            part.MoveNote(new VSMRelTick(luaNote.PosTick), note);

#elif VOCALOID5
            note.SetPhonemes(luaNote.Phonemes, true);
            note.SetDuration((int)luaNote.DurTick);
            part.MoveNote(new VSMRelTick((int)luaNote.PosTick), note);
#endif
            note.SetNoteExpression(new VSMNoteExpression(luaNote.Accent, luaNote.Decay, luaNote.BendDepth, luaNote.BendLength, luaNote.Opening, luaNote.RisePort > 0, luaNote.FallPort > 0));
            note.IsProtected = luaNote.PhonemeLock;
            JobPlugin.Instance.Modified = true;
            return 1;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateNoteEx)))
            {
                lua.Globals["VSUpdateNoteEx"] = fn;
            }
        }
    }
}

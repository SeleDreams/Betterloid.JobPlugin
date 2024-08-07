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
    public class VSUpdateNoteCommand
    {
        public static int VSUpdateNote(LuaTable noteTable)
        {
            VSLuaNote luaNote = new VSLuaNote(noteTable);
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();

            ulong noteId = luaNote.ObjID;
            int returnValue;
            if (noteId < part.NumNotes) // Verify that the current note is within the part
            {
                    // Populate the lua note's infos
                    var note = part.GetNote(noteId);
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
                returnValue = 1;
            }
            else
            {
                returnValue = 0;
            }
            return returnValue;
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

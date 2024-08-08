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
    public class VSUpdateNoteExCommand
    {
        public static int VSUpdateNoteEx(LuaTable noteTable)
        {
            VSLuaNoteEx luaNote = new VSLuaNoteEx(noteTable);
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
                note.SetNoteExpression(new VSMNoteExpression(luaNote.Accent, luaNote.Decay, luaNote.BendDepth, luaNote.BendLength, luaNote.Opening, luaNote.RisePort > 0, luaNote.FallPort > 0));

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
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateNoteEx)))
            {
                lua.Globals["VSUpdateNoteEx"] = fn;
            }
        }
    }
}

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
    public class VSRemoveNoteCommand
    {
        public static int VSRemoveNote(LuaTable noteTable)
        {
            VSLuaNote luaNote = new VSLuaNote(noteTable);
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var part = musicalEditor.ActivePart ?? throw new NoActivePartException();

            ulong noteId = luaNote.ObjID;
            if (noteId < part.NumNotes) // Verify that the current note is within the part
            {
                part.RemoveNote(part.GetNote(noteId));
                JobPlugin.Instance.Modified = true;
                return 1;
            }
            else
            {
               return 0;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSRemoveNote)))
            {
                lua.Globals["VSRemoveNote"] = fn;
            }
        }
    }
}

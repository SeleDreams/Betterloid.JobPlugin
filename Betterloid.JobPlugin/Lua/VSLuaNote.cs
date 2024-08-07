using Eluant;
using Eluant.ObjectBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua
{
    // Note event type.
    public class VSLuaNote
    {
        public VSLuaNote(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            DurTick = (long)table["durTick"].ToNumber();
            NoteNum = (int)table["noteNum"].ToNumber();
            Velocity = (int)table["velocity"].ToNumber();
            Lyric = table["lyric"].ToString();
            Phonemes = table["phonemes"].ToString();

            if (table.ContainsKey("phLock"))
            {
                PhonemeLock = table["phLock"].ToBoolean();
            }
            if (table.ContainsKey("objID"))
            {
                ObjID = (ulong)table["objID"].ToNumber();
            }
        }

        public LuaTable ToLuaTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["objID"] = new LuaNumber(ObjID);
            table["posTick"] = new LuaNumber(PosTick);
            table["durTick"] = new LuaNumber(DurTick);
            table["noteNum"] = new LuaNumber(NoteNum);
            table["velocity"] = new LuaNumber(Velocity);
            table["lyric"] = new LuaString(Lyric);
            table["phonemes"] = new LuaString(Phonemes);
            table["phLock"] = new LuaNumber(PhonemeLock ? 1 : 0);
            return table;
        }
        public VSLuaNote()
        {

        }
        public ulong ObjID { get; set; }

        public long PosTick { get; set; } // Note ON time

        public long DurTick { get; set; } // Note Duration

        public int NoteNum { get; set; } // Tone

        public int Velocity { get; set; } // Velocity

        public string Lyric { get; set; } = ""; // lyric

        public string Phonemes { get; set; } = ""; // Phonetic symbols separated by a space

        public bool PhonemeLock { get; set; } = false; // Phonetic lock (protects phonemes from being modified when changing the lyrics)
    }

}

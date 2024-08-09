using Eluant;

namespace JobPlugin.Lua.Types
{
    public class VSLuaMusicalPart
    {
        public VSLuaMusicalPart() { }

        public VSLuaMusicalPart(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            PlayTime = (long)table["playTime"].ToNumber();
            DurTick = (long)table["durTick"].ToNumber();
            Name = table["name"].ToString();
            Comment = table["comment"].ToString();
            if (table.ContainsKey("objID"))
            {
                ObjID = (ulong)table["objID"].ToNumber();
            }
        }

        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["posTick"] = new LuaNumber(PosTick);
            table["playTime"] = new LuaNumber(PlayTime);
            table["durTick"] = new LuaNumber(DurTick);
            table["name"] = new LuaString(Name);
            table["comment"] = new LuaString(Comment);
            table["objID"] = new LuaNumber(ObjID);
            return table;
        }

        public long PosTick { get; set; }
        public long PlayTime { get; set; } // Maximum Duration of the part
        public long DurTick { get; set; } // Duration of the part
        public string Name { get; set; }
        public string Comment { get; set; }
        public ulong ObjID { get; set; }
    }
}

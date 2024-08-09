using Eluant;

namespace JobPlugin.Lua.Types
{
    public class VSLuaMusicalSinger
    {
        public VSLuaMusicalSinger() { }
        public VSLuaMusicalSinger(LuaTable table)
        {
            VirtualBankSelect = (int)table["vBS"].ToNumber();
            VirtualProgramChange = (int)table["vPC"].ToNumber();
            ComponentID = table["compID"].ToString();
            Breathiness = (int)table["breathiness"].ToNumber();
            Brightness = (int)table["brightness"].ToNumber();
            Clearness = (int)table["clearness"].ToNumber();
            GenderFactor = (int)table["genderFactor"].ToNumber();
            Opening = (int)table["opening"].ToNumber();
            if (table.ContainsKey("objID"))
            {
                ObjID = (ulong)table["objID"].ToNumber();
            }
        }

        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["vBS"] = new LuaNumber(VirtualBankSelect);
            table["vPC"] = new LuaNumber(VirtualProgramChange);
            table["compID"] = new LuaString(ComponentID);
            table["breathiness"] = new LuaNumber(Breathiness);
            table["brightness"] = new LuaNumber(Brightness);
            table["clearness"] = new LuaNumber(Clearness);
            table["genderFactor"] = new LuaNumber(GenderFactor);
            table["opening"] = new LuaNumber(Opening);
            table["objID"] = new LuaNumber(ObjID);
            return table;
        }
        public int VirtualBankSelect { get; set; }
        public int VirtualProgramChange { get; set; }
        public string ComponentID { get; set; }
        public int Breathiness { get; set; }
        public int Brightness { get; set; }
        public int Clearness { get; set; }
        public int GenderFactor { get; set; }
        public int Opening { get; set; }
        public ulong ObjID { get; set; }
    }
}

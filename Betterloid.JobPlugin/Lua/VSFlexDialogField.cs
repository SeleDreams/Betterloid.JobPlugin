using Eluant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua
{
    public class VSFlexDialogField
    {
        public VSFlexDialogField(LuaTable table)
        {
            Name = table["name"].ToString();
            Caption = table["caption"].ToString();
            InitialVal = table["initialVal"].ToString();
            int type = (int)table["type"].ToNumber();
            Type = (VSFlexDialogFieldType)type;
        }
        public LuaTable ToTable()
        {
            var lua = JobPlugin.Instance.Lua;
            LuaTable table = lua.Runtime.CreateTable();
            table["name"] = new LuaString(Name);
            table["caption"] = new LuaString(Caption);
            table["initialVal"] = new LuaString(InitialVal);
            table["type"] = new LuaNumber((int)Type);
            return table;
        }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string InitialVal { get; set; }
        public VSFlexDialogFieldType Type { get; set; }
    }
}

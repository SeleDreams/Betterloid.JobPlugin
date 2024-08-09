using Eluant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua.Types
{
    public  class VSLuaTempo
    {
        public long PosTick { get; set; }
        public float Tempo { get; set; }

        public VSLuaTempo() { }
        public VSLuaTempo(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            Tempo = (float)table["tempo"].ToNumber();
        }

        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["posTick"] = new LuaNumber(PosTick);
            table["tempo"] = new LuaNumber(Tempo);
            return table;
        }
    }
}

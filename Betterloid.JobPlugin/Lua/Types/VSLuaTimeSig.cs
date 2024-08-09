using Eluant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua.Types
{
    public class VSLuaTimeSig
    {
        public long PosTick { get; set; }
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public VSLuaTimeSig() { }
        public VSLuaTimeSig(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            Numerator = (int)table["numerator"].ToNumber();
            Denominator = (int)table["denominator"].ToNumber();
        }

        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["posTick"] = new LuaNumber(PosTick);
            table["numerator"] = new LuaNumber(Numerator);
            table["denominator"] = new LuaNumber(Denominator);
            return table;
        }
    }
}

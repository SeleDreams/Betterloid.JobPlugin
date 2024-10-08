﻿using Eluant;
using Eluant.ObjectBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPlugin.Lua.Types
{
    // Note event type, including the facial control parameters.
    public class VSLuaNoteEx : VSLuaNote
    {
        public VSLuaNoteEx() : base()
        {

        }
        public VSLuaNoteEx(LuaTable table) : base(table)
        {
            BendDepth = (int)table["bendDepth"].ToNumber();
            BendLength = (int)table["bendLength"].ToNumber();
            RisePort = (int)table["risePort"].ToNumber();
            FallPort = (int)table["fallPort"].ToNumber();
            Decay = (int)table["decay"].ToNumber();
            Accent = (int)table["accent"].ToNumber();
            Opening = (int)table["opening"].ToNumber();
            VibratoLength = (int)table["vibratoLength"].ToNumber();
            VibratoType = (int)table["vibratoType"].ToNumber();
        }

        public override LuaTable ToLuaTable()
        {
            LuaTable table = base.ToLuaTable();
            table["bendDepth"] = new LuaNumber(BendDepth);
            table["bendLength"] = new LuaNumber(BendLength);
            table["risePort"] = new LuaNumber(RisePort);
            table["fallPort"] = new LuaNumber(FallPort);
            table["decay"] = new LuaNumber(Decay);
            table["accent"] = new LuaNumber(Accent);
            table["opening"] = new LuaNumber(Opening);
            table["vibratoLength"] = new LuaNumber(VibratoLength);
            table["vibratoType"] = new LuaNumber(VibratoType);
            return table;
        }

        public int BendDepth { get; set; } // Bend Depth (0 - 100)
        public int BendLength { get; set; } // Bend Length (0 - 100)
        public int RisePort { get; set; } // Addition of portamento flag on line form
        public int FallPort { get; set; } // Portamento additional flag in the descending form
        public int Decay { get; set; } // Decay (0 - 100)
        public int Accent { get; set; } // Accent (0 - 100)
        public int Opening { get; set; } // Opening (0 - 127)
        public long VibratoLength { get; set; } // Vibrato length (Percentages rounded to integer)
        // Vibrato type (Type of vibrato)
        // 0: No vibrato
        // 1 - 4: Normal // 5 - 8: Extreme // 9 -12: Fast // 13 - 16: Slight
        public int VibratoType { get; set; }
    };

}

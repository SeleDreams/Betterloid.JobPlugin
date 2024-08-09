using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSUpdateMusicalPartCommand
    {
        public static int VSUpdateMusicalPart(LuaTable table)
        {
            VSLuaMusicalPart luaPart = new VSLuaMusicalPart(table);
            var part = JobPlugin.Instance.MusicalEditor.ActivePart;
            part.SetName(luaPart.Name);
            VSMAbsTick ticks;
            VSMRelTick durationTicks;
#if VOCALOID6
            ticks = new VSMAbsTick(luaPart.PosTick);
            durationTicks = new VSMRelTick(luaPart.PlayTime);
#elif VOCALOID5
            ticks = new VSMAbsTick((int)luaPart.PosTick);
            durationTicks = new VSMRelTick((int)luaPart.PlayTime);
#endif
            part.Parent.MovePart(ticks, part.Parent, part);
            part.SetDuration(durationTicks);
            return 1;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateMusicalPart)))
            {
                lua.Globals["VSUpdateMusicalPart"] = fn;
            }
        }
    }
}

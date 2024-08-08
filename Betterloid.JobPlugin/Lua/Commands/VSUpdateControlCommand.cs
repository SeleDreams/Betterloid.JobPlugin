using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSInsertControlCommand
    {
        
        private static int VSInsertControl(LuaTable table)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            VSLuaControl control = new VSLuaControl(table);
            VSControlType controlType = control.Type;
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
#if VOCALOID6
            VSMRelTick tick = new VSMRelTick(control.PosTick);
#elif VOCALOID5
            VSMRelTick tick = new VSMRelTick((int)control.PosTick);
#endif
            var controller = part.InsertController(tick, vsmControlType, control.Value);
            if (controller == null)
            {
                return 0;
            }
            else
            {
                JobPlugin.Instance.Modified = true;
                return 1;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSInsertControl)))
            {
                lua.Globals["VSInsertControl"] = fn;
            }
        }
    }
}

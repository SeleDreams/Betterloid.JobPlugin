using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Collections.Generic;
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
            VSMControllerType vsmControlType;
            try
            {
                vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            }
            catch
            {
                return 1;
            }
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
                List<WIVSMMidiController> controllers = new List<WIVSMMidiController>();
                for (ulong i = 0; i < part.GetNumController(vsmControlType); i++)
                {
                    controllers.Add(part.GetController(vsmControlType, i));
                }
                JobPlugin.Instance.Controllers[controlType] = controllers;
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

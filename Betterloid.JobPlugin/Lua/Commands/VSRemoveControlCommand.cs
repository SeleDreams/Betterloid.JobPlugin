using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSRemoveControlCommand
    {
        
        private static int VSRemoveControl(LuaTable table)
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
            var controller = part.GetController(vsmControlType, control.ObjID);
            if (controller == null)
            {
                return 0;
            }
            else
            {
                part.RemoveController(controller);
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
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSRemoveControl)))
            {
                lua.Globals["VSRemoveControl"] = fn;
            }
        }
    }
}

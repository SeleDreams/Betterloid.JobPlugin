using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSUpdateControlCommand
    {
        
        private static int VSUpdateControl(LuaTable table)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            VSLuaControl control = new VSLuaControl(table);
            VSControlType controlType = control.Type;
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            var controller = part.GetController(vsmControlType, control.ObjID);
            if (controller == null)
            {
                return 0;
            }
            else
            {
                controller.Value = control.Value;
                JobPlugin.Instance.Modified = true;
                return 1;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateControl)))
            {
                lua.Globals["VSUpdateControl"] = fn;
            }
        }
    }
}

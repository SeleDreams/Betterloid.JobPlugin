using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSGetNextControlCommand
    {
        
        private static LuaVararg VSGetNextControl(string type)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            ulong id = JobPlugin.Instance.CurrentControlId[controlType];
            var controller = part.GetController(vsmControlType, id);
            if (controller == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance },true);
            }
            else
            {
                JobPlugin.Instance.CurrentControlId[controlType] += 1;
                VSLuaControl luaControl = new VSLuaControl(controller.RelPosition.Tick,controller.Value,controlType,id);
                return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaControl.ToTable() }, true);
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,LuaVararg>(VSGetNextControl)))
            {
                lua.Globals["VSGetNextControl"] = fn;
            }
        }
    }
}

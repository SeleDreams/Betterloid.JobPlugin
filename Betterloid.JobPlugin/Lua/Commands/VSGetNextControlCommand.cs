using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSGetNextControlCommand
    {
        
        private static LuaVararg VSGetNextControl(string type)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType;
            try
            {
                vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            }
            catch
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }

            var previousController = JobPlugin.Instance.CurrentControl[controlType];
            var controller = JobPlugin.Instance.CurrentControl[controlType].Next ?? JobPlugin.Instance.Controllers[controlType].Last();
            JobPlugin.Instance.CurrentControl[controlType] = controller;
            VSLuaControl luaControl = new VSLuaControl(controller.RelPosition.Tick, controller.Value, controlType, (ulong)JobPlugin.Instance.Controllers[controlType].IndexOf(controller));
            if (previousController != null && previousController.Next == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaControl.ToTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaControl.ToTable() }, true);

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

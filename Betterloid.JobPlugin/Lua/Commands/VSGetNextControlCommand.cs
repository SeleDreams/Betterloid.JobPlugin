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
            Debug.WriteLine("VSGetNextControl called.");
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType;
            try
            {
                vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            }
            catch
            {
                throw new ArgumentOutOfRangeException("The current parameter " + type + " is undefined !");
                //return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }

            WIVSMMidiController previousController = JobPlugin.Instance.CurrentControl?[controlType];
            if (part.GetNumController(vsmControlType) == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), new VSLuaControl(0,0,controlType,0).ToTable() }, true);
            }
            var controller = JobPlugin.Instance.CurrentControl[controlType].Next ?? JobPlugin.Instance.Controllers[controlType].Last();
            JobPlugin.Instance.CurrentControl[controlType] = controller;
#if VOCALOID5
            VSLuaControl luaControl = new VSLuaControl(controller.RelPosition.Tick, controller.Value, controlType, (ulong)JobPlugin.Instance.Controllers[controlType].IndexOf(controller));
#elif VOCALOID6
            VSLuaControl luaControl = new VSLuaControl(controller.RelPosTick.Value, controller.Value, controlType, (ulong)JobPlugin.Instance.Controllers[controlType].IndexOf(controller));
#endif
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

using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSGetControlAtCommand
    {
        private static LuaVararg VSGetControlAt(string type, int posTick)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            var controller = part.GetController(vsmControlType, new VSMRelTick(posTick), true, false);
            if (controller == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, true);
            }
            else
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaNumber(controller.Value) }, true);
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,int,LuaVararg>(VSGetControlAt)))
            {
                lua.Globals["VSGetControlAt"] = fn;
            }
        }
    }
}

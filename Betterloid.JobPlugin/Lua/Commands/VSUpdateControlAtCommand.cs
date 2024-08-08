using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSUpdateControlAtCommand
    {
        
        private static int VSUpdateControlAt(string type, int posTick, int value)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            var controller = part.GetController(vsmControlType, new VSMRelTick(posTick), true, false);
            if (controller == null)
            {
                return 0;
            }
            else
            {
                controller.Value = value;
                JobPlugin.Instance.Modified = true;
                return 1;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,int,int,int>(VSUpdateControlAt)))
            {
                lua.Globals["VSUpdateControlAt"] = fn;
            }
        }
    }
}

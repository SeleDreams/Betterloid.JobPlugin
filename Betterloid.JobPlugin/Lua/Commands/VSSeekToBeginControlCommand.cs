using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSSeekToBeginControlCommand
    {
        
        private static int VSSeekToBeginControl(string type)
        {
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            var controller = part.GetController(vsmControlType,0);
            if (controller == null)
            {
                return 0;
            }
            else
            {
                JobPlugin.Instance.CurrentControlId[controlType] = 0;
                return 1;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,int>(VSSeekToBeginControl)))
            {
                lua.Globals["VSSeekToBeginControl"] = fn;
            }
        }
    }
}

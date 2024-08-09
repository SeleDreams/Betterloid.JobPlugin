using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VSM;
using System.Collections.Generic;

#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID.MusicalEditor;
#endif

namespace JobPlugin.Lua.Commands
{
    public static class VSSeekToBeginControlCommand
    {
        
        private static int VSSeekToBeginControl(string type)
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
                return 0;
            }
            ControlParameterTypeEnum controlparam;
            try
            {
                controlparam = VSLuaControl.VSControlTypeToControlParameterTypeEnum(controlType);
            }
            catch
            {
                return 0;
            }
            var controller = part.GetController(vsmControlType,0);
            if (controller == null)
            {
#if VOCALOID6
                var defaultValue = MusicalEditorViewModel.GetDefaultControllerValue(controlparam);
#elif VOCALOID5
                var defaultValue = JobPlugin.Instance.MusicalEditor.GetDefaultControllerValue(controlparam);
#endif
                controller = part.InsertController(VSMRelTick.Zero, vsmControlType, defaultValue);
                JobPlugin.Instance.Modified = true;
            }
            JobPlugin.Instance.CurrentControl[controlType] = controller;
            List<WIVSMMidiController> controllers = new List<WIVSMMidiController>();
            for (ulong i = 0; i < part.GetNumController(vsmControlType);i++)
            {
                controllers.Add(part.GetController(vsmControlType, i));
            }
            JobPlugin.Instance.Controllers[controlType] = controllers;
            return 1;
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

using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID;
#if VOCALOID6
using Yamaha.VOCALOID.MusicalEditor;
#elif VOCALOID5
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
#endif
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSGetDefaultControlValueCommand
    {
        
        private static LuaVararg VSGetDefaultControlValue(string type)
        {

            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            VSControlType controlType;
            if (!Enum.TryParse(type, out controlType))
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), new LuaNumber(0) }, true);
            }
            
            var controlParameterType = VSLuaControl.VSControlTypeToControlParameterTypeEnum(controlType);
#if VOCALOID6
            var defaultValue  = MusicalEditorViewModel.GetDefaultControllerValue(controlParameterType);
#elif VOCALOID5
            var defaultValue = JobPlugin.Instance.MusicalEditor.GetDefaultControllerValue(controlParameterType);
#endif
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaNumber(defaultValue) },true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,LuaVararg>(VSGetDefaultControlValue)))
            {
                lua.Globals["VSGetDefaultControlValue"] = fn;
            }
        }
    }
}

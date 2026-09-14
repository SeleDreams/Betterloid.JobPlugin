using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
#elif VOCALOID6
using Yamaha.VOCALOID.MusicalEditor;
#endif
namespace JobPlugin.Lua.Commands
{
    public static class VSGetControlAtCommand
    {
        private static LuaVararg VSGetControlAt(string type, int posTick)
        {
            Debug.WriteLine($"VSGetControlAt called with type={type}, posTick={posTick}");
            if (posTick < 0)
            {
                posTick = 0;
            }
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();

            VSControlType controlType = (VSControlType)Enum.Parse(typeof(VSControlType), type);
            VSMControllerType vsmControlType;
            try
            {
                vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            }
            catch
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(1),new LuaNumber(0)},false);
            }
            int value;
            WIVSMMidiController controller = part.GetController(vsmControlType, new VSMRelTick(posTick), true, false);
            
            if (controller == null)
            {
                var controlParameterType = VSLuaControl.VSControlTypeToControlParameterTypeEnum(controlType);
#if VOCALOID6
                var defaultValue = MusicalEditorViewModel.GetDefaultControllerValue(controlParameterType);
#elif VOCALOID5
            var defaultValue = JobPlugin.Instance.MusicalEditor.GetDefaultControllerValue(controlParameterType);
#endif
                value = defaultValue;
            }
            else
            {
                value = controller.Value;
                controller = null;
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaNumber(value) }, false);
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

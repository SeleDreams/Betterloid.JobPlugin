using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSUpdateControlAtCommand
    {
        
        private static int VSUpdateControlAt(string type, long posTick, int value)
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
                return 1;
            }
#if VOCALOID6
            var tick = new VSMRelTick(posTick);
#elif VOCALOID5
            var tick = new VSMRelTick((int)posTick);
#endif
            var result = part.GetController(vsmControlType, tick, true, false);
            if (result == null || result.RelPosition != tick)
            {
                result = part.InsertController(tick, vsmControlType, value);
            }
            if (result != null)
            {
                if (result.RelPosition == tick)
                {
                    result.Value = value;
                }
                JobPlugin.Instance.Modified = true;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,long,int,int>(VSUpdateControlAt)))
            {
                lua.Globals["VSUpdateControlAt"] = fn;
            }
        }
    }
}

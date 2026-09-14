using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSUpdateControlAtCommand
    {

        private static int VSUpdateControlAt(LuaVararg args)
        {
            if (args.Count < 3 || args[2] == null || args[2].IsNil())
            {
                return 0;
            }
            string type = args[0].ToString();
            int posTick = (int)args[1].ToNumber();
            int value = (int)args[2].ToNumber();

            Debug.WriteLine($"VSUpdateControlAt called with args : type={type}, posTick={posTick},value={value}");
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

            var result = part.GetController(vsmControlType, tick, false, true);
            if (result == null)
            {
                result = part.InsertController(tick, vsmControlType, value);
            }
#if VOCALOID6
            var resultPos = result.RelPosTick;
#elif VOCALOID5
            var resultPos = result.RelPosition;
#endif
            if (result != null)
            {
                if (resultPos == tick)
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
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg, int>(VSUpdateControlAt)))
            {
                lua.Globals["VSUpdateControlAt"] = fn;
            }
        }
    }
}

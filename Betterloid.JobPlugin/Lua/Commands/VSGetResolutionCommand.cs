using System;
using Eluant;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VSM;
#elif VOCALOID6
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VSM;
#endif

namespace JobPlugin.Lua.Commands
{
    public class VSGetResolutionCommand
    {
        private static int VSGetResolution()
        {
#if VOCALOID6
            return VSMTickRangeExtension.Resolution;
#elif VOCALOID5
            return VSMLoopRangeExtension.Resolution;
#endif
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<int>(VSGetResolution)))
            {
                lua.Globals["VSGetResolution"] = fn;
            }
        }
    }
}

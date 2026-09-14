using System;
using System.Diagnostics;
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
    public class VSGetPremeasureCommand
    {
        private static int VSGetPremeasure()
        {
            Debug.WriteLine("VSGetPremeasure called.");
            return 0;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<int>(VSGetPremeasure)))
            {
                lua.Globals["VSGetPreMeasure"] = fn;
            }
        }
    }
}

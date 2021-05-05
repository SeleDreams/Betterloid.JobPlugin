using Eluant;
using System;
using System.Diagnostics;

namespace JobPlugin.Lua.Commands
{
    public static class PrintCommand
    {
        private static void print(string value)
        {
            Debug.WriteLine(value);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action<string>(print)))
            {
                lua.Globals["print"] = fn;
            }
        }
    }
}

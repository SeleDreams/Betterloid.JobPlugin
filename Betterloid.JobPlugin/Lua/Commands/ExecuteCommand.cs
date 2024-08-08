using Eluant;
using System;
using System.Diagnostics;

namespace JobPlugin.Lua.Commands
{
    public static class ExecuteCommand
    {
        private static int Execute(string value)
        {
            var process = Process.Start("cmd.exe", "/C chcp 65001 && " + value);
            process.WaitForExit();
            return process.ExitCode;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,int>(Execute)))
            {
                LuaTable table = (LuaTable)lua.Globals["os"];
                table["execute"] = fn;
            }
        }
    }
}

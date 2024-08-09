using Eluant;
using System;
using System.Diagnostics;

namespace JobPlugin.Lua.Commands
{
    public static class ExecuteCommand
    {
        private static int Execute(string value)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = "/C chcp 65001 && " + value;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            Process processTemp = new Process();
            processTemp.StartInfo = processStartInfo;
            processTemp.EnableRaisingEvents = true;
            try
            {
                processTemp.Start();
                processTemp.WaitForExit();
            }
            catch (Exception e)
            {
                throw;
            }
            return processTemp.ExitCode;
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

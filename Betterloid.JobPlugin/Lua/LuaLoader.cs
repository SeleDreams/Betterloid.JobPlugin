using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Reflection;
using Eluant;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Betterloid.Wrappers;
using JobPlugin.Lua.Commands;
using Yamaha.VOCALOID.VLC;

namespace JobPlugin.Lua
{
    public class LuaLoader : IDisposable
    {
        private LuaRuntime runtime;

        // Required to find the lua5.1.dll present in the plugins's directory
        static void AddEnvironmentPaths(IEnumerable<string> paths)
        {
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

            string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

            Environment.SetEnvironmentVariable("PATH", newPath);
        }

        public LuaLoader()
        {
            string[] paths = { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };
            AddEnvironmentPaths(paths);

            runtime = new LuaRuntime();
        }

        public void RunScript()
        {
           
            // Register the package path of the currently running lua script
            runtime.DoString($"package.path = \"{JobPlugin.Instance.LUAFolder}/?.lua;\" .. package.path").Dispose();

            // Run the script to register its methods
            runtime.DoString($"dofile(\"{JobPlugin.Instance.LUA}\")").Dispose();


            PrintCommand.RegisterCommand(runtime);
        }

        public JobManifest GetManifest()
        {
            LuaTable manifestTable = (LuaTable)((LuaFunction)runtime.Globals["manifest"]).Call()[0];
            return new JobManifest(manifestTable);
        }

        public LuaNumber Main(ProcessParam processParam, EnvParam envParam)
        {
            LuaTransparentClrObject transparentProcess = new LuaTransparentClrObject(processParam);
            LuaTransparentClrObject transparentEnv = new LuaTransparentClrObject(envParam);
            return (LuaNumber)((LuaFunction)runtime.Globals["main"]).Call(transparentProcess, transparentEnv)[0];
        }

        public void RegisterCommands()
        {
            VSMessageBoxCommand.RegisterCommand(runtime);
        }

        public void Dispose()
        {
            runtime.Dispose();
        }
    }
}

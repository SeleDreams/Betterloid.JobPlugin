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
        public LuaRuntime Runtime { get; private set; }
        public string LUA => "C:/Program Files/VOCALOID5/Editor/JobPlugins/hello.lua";
        public string LUAFolder => Path.GetDirectoryName(LUA).Replace("\\", "/");
        public string LUAFilename => Path.GetFileName(LUA);

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

            Runtime = new LuaRuntime();
        }

        public void RunScript()
        {
           
            // Register the package path of the currently running lua script
            Runtime.DoString($"package.path = \"{LUAFolder}/?.lua;\" .. package.path").Dispose();

            // Run the script to register its methods
            Runtime.DoString($"dofile(\"{LUA}\")").Dispose();


            PrintCommand.RegisterCommand(Runtime);
        }

        public JobManifest GetManifest()
        {
            LuaTable manifestTable = (LuaTable)((LuaFunction)Runtime.Globals["manifest"]).Call()[0];
            return new JobManifest(manifestTable);
        }

        public LuaNumber Main(ProcessParam processParam, EnvParam envParam)
        {
            LuaTransparentClrObject transparentProcess = new LuaTransparentClrObject(processParam);
            LuaTransparentClrObject transparentEnv = new LuaTransparentClrObject(envParam);
            return (LuaNumber)((LuaFunction)Runtime.Globals["main"]).Call(transparentProcess, transparentEnv)[0];
        }

        public void RegisterCommands()
        {
            VSMessageBoxCommand.RegisterCommand(Runtime);
            
            VSGetAudioDeviceNameCommand.RegisterCommand(Runtime);
            
            VSSeekToBeginNoteCommand.RegisterCommand(Runtime);
            VSGetNextNoteCommand.RegisterCommand(Runtime);
            
            VSUpdateNoteCommand.RegisterCommand(Runtime);
            VSInsertNoteCommand.RegisterCommand(Runtime);
            VSRemoveNoteCommand.RegisterCommand(Runtime);

            VSDlgSetDialogTitleCommand.RegisterCommand(Runtime);
            VSDlgAddFieldCommand.RegisterCommand(Runtime);
            VSDlgDoModalCommand.RegisterCommand(Runtime);

            VSDlgGetBoolValueCommand.RegisterCommand(Runtime);
            VSDlgGetFloatValueCommand.RegisterCommand(Runtime);
            VSDlgGetIntValueCommand.RegisterCommand(Runtime);
            VSDlgGetStringValueCommand.RegisterCommand(Runtime);
        }

        public void Dispose()
        {
            Runtime.Dispose();
        }
    }
}

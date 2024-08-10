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
        public string LUA { get; set; }
        public string LUAFolder {
            get => Path.GetDirectoryName(LUA).Replace("\\", "/");
        }
        public string LUAFilename { get => Path.GetFileName(LUA); }

        // Required to find the lua5.1.dll present in the plugins's directory
        static void AddEnvironmentPaths(IEnumerable<string> paths)
        {
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

            string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

            Environment.SetEnvironmentVariable("PATH", newPath);
        }

        public LuaLoader(string luaPath)
        {
            LUA = luaPath;
            string[] paths = { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ,LUAFolder};
            AddEnvironmentPaths(paths);

            Runtime = new LuaRuntime();
        }

        public void RunScript()
        {
           
            // Register the package path of the currently running lua script
            Runtime.DoString($"package.path = \"{LUAFolder}/?.lua;\" .. package.path").Dispose();

            string content = File.ReadAllText(LUA);
            // Run the script to register its methods
            Runtime.DoString(content).Dispose();

            ExecuteCommand.RegisterCommand(Runtime);
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
            var returnValue = ((LuaFunction)Runtime.Globals["main"]).Call(transparentProcess, transparentEnv);
            if (returnValue.Count > 0)
            {
                return (LuaNumber)returnValue[0];
            }
            else
            {
                return new LuaNumber(0);
            }
            
        }

        public void RegisterCommands()
        {
            VSMessageBoxCommand.RegisterCommand(Runtime);
            
            VSGetAudioDeviceNameCommand.RegisterCommand(Runtime);
            
            VSSeekToBeginNoteCommand.RegisterCommand(Runtime);

            VSGetNextNoteCommand.RegisterCommand(Runtime);
            VSGetNextNoteExCommand.RegisterCommand(Runtime);
            
            VSUpdateNoteCommand.RegisterCommand(Runtime);
            VSInsertNoteCommand.RegisterCommand(Runtime);
            VSRemoveNoteCommand.RegisterCommand(Runtime);

            VSUpdateNoteExCommand.RegisterCommand(Runtime);
            VSInsertNoteExCommand.RegisterCommand(Runtime);

            VSDlgSetDialogTitleCommand.RegisterCommand(Runtime);
            VSDlgAddFieldCommand.RegisterCommand(Runtime);
            VSDlgDoModalCommand.RegisterCommand(Runtime);

            VSDlgGetBoolValueCommand.RegisterCommand(Runtime);
            VSDlgGetFloatValueCommand.RegisterCommand(Runtime);
            VSDlgGetIntValueCommand.RegisterCommand(Runtime);
            VSDlgGetStringValueCommand.RegisterCommand(Runtime);

            VSGetControlAtCommand.RegisterCommand(Runtime);
            VSUpdateControlAtCommand.RegisterCommand(Runtime);

            VSSeekToBeginControlCommand.RegisterCommand(Runtime);
            VSGetNextControlCommand.RegisterCommand(Runtime);
            VSUpdateControlCommand.RegisterCommand(Runtime);
            VSInsertControlCommand.RegisterCommand(Runtime);
            VSRemoveControlCommand.RegisterCommand(Runtime);
            VSGetDefaultControlValueCommand.RegisterCommand(Runtime);

            VSGetMusicalPartCommand.RegisterCommand(Runtime);
            VSUpdateMusicalPartCommand.RegisterCommand(Runtime);

            VSGetMusicalPartSingerCommand.RegisterCommand(Runtime);

            VSSeekToBeginTempoCommand.RegisterCommand(Runtime);
            VSSeekToBeginTimeSigCommand.RegisterCommand(Runtime);

            VSGetNextTempoCommand.RegisterCommand(Runtime);
            VSGetTempoAtCommand.RegisterCommand(Runtime);
            VSGetNextTimeSigCommand.RegisterCommand(Runtime);
            VSGetTimeSigAtCommand.RegisterCommand(Runtime);

            VSGetSequenceNameCommand.RegisterCommand(Runtime);
            VSGetSequencePathCommand.RegisterCommand(Runtime);
            VSGetResolutionCommand.RegisterCommand(Runtime);
        }

        public void Dispose()
        {
            Runtime.Dispose();
        }
    }
}

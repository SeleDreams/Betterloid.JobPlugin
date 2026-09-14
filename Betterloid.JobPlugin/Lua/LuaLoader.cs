using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Reflection;
using Eluant;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JobPlugin.Lua.Commands;
using Yamaha.VOCALOID.VLC;
using System.Text;

namespace JobPlugin.Lua
{
    public class LuaLoader : IDisposable
    {
        public LuaRuntime Runtime { get; private set; }
        public string LUA { get; set; }
        private readonly object luaLock = new object();
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
            lock (luaLock)
            {
                string folder = LUAFolder.Replace("\\", "/").Replace("\"", "\\\"");
                string file = LUAFilename.Replace("\"", "\\\"");

                using (Runtime.DoString("package.path = \"" + folder + "/?.lua;\" .. package.path"))
                {
                }

                string oldDir = Directory.GetCurrentDirectory();
                try
                {
                    Directory.SetCurrentDirectory(LUAFolder);
                    using (Runtime.DoString("dofile(\"" + file + "\")"))
                    {
                    }
                    using (Runtime.DoString(
                         "function __job_trace(e)\n" +
                         "  return tostring(e) .. \"\\n\" .. debug.traceback()\n" +
                         "end\n" +
                         "function __job_main(processParam, envParam)\n" +
                         "  local ok, res = xpcall(function()\n" +
                         "    return main(processParam, envParam)\n" +
                         "  end, __job_trace)\n" +
                         "  if not ok then error(res, 0) end\n" +
                         "  return res\n" +
                         "end\n"))
                                        {
                                        }
                }
                finally
                {
                    Directory.SetCurrentDirectory(oldDir);
                }

                ExecuteCommand.RegisterCommand(Runtime);
                PrintCommand.RegisterCommand(Runtime);
            }

        }

        public JobManifest GetManifest()
        {
            lock (luaLock)
            {
                LuaFunction manifestFn = null;
                LuaVararg callResult = null;
                LuaTable manifestTable = null;
                try
                {
                    manifestFn = (LuaFunction)Runtime.Globals["manifest"];
                    callResult = manifestFn.Call();
                    manifestTable = (LuaTable)callResult[0];
                    return new JobManifest(manifestTable);
                }
                finally
                {
                    if (manifestTable != null) manifestTable.Dispose();
                    if (callResult != null) callResult.Dispose();
                    if (manifestFn != null) manifestFn.Dispose();
                }
            }
        }

        public LuaNumber Main(ProcessParam processParam, EnvParam envParam)
        {
            lock (luaLock)
            {
                LuaFunction jobMain = null;
                LuaVararg returnValue = null;
                try
                {
                    jobMain = Runtime.Globals["__job_main"] as LuaFunction;
                    if (jobMain == null)
                    {
                        return new LuaNumber(1);
                    }

                    LuaTransparentClrObject transparentProcess = new LuaTransparentClrObject(processParam);
                    LuaTransparentClrObject transparentEnv = new LuaTransparentClrObject(envParam);
                    returnValue = jobMain.Call(transparentProcess, transparentEnv);

                    if (returnValue.Count > 0 && returnValue[0] is LuaNumber)
                    {
                        return (LuaNumber)returnValue[0];
                    }
                    return new LuaNumber(0);
                }
                catch (LuaException ex)
                {
                    Betterloid.Wrappers.MessageBoxDeliverer.GeneralError($"LUA error : {ex.Message}");
                    Debug.WriteLine("LUA main: " + ex.Message);
                    return new LuaNumber(1);
                }
                catch (InvalidOperationException ex)
                {
                    Betterloid.Wrappers.MessageBoxDeliverer.GeneralError($"ELUANT error : {ex.Message}");
                    Debug.WriteLine("ELUANT: " + ex.ToString());
                    return new LuaNumber(1);
                }
                catch (Exception ex)
                {
                    Betterloid.Wrappers.MessageBoxDeliverer.GeneralError($"CLR error : {ex.Message}");
                    Debug.WriteLine("CLR: " + ex.ToString());
                    return new LuaNumber(1);
                }
                finally
                {
                    if (returnValue != null) returnValue.Dispose();
                    if (jobMain != null) jobMain.Dispose();
                }
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
            VSGetPremeasureCommand.RegisterCommand(Runtime);

            VSSeekToBeginMonoWAVPartCommand.RegisterCommand(Runtime);
            VSSeekToBeginStereoWAVPartCommand.RegisterCommand(Runtime);
            VSGetNextMonoWAVPartCommand.RegisterCommand(Runtime);
            VSGetNextStereoWAVPartCommand.RegisterCommand(Runtime);
            VSGetStereoWavPartCommand.RegisterCommand(Runtime);
        }

        public void Dispose()
        {
            Runtime.Dispose();
        }
    }
}

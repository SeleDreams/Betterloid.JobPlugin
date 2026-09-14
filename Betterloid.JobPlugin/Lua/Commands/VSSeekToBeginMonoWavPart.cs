using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using Yamaha.VOCALOID;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginMonoWAVPartCommand
    {
        public static void VSSeekToBeginMonoWAVPart()
        {
            Debug.WriteLine("VSSeekToBeginMonoWAVPart called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;
            var mono = sequence.AudioTracks.FirstOrDefault(m => m.Name == "Stereo");
            if (mono == null)
            {
                throw new NullReferenceException("This plugin requires you to have an audio track named Mono! This track would contain your Mono audio data !");
            }
            JobPlugin.Instance.MonoSeek = 0;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action(VSSeekToBeginMonoWAVPart)))
            {
                lua.Globals["VSSeekToBeginMonoWAVPart"] = fn;
            }
        }
    }
}

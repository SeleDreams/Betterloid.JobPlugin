using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
using Yamaha.VOCALOID;

namespace JobPlugin.Lua.Commands
{
    public class VSSeekToBeginStereoWAVPartCommand
    {
        public static void VSSeekToBeginStereoWAVPart()
        {
            Debug.WriteLine("VSSeekToBeginStereoWAVPart called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;
            var stereo = sequence.AudioTracks.FirstOrDefault(m => m.Name == "Stereo");
            if (stereo == null)
            {
                throw new NullReferenceException("This plugin requires you to have an audio track named Stereo! This track would contain your Stereo audio data !");
            }
            JobPlugin.Instance.StereoSeek = 0;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action(VSSeekToBeginStereoWAVPart)))
            {
                lua.Globals["VSSeekToBeginStereoWAVPart"] = fn;
            }
        }
    }
}

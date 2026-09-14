using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using System.Linq;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.TrackEditor;

#endif
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSGetNextStereoWAVPartCommand
    {
        public static LuaVararg VSGetNextStereoWAVPart()
        {
            Debug.WriteLine("VSGetNextStereoWAVPart called");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            WIVSMSequence sequence = musicalEditor.ActiveTrack.Parent;
            WIVSMAudioTrack Stereo = sequence.AudioTracks.FirstOrDefault(n => n.Name == "Stereo");
            if (Stereo == null)
            {
                throw new NullReferenceException("This plugin requires you to have an audio track named Stereo! This track would contain your Stereo audio data !");
            }
            VSLuaWAVPart luaWAVPart = new VSLuaWAVPart();
            WIVSMAudioPart audioPart = JobPlugin.Instance.CurrentStereoWAVPart;
            var previousCurrentAudioPart = JobPlugin.Instance.CurrentStereoWAVPart;
            bool isFirstAudioPart = JobPlugin.Instance.StereoSeek == 0;
            JobPlugin.Instance.StereoSeek += 1;
            if (Stereo.Parts.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }
            audioPart = audioPart?.Next ?? (WIVSMAudioPart)Stereo.Parts.Last();
            JobPlugin.Instance.CurrentStereoWAVPart = audioPart;
            WaveFile waveFile = new WaveFile();
            if (waveFile.ReadWave(audioPart.GetWaveFilePath(), true) != WaveFileError.None)
            {
                luaWAVPart.FilePath = audioPart.GetWaveFilePath();
                luaWAVPart.Name = audioPart.GetOriginalWaveFileName();
                luaWAVPart.Comment = "";
                luaWAVPart.SampleReso = 16;
                luaWAVPart.SampleRate = waveFile.SampleRate;
#if VOCALOID6
                luaWAVPart.Channels = waveFile.ChannelCount;
#elif VOCALOID5
                luaWAVPart.Channels = waveFile.Channel;
#endif
            }
            if (previousCurrentAudioPart != null && previousCurrentAudioPart.Next == null && !isFirstAudioPart)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), luaWAVPart.ToTable() }, true);
            }
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaWAVPart.ToTable() }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextStereoWAVPart)))
            {
                lua.Globals["VSGetNextStereoWAVPart"] = fn;
            }
        }
    }
}

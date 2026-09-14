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
    public class VSGetNextMonoWAVPartCommand
    {
        public static LuaVararg VSGetNextMonoWAVPart()
        {
            Debug.WriteLine("VSGetNextMonoWAVPart called");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            WIVSMSequence sequence = musicalEditor.ActiveTrack.Parent;
            WIVSMAudioTrack mono = sequence.AudioTracks.FirstOrDefault(n => n.Name == "Mono");
            if (mono == null)
            {
                throw new NullReferenceException("This plugin requires you to have an audio track named Mono! This track would contain your Mono audio data !"); 
            }
            VSLuaWAVPart luaWAVPart = new VSLuaWAVPart();
            WIVSMAudioPart audioPart = JobPlugin.Instance.CurrentMonoWAVPart;
            var previousCurrentAudioPart = JobPlugin.Instance.CurrentMonoWAVPart;
            bool isFirstAudioPart = JobPlugin.Instance.MonoSeek == 0;
            JobPlugin.Instance.MonoSeek += 1;
            if(mono.Parts.Count == 0)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, false);
            }
            audioPart = audioPart?.Next ?? (WIVSMAudioPart)mono.Parts.Last();
            JobPlugin.Instance.CurrentMonoWAVPart = audioPart;
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
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetNextMonoWAVPart)))
            {
                lua.Globals["VSGetNextMonoWAVPart"] = fn;
            }
        }
    }
}

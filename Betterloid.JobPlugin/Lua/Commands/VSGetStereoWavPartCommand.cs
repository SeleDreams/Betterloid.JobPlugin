using System;
using System.Diagnostics;
using System.Linq;
using Eluant;
using JobPlugin.Lua.Types;

#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#elif VOCALOID6 || VOCALOID7
using Yamaha.VOCALOID;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSGetStereoWavPartCommand
    {
        private static LuaVararg VSGetStereoWavPart()
        {
            Debug.WriteLine("VSGetStereoWavPart called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            var sequence = musicalEditor.ActiveTrack.Parent;
            var stereo = sequence.AudioTracks.FirstOrDefault(m => m.Name == "Stereo");
            if (stereo == null)
            {
                throw new NullReferenceException("This plugin requires you to have an audio track named Stereo! This track would contain your stereo audio data !");
            }
            var part = stereo.Parts.FirstOrDefault();
            var wavInfo = new VSLuaWAVPart();

            if (part == null)
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0) ,wavInfo.ToTable()}, true);
            }
            WaveFile waveFile = new WaveFile();
            if (waveFile.ReadWave(part.PartFilePath, true) != WaveFileError.None)
            {
                wavInfo.FilePath = part.PartFilePath;
                wavInfo.Name = part.Name;
                wavInfo.Comment = "";
                wavInfo.SampleReso = 16;
                wavInfo.SampleRate = waveFile.SampleRate;
#if VOCALOID6 || VOCALOID7
                wavInfo.Channels = waveFile.ChannelCount;
#elif VOCALOID5
                wavInfo.Channels = waveFile.Channel;
#endif
            }

            return new LuaVararg(new LuaValue[] { new LuaNumber(1), wavInfo.ToTable() }, true);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetStereoWavPart)))
            {
                lua.Globals["VSGetStereoWAVPart"] = fn;
            }
        }
    }
}

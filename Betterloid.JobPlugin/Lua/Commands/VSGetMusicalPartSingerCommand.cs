using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VDM;
using Yamaha.VOCALOID.VSM;
using System.Diagnostics;


#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VOCALOID5.MusicalEditor;
using Yamaha.VOCALOID.VOCALOID5.AudioEffect;
using Yamaha.VOCALOID.VSStyle;
#elif VOCALOID6 || VOCALOID7
using Yamaha.VOCALOID.MusicalEditor;
using Yamaha.VOCALOID.AudioEffect;
#endif
namespace JobPlugin.Lua.Commands
{
    public class VSGetMusicalPartSingerCommand
    {
        public static LuaVararg VSGetMusicalPartSinger()
        {
            Debug.WriteLine("VSGetMusicalPartSinger called.");
            VSLuaMusicalSinger luaSinger = new VSLuaMusicalSinger();
            var part = JobPlugin.Instance.MusicalEditor.ActivePart;
            var track = JobPlugin.Instance.MusicalEditor.ActiveTrack;
            VoiceBank vb;
#if VOCALOID5
            vb = App.DatabaseManager.GetVoiceBankByCompID(part.VoiceBankID);
            WIVSMMidiEffect midiEffect = part.EffectManager.GetMidiEffectByType(VSMMidiEffectType.VoiceColor);
            int opening;
            bool success = midiEffect.GetParameterInt(midiEffect.GetParameterNameVoiceColor(VSMVoiceColorParameterType.Mouth), out opening);
            luaSinger.Opening = success ? opening : 0;
            int breathiness;
            success = midiEffect.GetParameterInt(midiEffect.GetParameterNameVoiceColor(VSMVoiceColorParameterType.Breathiness), out breathiness);
            luaSinger.Breathiness = success ? breathiness : 0;
            int brightness;
            success = midiEffect.GetParameterInt(midiEffect.GetParameterNameVoiceColor(VSMVoiceColorParameterType.Exciter), out brightness);
            luaSinger.Brightness = success ? brightness : 0;
            int genderFactor;
            success = midiEffect.GetParameterInt(midiEffect.GetParameterNameVoiceColor(VSMVoiceColorParameterType.Character), out genderFactor);
            luaSinger.GenderFactor = success ? genderFactor : 0;
            int clearness;
            success = midiEffect.GetParameterInt(midiEffect.GetParameterNameVoiceColor(VSMVoiceColorParameterType.Air), out clearness);
            luaSinger.Clearness = success ? 127 - clearness : 0;
            luaSinger.VirtualBankSelect = vb.LangID;
#elif VOCALOID6 || VOCALOID7
            vb = part.VoiceBank();
            WIVSMVoiceColorEffect midiEffect;
#if VOCALOID6
            midiEffect = part.VoiceColorEffect;
#elif VOCALOID7
            midiEffect = track.VoiceColorEffect;
#endif
            luaSinger.Opening = midiEffect.GetValueByIndex((ulong)VSMVoiceColor.Mouth).RawInt;
            luaSinger.Breathiness = midiEffect.GetValueByIndex((ulong)VSMVoiceColor.Breathiness).RawInt;
            luaSinger.GenderFactor = midiEffect.GetValueByIndex((ulong)VSMVoiceColor.Character).RawInt;
            luaSinger.Brightness = midiEffect.GetValueByIndex((ulong)VSMVoiceColor.Exciter).RawInt;
            luaSinger.Clearness = 127 - midiEffect.GetValueByIndex((ulong)VSMVoiceColor.Air).RawInt;
            luaSinger.VirtualBankSelect = vb.NativeLangID;
            luaSinger.VirtualProgramChange = vb.SingerID;
#endif

            luaSinger.ComponentID = vb.CompID;
            luaSinger.VirtualProgramChange = vb.SingerID;
            luaSinger.ObjID = (ulong)vb.SingerID;
            return new LuaVararg(new LuaValue[] { new LuaNumber(1), luaSinger.ToTable() }, false);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaVararg>(VSGetMusicalPartSinger)))
            {
                lua.Globals["VSGetMusicalPartSinger"] = fn;
            }
        }
    }
}

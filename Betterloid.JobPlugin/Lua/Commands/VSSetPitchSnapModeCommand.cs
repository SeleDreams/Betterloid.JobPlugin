using Betterloid.Wrappers;
using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID;

#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.VOCALOID5.StyleEditor;
using APP = Yamaha.VOCALOID.VOCALOID5.App;
#elif VOCALOID6
using APP = Yamaha.VOCALOID.App;
#endif

using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public class VSSetPitchSnapModeCommand
    {
        public static LuaNumber VSSetPitchSnapMode(double value)
        {
            Debug.WriteLine("VSSetPitchSnapMode called.");
            var musicalEditor = JobPlugin.Instance.MusicalEditor;
            bool mode = value != 0;
            var part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            var effect = part.EffectManager.GetMidiEffectByType(VSMMidiEffectType.RobotVoice);
            if (effect == null)
            {
                return new LuaNumber(0);
            }
#if VOCALOID5
            effect.IsBypassed = !mode;
            
            if (mode)
            {
                string modeName = effect.GetModeNameRobotVoice(VSMRobotVoiceModeType.Hard);
                WIVSMEffectValue modeValue = effect.GetValueByName("Mode");
                if (modeValue == null)
                {
                    return new LuaNumber(0);
                }
                modeValue.RawInt = (int)VSMRobotVoiceModeType.Hard;
                effect.ScriptFilePath = APP.LuaManager.GetRobotVoiceScriptPath(VSMRobotVoiceModeType.Hard);
                effect.Timing = VSMMidiEffectTiming.BeforeRendering;
            }
#elif VOCALOID6
            effect.SetBypass(!mode);

            if (mode)
            {
                WIVSMEffectValue modeValue = effect.GetValueByName("Mode");
                if (modeValue == null)
                {
                    return new LuaNumber(0);
                }
                modeValue.SetRawInt((int)VSMRobotVoiceMode.Hard);
                effect.ScriptFilePath = APP.LuaManager.GetRobotVoiceScriptPath(VSMRobotVoiceMode.Hard);
                effect.Timing = VSMMidiEffectTiming.BeforeRendering;
            }
#endif
            return new LuaNumber(1);
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<double,LuaNumber>(VSSetPitchSnapMode)))
            {
                lua.Globals["VSSetPitchSnapMode"] = fn;
            }
        }
    }
}

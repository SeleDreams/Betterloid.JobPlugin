using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eluant;
#if VOCALOID6
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VAE;
#elif VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
using Yamaha.VOCALOID.AudioEngine;
#endif
namespace JobPlugin.Lua.Commands
{
    public static class VSGetAudioDeviceNameCommand
    {
        public static string VSGetAudioDeviceName()
        {
            var audioDeviceManager = App.AudioPlayer.AudioDeviceManager;
            VEAudioDevice currentAudioDevice;
#if VOCALOID5
            currentAudioDevice = audioDeviceManager.GetCurrentAudioDevice();
#elif VOCALOID6
            currentAudioDevice = audioDeviceManager.CurrentAudioDevice;
#endif
            return currentAudioDevice.DisplayName();
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string>(VSGetAudioDeviceName)))
            {
                lua.Globals["VSGetAudioDeviceName"] = fn;
            }
        }
    }
}

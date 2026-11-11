using Eluant;
using JobPlugin.Lua.Types;
using System;
using System.Diagnostics;
using Yamaha.VOCALOID.VSM;

namespace JobPlugin.Lua.Commands
{
    public static class VSUpdateControlCommand
    {
        
        private static int VSUpdateControl(LuaTable table)
        {
            
            VSLuaControl control = new VSLuaControl(table);
            Debug.WriteLine($"VSUpdateControl called with args : type={control.Type}, posTick={control.PosTick},value={control.Value}.");
            WIVSMMidiPart part = JobPlugin.Instance.MusicalEditor.ActivePart ?? throw new NoActivePartException();
            VSControlType controlType = control.Type;
            VSMControllerType vsmControlType;
            try
            {
                vsmControlType = VSLuaControl.VSControlTypeToVSMControllerType(controlType);
            }
            catch
            {
                return 1;
            }
            var controller = (JobPlugin.Instance.Controllers[controlType])[(int)control.ObjID];
            VSMRelTick ticks;
#if VOCALOID6 || VOCALOID7
            ticks = new VSMRelTick(control.PosTick);
#elif VOCALOID5
            ticks = new VSMRelTick((int)control.PosTick);
#endif
            if (controller == null)
            {
                part.InsertController(ticks, vsmControlType, control.Value);
                JobPlugin.Instance.Modified = true;
                return 1;
            }
            else
            {
                controller.Value = control.Value;
                part.MoveController(new VSMRelTick((int)control.PosTick),controller);
                JobPlugin.Instance.Modified = true;
                return 1;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<LuaTable,int>(VSUpdateControl)))
            {
                lua.Globals["VSUpdateControl"] = fn;
            }
        }
    }
}

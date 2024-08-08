using Eluant;
using System;
using System.Security.Permissions;
using Yamaha.VOCALOID;
using Yamaha.VOCALOID.VSM;
#if VOCALOID5
using Yamaha.VOCALOID.VOCALOID5;
#endif

namespace JobPlugin.Lua.Types
{
   
    public enum VSControlType
    {
        DYN, // Dynamics
        BRE, // Breathiness
        BRI, // Brightness
        CLE, // Clearness
        GEN, // Gender factor
        PIT, // Pitch bend
        PBS, // Pitch bend sensitivity
        POR, // Portamento timing
        GWL // Growl
    }
    public class VSLuaControl
    {
        public static ControlParameterTypeEnum VSControlTypeToControlParameterTypeEnum(VSControlType type)
        {
            switch (type)
            {
                case VSControlType.PIT:
                    return ControlParameterTypeEnum.PitchBend;
                case VSControlType.POR:
                    return ControlParameterTypeEnum.PortamentoTiming;
                case VSControlType.PBS:
                    return ControlParameterTypeEnum.PitchBendSensitivity;
                case VSControlType.DYN:
                    return ControlParameterTypeEnum.Dynamics;
                case VSControlType.CLE:
                    return ControlParameterTypeEnum.Clearness;
                case VSControlType.BRI:
                    return ControlParameterTypeEnum.Brightness;
                case VSControlType.GEN:
                    return ControlParameterTypeEnum.Character;
                case VSControlType.GWL:
                    return ControlParameterTypeEnum.Growl;
                default:
                    throw new InvalidOperationException();
            }
        }
        public static VSMControllerType VSControlTypeToVSMControllerType(VSControlType type)
        {
            switch (type)
            {
                case VSControlType.PIT:
                    return VSMControllerType.PitchBend;
                case VSControlType.POR:
                    return VSMControllerType.Portamento;
                case VSControlType.PBS:
                    return VSMControllerType.PitchBendSens;
                case VSControlType.DYN:
                    return VSMControllerType.Dynamics;
                case VSControlType.CLE:
                    return VSMControllerType.Clearness;
                case VSControlType.BRI:
                    return VSMControllerType.Brightness;
                case VSControlType.GEN:
                    return VSMControllerType.Character;
                case VSControlType.GWL:
                    return VSMControllerType.Growl;
                default:
                    throw new InvalidOperationException();
            }
        }
        public VSLuaControl(long posTick = 0, int value = 0, VSControlType type = VSControlType.BRE, ulong id = 0)
        {
            PosTick = posTick;
            Value = value;
            Type = type;
            ObjID = id;
        }
        public VSLuaControl(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            Value = (int)table["value"].ToNumber();
            Type = (VSControlType)Enum.Parse(typeof(VSControlType), table["value"].ToString());
            if (table.ContainsKey("objID"))
            {
                ObjID = (ulong)table["objID"].ToNumber();
            }
            
        }
        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["posTick"] = new LuaNumber(PosTick);
            table["value"] = new LuaNumber(Value);
            table["type"] = new LuaString(Type.ToString());
            table["objID"] = new LuaNumber(ObjID);
            return table;
        }
        public long PosTick { get; set; }
        public int Value { get; set; }
        public VSControlType Type { get; set; }
        public ulong ObjID { get; set; }
    }
}

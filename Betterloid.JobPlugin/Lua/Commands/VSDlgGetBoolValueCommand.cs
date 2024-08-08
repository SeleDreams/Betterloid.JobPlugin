using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgGetBoolValueCommand
    {
        public static LuaVararg VSDlgGetBoolValue(string fieldName)
        {
            try
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaNumber(JobPlugin.Instance.VSDialog.GetBoolField(fieldName) ? 1 : 0)}, true);
            }
            catch
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, true );
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,LuaVararg>(VSDlgGetBoolValue)))
            {
                lua.Globals["VSDlgGetBoolValue"] = fn;
            }
        }
    }
}

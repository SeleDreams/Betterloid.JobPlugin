using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgGetStringValueCommand
    {
        public static LuaVararg VSDlgGetStringValue(string fieldName)
        {
            try
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaString(JobPlugin.Instance.VSDialog.GetTextField(fieldName))}, true);
            }
            catch
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), new LuaString("") }, true );
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,LuaVararg>(VSDlgGetStringValue)))
            {
                lua.Globals["VSDlgGetStringValue"] = fn;
            }
        }
    }
}

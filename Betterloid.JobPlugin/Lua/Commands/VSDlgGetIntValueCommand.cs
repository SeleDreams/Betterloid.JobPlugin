using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgGetIntValueCommand
    {
        public static LuaVararg VSDlgGetIntValue(string fieldName)
        {
            try
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(1), new LuaNumber(JobPlugin.Instance.VSDialog.GetIntField(fieldName))}, true);
            }
            catch
            {
                return new LuaVararg(new LuaValue[] { new LuaNumber(0), LuaNil.Instance }, true );
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<string,LuaVararg>(VSDlgGetIntValue)))
            {
                lua.Globals["VSDlgGetIntValue"] = fn;
            }
        }
    }
}

using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgDoModalCommand
    {
        public static int VSDlgDoModal()
        {
            bool result = JobPlugin.Instance.VSDialog.DoModal();
            if (result)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Func<int>(VSDlgDoModal)))
            {
                lua.Globals["VSDlgDoModal"] = fn;
            }
        }
    }
}

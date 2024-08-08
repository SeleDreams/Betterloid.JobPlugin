using System;
using Eluant;

namespace JobPlugin.Lua.Commands
{
    public class VSDlgSetDialogTitleCommand
    {
        public static void VSDlgSetDialogTitle(string title)
        {
            JobPlugin.Instance.VSDialog.Title = title;
        }

        public static void RegisterCommand(LuaRuntime lua)
        {
            using (var fn = lua.CreateFunctionFromDelegate(new Action<string>(VSDlgSetDialogTitle)))
            {
                lua.Globals["VSDlgSetDialogTitle"] = fn;
            }
        }
    }
}

using Eluant;

namespace JobPlugin.Lua.Types
{

    public class EnvParam
    {
        public string ScriptDir;

        public string ScriptName { get; set; }

        public string TempDir { get; set; }

        public string ApiVersion { get; set; }

        public LuaTable ToTable()
        {
            var lua = JobPlugin.Instance.Lua;
            LuaTable table = lua.Runtime.CreateTable();
            table["scriptDir"] = new LuaString(ScriptDir);
            table["scriptName"] = new LuaString(ScriptName);
            table["tempDir"] = new LuaString(TempDir);
            table["apiVersion"] = new LuaString(ApiVersion);
            return table;
        }
    }
}

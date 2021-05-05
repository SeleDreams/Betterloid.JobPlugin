using Eluant;
using System.Diagnostics;

namespace JobPlugin
{
    public class JobManifest
    {
        public JobManifest(LuaTable table)
        {
            Name = table["name"].ToString();
            Comment = table["comment"].ToString();
            Author = table["author"].ToString();
            PluginID = table["pluginID"].ToString();
            PluginVersion = table["pluginVersion"].ToString();
            ApiVersion = table["apiVersion"].ToString();
        }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public string PluginID { get; set; }

        public string PluginVersion { get; set; }

        public string ApiVersion { get; set; }
    }
}

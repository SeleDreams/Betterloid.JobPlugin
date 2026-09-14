using Eluant;

namespace JobPlugin.Lua.Types
{
    public class VSLuaWAVPart
    {
        public long PosTick { get; set; }
        public long PlayTime { get; set; }
        public int SampleRate { get; set; }
        public int SampleReso { get; set; }
        public int Channels { get; set; }
        public string Name { get; set; } = "";
        public string Comment { get; set; } = "";
        public string FilePath { get; set; } = "";

        public VSLuaWAVPart() { }
        public VSLuaWAVPart(LuaTable table)
        {
            PosTick = (long)table["posTick"].ToNumber();
            PlayTime = (long)table["playTime"].ToNumber();
            SampleRate = (int)table["sampleRate"].ToNumber();
            SampleReso = (int)table["sampleReso"].ToNumber();
            Channels = (int)table["channels"].ToNumber();
            Name = table["name"].ToString();
            Comment = table["comment"].ToString();
            FilePath = table["filePath"].ToString();

        }

        public LuaTable ToTable()
        {
            LuaTable table = JobPlugin.Instance.Lua.Runtime.CreateTable();
            table["posTick"] = new LuaNumber(PosTick);
            table["playTime"] = new LuaNumber(PlayTime);
            table["sampleRate"] = new LuaNumber(SampleRate);
            table["sampleReso"] = new LuaNumber(SampleReso);
            table["channels"] = new LuaNumber(Channels);
            table["name"] = new LuaString(Name);
            table["comment"] = new LuaString(Comment);
            table["filePath"] = new LuaString(FilePath);
            return table;
        }
    }
}

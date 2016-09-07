using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.OoA
{
    public static class DefaultInfo
    {
        public static InfoList Pointers = new InfoList
        {
            //0x0XXX pointers
            new Info("Sword", "byte", 0, 0x6B2),
            new Info("D1Ess", "byte", 0, 0x911),
            new Info("D2Ess", "byte", 0, 0x938),
            new Info("D3Ess", "byte", 0, 0x949),
            new Info("D6Ess", "byte", 0, 0xA37),
            new Info("D1Enter", "byte", 0, 0x924),
            new Info("D2Enter", "byte", 0, 0x946),
            new Info("D3Enter", "byte", 0, 0x966),
            new Info("D4Enter", "byte", 0, 0x991),
            new Info("D5Enter", "byte", 0, 0x9BB),
            new Info("D6Enter", "byte", 0, 0xA44),
            new Info("VeranEnter", "byte", 0, 0x9D4),

            new Info("SeedSatchel", "byte", 0, 0x738),
            new Info("Harp1", "byte", 0, 0x8AE),
            new Info("Feather", "byte", 0, 0x928),
            new Info("D2Skip", "byte", 0, 0x82E),
            new Info("Raft", "byte", 0, 0x6A4),
            new Info("CrescentIsland", "byte", 0, 0x8AA),
            new Info("SeedShooter", "byte", 0, 0x958),
            new Info("Flute", "byte", 0, 0x6B5),
            new Info("Harp2", "byte", 0, 0x88F),
            new Info("Cane", "byte", 0, 0x9A5),
            new Info("TuniNut", "byte", 0, 0x6C2),
            new Info("GreatMoblin", "byte", 0, 0x709),
            new Info("SwitchHook", "byte", 0, 0x987),
            new Info("LavaJuice", "byte", 0, 0x8E7),
            new Info("MermaidSuit", "byte", 0, 0xA13),
            new Info("D6BossKey", "byte", 0, 0xA1C),

            new Info("VersionCheck", "byte", 0, 0x2FD),
            new Info("FileSelect1", "byte", 0, 0xB00),
            new Info("FileSelect2", "short", 0, 0xBB3),
            new Info("ResetCheck", "byte", 0, 0),

            //0x1XXX pointers
            new Info("VeranHP", "byte", 1, 0x0A9),
            new Info("NayruHP", "byte", 1, 0x0B3),
            new Info("BossCycle", "byte", 1, 0x084),
        };

        public static InfoList BaseSplits = new InfoList
        {
            //dungeon entrances
            new Info("ED1", "D1Enter", 0x10),
            new Info("ED2", "D2Enter", 0x10),
            new Info("ED3", "D3Enter", 0x10),
            new Info("ED4", "D4Enter", 0x10),
            new Info("ED5", "D5Enter", 0x10),
            new Info("ED6", "D6Enter", 0x10),
            
            //essences
            new Info("D1", "D1Ess", 0x30),
            new Info("D2", "D2Ess", 0x30),
            new Info("D3", "D3Ess", 0x30),
            new Info("D6", "D6Ess", 0x30),

            //items
            new Info("L1Sword", "Sword", 1),
            new Info("D0T", "SeedSatchel", 0xB0),
            new Info("D2T", "Feather", 0x30),
            new Info("D3T", "SeedShooter", 0x30),
            new Info("D4T", "SwitchHook", 0x34),
            new Info("D5T", "Cane", 0x30),
            new Info("D6T", "MermaidSuit", 0x30),
            new Info("D6BK", "D6BossKey", 0x30),
            new Info("FluteR", "Flute", 0x01),
            new Info("FluteD", "Flute", 0x02),
            new Info("FluteM", "Flute", 0x03),
            new Info("Harp1", "Harp1", 0x30),
            new Info("Harp2", "Harp2", 0x30),
            new Info("Rope", "Raft", 0x04),
            new Info("Chart", "Raft", 0x10),
            new Info("TN", "TuniNut", 0x02),
            new Info("LJ", "LavaJuice", 0x30),

            //others
            new Info("D2Skip", "D2Skip", 0x10),
            new Info("CI", "CrescentIsland", 0x10),
            new Info("Moblin", "GreatMoblin", 0x11),
            new Info("Nayru", new Dictionary<string,int> { { "NayruHP", 1 }, { "BossCycle", 0x12 } }),
            new Info("EVeran", "VeranEnter", 0x10),
            new Info("Veran", new Dictionary<string,int> { { "VeranHP", 1 }, { "VeranEnter", 0x10 } }),
        };
    }

    public class Info
    {
        public string Name { get; }

        public string Type { get; }
        public int Index { get; }
        public int Offset { get; }

        public Dictionary<string, int> Triggers { get; }

        public bool isEnabled { get; set; }

        //pointer
        public Info(string _name, string _type, int _index, int _offset)
        {
            Name = _name;
            Type = _type;
            Index = _index;
            Offset = _offset;
        }

        //split
        public Info(string _name, string _pointer, int _condition)
        {
            Name = _name;
            Triggers = new Dictionary<string, int> { { _pointer, _condition } };
        }

        //split
        public Info(string _name, Dictionary<string, int> _triggers)
        {
            Name = _name;
            Triggers = _triggers;
        }

        //settings
        public Info(string _name, bool _enabled)
        {
            Name = _name;
            isEnabled = _enabled;
        }
    }

    public class InfoList : List<Info>
    {
        public Info this[string name]
        {
            get { return this.First(w => w.Name == name); }
        }
    }
}

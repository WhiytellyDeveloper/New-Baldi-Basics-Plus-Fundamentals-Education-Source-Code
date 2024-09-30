using MTM101BaldAPI.AssetTools;
using System.Collections.Generic;
using System.IO;

namespace nbbpfe.FundamentalsManager
{
    public static class Paths
    {
        public static Dictionary<PathsEnum, string> paths = [];

        public static void Initialize()
        {
            paths.Add(PathsEnum.Misc, "Misc");
            paths.Add(PathsEnum.Rooms, "Rooms");
            paths.Add(PathsEnum.Items, "Items");
            paths.Add(PathsEnum.NPCs, "NPCs");
        }

        public static string GetPath(PathsEnum path, params string[] strings) {
            return Path.Combine(AssetLoader.GetModPath(BasePlugin.Instance), paths[path], Path.Combine(strings));
        }

    }

    public enum PathsEnum
    {
        Misc,
        Rooms,
        Items,
        NPCs,
        Events,
        Structures
    }
}

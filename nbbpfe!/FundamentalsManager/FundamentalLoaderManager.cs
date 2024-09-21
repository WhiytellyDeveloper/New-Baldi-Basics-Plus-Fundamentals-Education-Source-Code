using nbbpfe.FundamentalsManager.Loaders;
using System.Collections.Generic;

namespace nbbpfe.FundamentalsManager
{
    public static class FundamentalLoaderManager
    {
        public static void Initialize()
        {
            RoomsLoader.LoadRooms();
        }

        public static List<FloorData> floors = new List<FloorData>{ 
            new FloorData("F1"),
            new FloorData("F2"),
            new FloorData("F3"),
            new FloorData("F4"),
            new FloorData("END")
        };

        public class FloorData(string floor = "None")
        {
            public string Floor => _floor;
            readonly string _floor = floor;

            public Dictionary<RoomTextures, List<WeightedTexture2D>> wallTextures = new Dictionary<RoomTextures, List<WeightedTexture2D>>{
                { RoomTextures.Hall, new List<WeightedTexture2D>() },
                { RoomTextures.Class, new List<WeightedTexture2D>() },
                { RoomTextures.Faculty, new List<WeightedTexture2D>() }
            };

            public Dictionary<RoomTextures, List<WeightedTexture2D>> floorTextures = new Dictionary<RoomTextures, List<WeightedTexture2D>>{
                { RoomTextures.Hall, new List<WeightedTexture2D>() },
                { RoomTextures.Class, new List<WeightedTexture2D>() },
                { RoomTextures.Faculty, new List<WeightedTexture2D>() }
            };

            public Dictionary<RoomTextures, List<WeightedTexture2D>> ceilingTextures = new Dictionary<RoomTextures, List<WeightedTexture2D>>{
                { RoomTextures.Hall, new List<WeightedTexture2D>() },
                { RoomTextures.Class, new List<WeightedTexture2D>() },
                { RoomTextures.Faculty, new List<WeightedTexture2D>() }
            };
        }
    }

    public enum RoomTextures
    {
        Hall,
        Class,
        Faculty,
        Storage
    }
}

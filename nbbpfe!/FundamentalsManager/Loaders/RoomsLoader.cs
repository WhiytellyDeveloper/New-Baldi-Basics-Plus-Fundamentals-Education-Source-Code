using MTM101BaldAPI.AssetTools;
using nbbpfe.BasicClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using nbbpfe.FundamentalsManager;
using UnityEngine;
using Newtonsoft.Json;

namespace nbbpfe.FundamentalsManager.Loaders
{
    public static partial class RoomsLoader
    {
        public static void LoadRooms()
        {
            LoadTextures(RoomTextures.Hall, "Hall");
            LoadTextures(RoomTextures.Class, "Class");
            LoadTextures(RoomTextures.Faculty, "Faculty");
        }

        public static void LoadTextures(RoomTextures roomTextures, string roomPath)
        {
            string[] textures = Directory.GetFiles(Paths.GetPath(PathsEnum.Rooms, roomPath), "*.png", SearchOption.AllDirectories);
            RoomTextureData data = JsonConvert.DeserializeObject<RoomTextureData>(File.ReadAllText(Paths.GetPath(PathsEnum.Rooms, roomPath, "Textures.data")));
            var wallSelections = data.wallTextures.Select(w => w.selection).ToList();
            var floorSelections = data.floorTextures.Select(f => f.selection).ToList();
            var ceilingSelections = data.ceilingTextures.Select(c => c.selection).ToList();

            foreach (string file in textures)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                foreach (var floor in FundamentalLoaderManager.floors)
                {
                    if (!file.Contains(floor.Floor + "!")) continue;

                    WeightedTexture2D weightedTexture = new WeightedTexture2D
                    {
                        selection = AssetLoader.TextureFromFile(Paths.GetPath(PathsEnum.Rooms, new[] { roomPath, file })),
                        weight = wallSelections.Contains(fileName) ? data.wallTextures.FirstOrDefault(w => w.selection == fileName)?.weight ?? 0 :
                                floorSelections.Contains(fileName) ? data.floorTextures.FirstOrDefault(f => f.selection == fileName)?.weight ?? 0 :
                                ceilingSelections.Contains(fileName) ? data.ceilingTextures.FirstOrDefault(c => c.selection == fileName)?.weight ?? 0 : 0
                    };

                    if (wallSelections.Contains(fileName))
                        floor.wallTextures[roomTextures].Add(weightedTexture);
                    else if (floorSelections.Contains(fileName))
                        floor.floorTextures[roomTextures].Add(weightedTexture);
                    else if (ceilingSelections.Contains(fileName))
                        floor.ceilingTextures[roomTextures].Add(weightedTexture);
                }
            }
            Debug.Log($"Finished loading {roomPath} textures.");
        }


    }

    [Serializable]
    public class RoomTextureData {
        public List<WeightedStringFile> wallTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
        public List<WeightedStringFile> floorTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
        public List<WeightedStringFile> ceilingTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
    }
}

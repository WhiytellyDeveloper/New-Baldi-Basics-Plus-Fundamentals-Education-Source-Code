using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using EditorCustomRooms;
using MTM101BaldAPI;
using PixelInternalAPI.Extensions;
using PlusLevelLoader;
using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.BasicClasses;
using nbppfe.Enums;
using nbppfe.BasicClasses.Functions;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class RoomsLoader
    {
        public static void LoadRooms()
        {
            var light1 = Resources.FindObjectsOfTypeAll<Transform>().Where(x => x.name == "HangingLight").FirstOrDefault();

            LoadTextures(RoomTextures.Hall, "Hall");
            LoadTextures(RoomTextures.Class, "Class");
            LoadTextures(RoomTextures.Faculty, "Faculty");

            var faculty = Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.name.Contains("Faculty")).First();
            var container = faculty.roomFunctionContainer.DuplicatePrefab();
            var function = container.gameObject.AddComponent<CopyCastTexturesFunction>();
            function.category = RoomCategory.Faculty;
            container.AddFunction(function);
            var faculyts = CreateRooms("EmellyGutterFacutly", faculty.maxItemValue, faculty.offLimits, container, false, false, (Texture2D)faculty.mapMaterial.GetTexture("_MapBackground"), false);

            PlusLevelLoaderPlugin.Instance.roomSettings.Add("CheapStore", new RoomSettings(
                 CustomRoomsEnum.Storage.ToRoomEnum(),
                 RoomType.Room,
                 Color.black,
                 Resources.FindObjectsOfTypeAll<StandardDoorMats>().Last()
             ));

            AssetsLoader.CreateTexture("CheapStoreCounterAtlas", Paths.GetPath(PathsEnum.Rooms, "CheapStore"));
            var cheapContainer = new GameObject("CheapStoreContainer").AddComponent<RoomFunctionContainer>();
            cheapContainer.ReflectionSetVariable("functions", new List<RoomFunction>());
            cheapContainer.AddFunction(cheapContainer.gameObject.AddComponent<CheapShopFunction>());
            var cheapSwingDoorFunction = cheapContainer.gameObject.AddComponent<SpecialRoomSwingingDoorsBuilder>();
            cheapSwingDoorFunction.ReflectionSetVariable("swingDoorPre", Resources.FindObjectsOfTypeAll<SwingDoor>().Where(x => x.name.Contains("Door_Auto")).First());
            cheapContainer.AddFunction(cheapSwingDoorFunction);
            cheapContainer.gameObject.ConvertToPrefab(true);

            PlusLevelLoaderPlugin.Instance.roomSettings["CheapStore"].container = cheapContainer;
            PlusLevelLoaderPlugin.Instance.textureAliases.Add("CheapWall", AssetsLoader.CreateTexture("CheapStoreWall", Paths.GetPath(PathsEnum.Rooms, "CheapStore")));
            PlusLevelLoaderPlugin.Instance.textureAliases.Add("CheapFloor", AssetsLoader.CreateTexture("CheapStoreFloor", Paths.GetPath(PathsEnum.Rooms, "CheapStore")));
            PlusLevelLoaderPlugin.Instance.textureAliases.Add("CheapCeiling", AssetsLoader.CreateTexture("CheapStoreCeiling", Paths.GetPath(PathsEnum.Rooms, "CheapStore")));

            Dictionary<string, RoomAsset> cheapStores = CreateRooms("CheapStore", 0, true, cheapContainer, false, false, null, false, [PlusLevelLoaderPlugin.Instance.textureAliases["CheapWall"], PlusLevelLoaderPlugin.Instance.textureAliases["CheapFloor"], PlusLevelLoaderPlugin.Instance.textureAliases["CheapCeiling"]]);
            //CheapStore1

            AddGroupToFloor(floor: ["F2", "F4"], name: "Cheap Stores", min: 1, max: 1, sticky: 1,
                rooms: [cheapStores["CheapStore1"].ToWeighted<WeightedRoomAsset, RoomAsset>(100)],
                lights: [light1.ToWeighted<WeightedTransform, Transform>(100)],
                walls: [PlusLevelLoaderPlugin.Instance.textureAliases["CheapWall"].ToWeighted<WeightedTexture2D, Texture2D>(100)],
                floors: [PlusLevelLoaderPlugin.Instance.textureAliases["CheapFloor"].ToWeighted<WeightedTexture2D, Texture2D>(100)],
                ceilings: [PlusLevelLoaderPlugin.Instance.textureAliases["CheapCeiling"].ToWeighted<WeightedTexture2D, Texture2D>(100)]);
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
                    if (!fileName.Contains(floor.Floor + "!")) continue;

                    WeightedTexture2D weightedTexture = new WeightedTexture2D
                    {
                        selection = AssetsLoader.CreateTexture(fileName, Paths.GetPath(PathsEnum.Rooms, new[] { roomPath })),
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

        private static Dictionary<string, RoomAsset> CreateRooms(string path, int maxValue, bool isOffLimits = false, RoomFunctionContainer cont = null, bool isAHallway = false, bool secretRoom = false, Texture2D mapBg = null, bool keepTextures = true, Texture2D[] roomsTextures = null, WeightedPosterObject[] posters = null, float posterChance = 0, bool squaredShape = false)
        {
            Dictionary<string, RoomAsset> assets = [];
            RoomFunctionContainer container = cont;

            foreach (var file in Directory.GetFiles(Paths.GetPath(PathsEnum.Rooms, path), "*.cbld"))
            {
                if (File.ReadAllBytes(file).Length == 0) continue;


                Debug.LogWarning(file);
                var asset = RoomFactory.CreateAssetsFromPath(file, maxValue, isOffLimits, container, isAHallway, secretRoom, mapBg, keepTextures, squaredShape);
                foreach (var room in asset)
                {
                    if (roomsTextures != null)
                    {
                        room.wallTex = roomsTextures[0];
                        room.florTex = roomsTextures[1];
                        room.ceilTex = roomsTextures[2];
                    }
                    if (posters != null)
                        room.posters = posters.ToList();

                    room.posterChance = posterChance;

                    string roomName = Path.GetFileNameWithoutExtension(file);
                    Debug.LogWarning(roomName);
                    if (!assets.ContainsKey(roomName))
                    {
                        assets.Add(roomName, room);
                        AssetsLoader.assetMan.Add(roomName, room);
                    }

                }


            }

            return assets;
        }

        public static void AddGroupToFloor(string[] floor, string name, int min, int max, int sticky, WeightedRoomAsset[] rooms, WeightedTransform[] lights, WeightedTexture2D[] walls, WeightedTexture2D[] floors, WeightedTexture2D[] ceilings)
        {
            foreach (string _floor in floor)
            {
                FundamentalLoaderManager.GetFloorByName(_floor).roomGroups = new List<RoomGroup>
                {
                    new RoomGroup{
                         name = name,
                         minRooms = min,
                         maxRooms = max,
                         stickToHallChance = sticky,
                         potentialRooms = rooms,
                         light = lights,
                         wallTexture = walls,
                         floorTexture = floors,
                         ceilingTexture = ceilings
                    }
                };

            }
        }
    }

    [Serializable]
    public class RoomTextureData
    {
        public List<WeightedStringFile> wallTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
        public List<WeightedStringFile> floorTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
        public List<WeightedStringFile> ceilingTextures = new List<WeightedStringFile> { new WeightedStringFile { selection = "None", weight = 0 } };
    }
}

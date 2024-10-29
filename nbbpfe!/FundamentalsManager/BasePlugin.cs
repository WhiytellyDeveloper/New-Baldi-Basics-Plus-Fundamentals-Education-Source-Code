using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using static nbppfe.FundamentalsManager.FundamentalLoaderManager;
using System.Linq;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using nbppfe.BasicClasses.Functions;
using nbppfe.CustomData;
using BepInEx.Configuration;

namespace nbppfe.FundamentalsManager
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", MTM101BaldiDevAPI.VersionNumber)]
    [BepInProcess("BALDI.exe")]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; private set; }
        public Configs configs = new Configs();

        private void Awake()
        {
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            Instance = this;

            configs.disablelobby = Config.Bind("Floors Settings", "Disable Initial Lobby", false, "When activating, disable the initial lobby that is executed before f1 or f0 if activated. (Making this decision will disable a lot of the mod's content.)");

            harmony.PatchAllConditionals();
            Debug.Log("Thx for playing ;)");

            LoadingEvents.RegisterOnAssetsLoaded(Info, PreLoading(), false);
            LoadingEvents.RegisterOnAssetsLoaded(Info, PostLoading, true);
            ModdedSaveGame.AddSaveHandler(Info);


            GeneratorManagement.Register(this, GenerationModType.Base, (floorName, floorNum, ld) =>
            {
                var f1 = Resources.FindObjectsOfTypeAll<LevelObject>().Where(x => x.name == "Main1").First();
                ld.standardDoorMat = backDoor;
                switch (floorName)
                {
                    case "F1":
                        ld.roomGroup.First(x => x.name == "Class").maxRooms = 5;
                        ld.roomGroup.First(x => x.name == "Class").minRooms = 5;
                        ld.maxEvents += 1;
                        ld.minSpecialBuilders += 1;
                        ld.maxSpecialBuilders += 2;
                        ld.additionalNPCs += 3;
                        ld.maxItemValue = 130;
                        List<WeightedNPC> npcsToRemove = new List<WeightedNPC>();

                        foreach (WeightedNPC npc in ld.potentialNPCs)
                        {
                            if (npc.selection.Character == Character.Sweep)
                                npcsToRemove.Add(npc);
                        }

                        foreach (WeightedNPC npc in npcsToRemove)
                            ld.potentialNPCs.Remove(npc);

                        ld.forcedNpcs = ld.forcedNpcs.AddItem(NPCMetaStorage.Instance.Get(Character.Sweep).value).ToArray();

                        break;
                    case "F2":
                        ld.minEvents += 1;
                        ld.maxEvents += 2;
                        ld.minSpecialBuilders += 2;
                        ld.maxSpecialBuilders += 3;
                        ld.additionalNPCs += 5;
                        ld.maxItemValue /= 2;
                        List<WeightedNPC> _npcsToRemove = new List<WeightedNPC>();

                        foreach (WeightedNPC npc in ld.potentialNPCs)
                        {
                            if (npc.selection.Character == Character.Sweep || npc.selection.Character == Character.DrReflex)
                                _npcsToRemove.Add(npc);
                        }

                        foreach (WeightedNPC npc in _npcsToRemove)
                            ld.potentialNPCs.Remove(npc);

                        ld.forcedNpcs = ld.forcedNpcs.AddItem(NPCMetaStorage.Instance.Get(Character.DrReflex).value).ToArray();

                        ld.roomGroup.First(x => x.name == "Class").stickToHallChance = 0.8f;
                        foreach (WeightedRoomAsset classRoom in f1.roomGroup.First(x => x.name == "Class").potentialRooms)
                        {
                            ld.roomGroup.First(x => x.name == "Class").potentialRooms = ld.roomGroup.First(x => x.name == "Class").potentialRooms.AddItem(new WeightedRoomAsset
                            {
                                selection = classRoom.selection,
                                weight = classRoom.weight / 2
                            }).ToArray();
                        }
                        break;
                    case "F3":
                        ld.minExtraRooms += 1;
                        ld.maxExtraRooms += 2;
                        ld.facultyStickToHallChance += 0.75f;
                        ld.minEvents += 3;
                        ld.maxEvents += 4;
                        ld.minSpecialBuilders += 4;
                        ld.maxSpecialBuilders += 5;
                        ld.additionalNPCs += 9;
                        ld.maxItemValue /= 2;

                        List<WeightedNPC> __npcsToRemove = new List<WeightedNPC>();

                        foreach (WeightedNPC npc in ld.potentialNPCs)
                        {
                            if (npc.selection.Character == Character.Sweep || npc.selection.Character == Character.DrReflex)
                                __npcsToRemove.Add(npc);
                        }

                        foreach (WeightedNPC npc in __npcsToRemove)
                            ld.potentialNPCs.Remove(npc);

                        ld.roomGroup.First(x => x.name == "Class").stickToHallChance = 0.8f;
                        foreach (WeightedRoomAsset classRoom in f1.roomGroup.First(x => x.name == "Class").potentialRooms)
                        {
                            ld.roomGroup.First(x => x.name == "Class").potentialRooms = ld.roomGroup.First(x => x.name == "Class").potentialRooms.AddItem(new WeightedRoomAsset
                            {
                                selection = classRoom.selection,
                                weight = classRoom.weight / 2
                            }).ToArray();
                        }

                        ld.exitCount = 3;
                        ld.roomGroup.First(x => x.name == "Class").stickToHallChance = 0.5f;
                        foreach (WeightedRoomAsset classRoom in f1.roomGroup.First(x => x.name == "Class").potentialRooms)
                        {
                            ld.roomGroup.First(x => x.name == "Class").potentialRooms = ld.roomGroup.First(x => x.name == "Class").potentialRooms.AddItem(new WeightedRoomAsset
                            {
                                selection = classRoom.selection,
                                weight = classRoom.weight / 3
                            }).ToArray();
                        }
                        break;
                    case "END":
                        ld.maxExtraRooms += 1;
                        ld.minEvents += 2;
                        ld.maxEvents += 5;
                        ld.minSpecialBuilders += 2;
                        ld.maxSpecialBuilders += 5;
                        ld.additionalNPCs += 5;
                        ld.roomGroup.First(x => x.name == "Class").stickToHallChance = 0.6f;
                        foreach (WeightedRoomAsset classRoom in f1.roomGroup.First(x => x.name == "Class").potentialRooms)
                        {
                            ld.roomGroup.First(x => x.name == "Class").potentialRooms = ld.roomGroup.First(x => x.name == "Class").potentialRooms.AddItem(new WeightedRoomAsset
                            {
                                selection = classRoom.selection,
                                weight = classRoom.weight / 2
                            }).ToArray();
                        }
                        break;
                }

                RoomGroup[] groups = [ld.roomGroup.Where(x => x.name.Contains("Class")).First(), ld.roomGroup.Where(x => x.name.Contains("Faculty")).First(), ld.roomGroup.Where(x => x.name.Contains("Office")).First()];
                foreach (FloorData floorData in floors)
                {
                    if (floorData.Floor == floorName)
                    {
                        ld.hallWallTexs = ld.hallWallTexs.AddRangeToArray(floorData.wallTextures[RoomTextures.Hall].ToArray());
                        ld.hallFloorTexs = ld.hallFloorTexs.AddRangeToArray(floorData.floorTextures[RoomTextures.Hall].ToArray());
                        ld.hallCeilingTexs = ld.hallCeilingTexs.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Hall].ToArray());

                        ld.roomGroup = ld.roomGroup.AddRangeToArray(floorData.roomGroups.ToArray());

                        groups[0].wallTexture = groups[0].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Class].ToArray());
                        groups[0].floorTexture = groups[0].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Class].ToArray());
                        groups[0].ceilingTexture = groups[0].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Class].ToArray());

                        groups[1].wallTexture = groups[1].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Faculty].ToArray());
                        groups[1].floorTexture = groups[1].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Faculty].ToArray());
                        groups[1].ceilingTexture = groups[1].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Faculty].ToArray());

                        groups[2].wallTexture = groups[2].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Faculty].ToArray());
                        groups[2].floorTexture = groups[2].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Faculty].ToArray());
                        groups[2].ceilingTexture = groups[2].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Faculty].ToArray());

                        ld.potentialItems = ld.potentialItems.AddRangeToArray(floorData.items.ToArray());
                        ld.forcedItems.AddRange(floorData.forcedItems);
                        ld.shopItems = ld.shopItems.AddRangeToArray(floorData.shopItems.ToArray());

                        ld.randomEvents.AddRange(floorData.events);

                        ld.potentialNPCs.AddRange(floorData.NPCs.ToArray());
                        ld.forcedNpcs = ld.forcedNpcs.AddRangeToArray(floorData.forcedNPCs.ToArray());

                    }
                }

                NPCMetaStorage.Instance.Get(Character.Sweep).value.potentialRoomAssets[0].selection.category = RoomCategory.Null;

                foreach (SceneObject scene in Resources.FindObjectsOfTypeAll<SceneObject>())
                {
                    if (scene.levelObject == null)
                    {
                        foreach (RoomData data in scene.levelAsset.rooms)
                        {
                            if (data.category == RoomCategory.Hall || data.type == RoomType.Hall || data.category == RoomCategory.Special)
                                data.doorMats = backDoor;
                        }
                    }
                }
                foreach (RoomAsset room in Resources.FindObjectsOfTypeAll<RoomAsset>())
                {
                    if (room.category == RoomCategory.Special)
                        room.doorMats = backDoor;
                }
            });
        }

        private IEnumerator PreLoading()
        {
            yield return 1;
            yield return "Lodaing...";

            /*
             string json = JsonConvert.SerializeObject(new FileEventData(), Newtonsoft.Json.Formatting.Indented);
             string path = Path.Combine(Application.streamingAssetsPath, "EventData.data");
             File.WriteAllText(path, json);
            */

            Paths.Initialize();
            backDoor = ObjectCreators.CreateDoorDataObject("BackDoor", AssetsLoader.CreateTexture("BackDoor_Open", Paths.GetPath(PathsEnum.Misc)), AssetsLoader.CreateTexture("BackDoor_Close", Paths.GetPath(PathsEnum.Misc)));
            Initialize();
        }

        public void PostLoading()
        {
            foreach (RoomAsset rooms in Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.name.Contains("Class")))
                rooms.AddRoomFunctionToContainer<MathMachineClassFunction>();       
        }

        protected StandardDoorMats backDoor;
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.newbaldibasicspluseducationalfundamentals";
        public const string PLUGIN_NAME = "New Baldi's Basics Plus Educational Fundamentals";
        public const string PLUGIN_VERSION = "0.2";
    }

    public class Configs
    {
        internal ConfigEntry<bool> disablelobby, disablef4, disablef5;
    }
}

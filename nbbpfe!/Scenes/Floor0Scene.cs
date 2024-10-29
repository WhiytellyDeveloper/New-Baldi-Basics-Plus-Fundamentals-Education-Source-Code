using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using System.Linq;
using UnityEngine;

namespace nbppfe.Scenes
{
    public class Floor0Scene : PlusScene
    {
        public override void LoadEverything()
        {
            base.LoadEverything();

            var f1 = Resources.FindObjectsOfTypeAll<SceneObject>().Where(x => x.name.Contains("1")).First();

            var floorObj = ScriptableObject.CreateInstance<CustomLevelObject>();
            floorObj.name = "Main0";
            floorObj.minSize = new (8, 8);
            floorObj.maxSize = new(15, 15);
            floorObj.minPlots = 2; 
            floorObj.maxPlots = 4;
            floorObj.outerEdgeBuffer = 3;

            floorObj.minHallsToRemove = 0;
            floorObj.maxHallsToRemove = 1;
            floorObj.minReplacementHalls = 0;
            floorObj.maxReplacementHalls = 1;
            floorObj.bridgeTurnChance = 1;
            floorObj.additionTurnChance = 3;
            floorObj.maxHallAttempts = 1;
            floorObj.deadEndBuffer = 3;
            floorObj.includeBuffers = true;

            floorObj.hallWallTexs = [
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("Wall").ToWeighted<WeightedTexture2D, Texture2D>(100),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("SaloonWall").ToWeighted<WeightedTexture2D, Texture2D>(85)
            ];

            floorObj.hallFloorTexs = [
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("TileFloor").ToWeighted<WeightedTexture2D, Texture2D>(100),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("ActualTileFloor").ToWeighted<WeightedTexture2D, Texture2D>(100),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("BasicFloor").ToWeighted<WeightedTexture2D, Texture2D>(100)
            ];

            floorObj.hallCeilingTexs = [
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("CeilingNoLight").ToWeighted<WeightedTexture2D, Texture2D>(100),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Texture2D>("PlasticTable").ToWeighted<WeightedTexture2D, Texture2D>(85)
            ];

            floorObj.hallLights = [PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<Transform>("FluorescentLight").ToWeighted<WeightedTransform, Transform>(100)];

            floorObj.maxLightDistance = 18;
            floorObj.minPrePlotSpecialHalls = 0;
            floorObj.maxPrePlotSpecialHalls = 2;
            floorObj.minPostPlotSpecialHalls = 0;
            floorObj.maxPostPlotSpecialHalls = 2;
            floorObj.prePlotSpecialHallChance = 0.1f;
            floorObj.postPlotSpecialHallChance = 0.1f;
            floorObj.standardHallBuilders = [];
            floorObj.minSpecialBuilders = 1;
            floorObj.maxSpecialBuilders = 3;
            floorObj.forcedSpecialHallBuilders = [
               PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<GenericHallBuilder>("PlantBuilder"),
               PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<SwingDoorBuilder>("SwingDoorBuilder")
            ];

            floorObj.specialHallBuilders = [
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<GenericHallBuilder>("DietBsodaHallBuilder").ToWeighted<WeightedObjectBuilder, ObjectBuilder>(45),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<GenericHallBuilder>("ZestyHallBuilder").ToWeighted<WeightedObjectBuilder, ObjectBuilder>(63),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<GenericHallBuilder>("PayphoneBuilder").ToWeighted<WeightedObjectBuilder, ObjectBuilder>(70),
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<GenericHallBuilder>("WaterFountainHallBuilder").ToWeighted<WeightedObjectBuilder, ObjectBuilder>(100)
            ];

            floorObj.potentialPrePlotSpecialHalls = [];
            floorObj.potentialPostPlotSpecialHalls = [];

            floorObj.roomGroup = [
                new() {
                    name = "Office",
                    minRooms = 1,
                    maxRooms = 1,
                    stickToHallChance = 1,
                    potentialRooms = f1.levelObject.roomGroup.Where(x => x.name == "Office").First().potentialRooms,
                    light = f1.levelObject.roomGroup.Where(x => x.name == "Office").First().light,
                    wallTexture = f1.levelObject.roomGroup.Where(x => x.name == "Office").First().wallTexture,
                    floorTexture = f1.levelObject.roomGroup.Where(x => x.name == "Office").First().floorTexture,
                    ceilingTexture = f1.levelObject.roomGroup.Where(x => x.name == "Office").First().ceilingTexture
                },
                new() {
                    name = "Class",
                    minRooms = 3,
                    maxRooms = 3,
                    stickToHallChance = 1,
                    potentialRooms = f1.levelObject.roomGroup.Where(x => x.name == "Class").First().potentialRooms,
                    light = f1.levelObject.roomGroup.Where(x => x.name == "Class").First().light,
                    wallTexture = f1.levelObject.roomGroup.Where(x => x.name == "Class").First().wallTexture,
                    floorTexture = f1.levelObject.roomGroup.Where(x => x.name == "Class").First().floorTexture,
                    ceilingTexture = f1.levelObject.roomGroup.Where(x => x.name == "Class").First().ceilingTexture
                },
                new() {
                    name = "Faculty",
                    minRooms = 1,
                    maxRooms = 3,
                    stickToHallChance = 1,
                    potentialRooms = f1.levelObject.roomGroup.Where(x => x.name == "Faculty").First().potentialRooms,
                    light = f1.levelObject.roomGroup.Where(x => x.name == "Faculty").First().light,
                    wallTexture = f1.levelObject.roomGroup.Where(x => x.name == "Faculty").First().wallTexture,
                    floorTexture = f1.levelObject.roomGroup.Where(x => x.name == "Faculty").First().floorTexture,
                    ceilingTexture = f1.levelObject.roomGroup.Where(x => x.name == "Faculty").First().ceilingTexture
                }
            ];

            floorObj.centerWeightMultiplier = 7;
            floorObj.perimeterBase = 2;
            floorObj.dijkstraWeightPower = 0.5f;
            floorObj.dijkstraWeightValueMultiplier = 0.15f;
            floorObj.extraDoorChance = 0.01f;
            floorObj.additionalHallDoorRequirementMultiplier = 1;
            floorObj.hallPriorityDampening = 2;
            floorObj.standardDoorMat = f1.levelObject.standardDoorMat;
            floorObj.minSpecialRooms = 0;
            floorObj.maxSpecialRooms = 0;
            floorObj.specialRoomsStickToEdge = true;
            floorObj.potentialSpecialRooms = f1.levelObject.potentialSpecialRooms;

            floorObj.lightMode = LightMode.Additive;
            floorObj.standardLightStrength = 30;
            floorObj.standardLightColor = Color.white;
            floorObj.standardLightColor = Color.white;
            floorObj.potentialBaldis = f1.levelObject.potentialBaldis; //Placeholder 
            floorObj.additionalNPCs = 1;
            floorObj.potentialNPCs = [
                NPCMetaStorage.Instance.Get(Character.Crafters).value.ToWeighted<WeightedNPC, NPC>(100),
                NPCMetaStorage.Instance.Get(Character.Bully).value.ToWeighted<WeightedNPC, NPC>(84),
                NPCMetaStorage.Instance.Get(Character.Cumulo).value.ToWeighted<WeightedNPC, NPC>(100)
            ];

            floorObj.forcedNpcs = [
                PixelInternalAPI.Extensions.GenericExtensions.FindResourceObjectByName<NPC>("Principal"),
            ];

            floorObj.posterChance = 0;
            floorObj.posters = [];

            floorObj.potentialItems = [
                ItemMetaStorage.Instance.FindByEnum(Items.AlarmClock).value.ToWeighted<WeightedItemObject, ItemObject>(42),
                ItemMetaStorage.Instance.FindByEnum(Items.Boots).value.ToWeighted<WeightedItemObject, ItemObject>(50),
                ItemMetaStorage.Instance.FindByEnum(Items.DietBsoda).value.ToWeighted<WeightedItemObject, ItemObject>(5),
                ItemMetaStorage.Instance.FindByEnum(Items.ChalkEraser).value.ToWeighted<WeightedItemObject, ItemObject>(50),
                ItemMetaStorage.Instance.FindByEnum(Items.DetentionKey).value.ToWeighted<WeightedItemObject, ItemObject>(30),
                ItemMetaStorage.Instance.FindByEnum(Items.Wd40).value.ToWeighted<WeightedItemObject, ItemObject>(50),
                ItemMetaStorage.Instance.FindByEnum(Items.DoorLock).value.ToWeighted<WeightedItemObject, ItemObject>(44),
                ItemMetaStorage.Instance.FindByEnum(Items.Tape).value.ToWeighted<WeightedItemObject, ItemObject>(32),
                ItemMetaStorage.Instance.FindByEnum(Items.NanaPeel).value.ToWeighted<WeightedItemObject, ItemObject>(38),
                ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value.ToWeighted<WeightedItemObject, ItemObject>(37),
                ItemMetaStorage.Instance.GetPointsObject(25, true).ToWeighted<WeightedItemObject, ItemObject>(40)
            ];

            floorObj.forcedItems = [
                ItemMetaStorage.Instance.FindByEnum(Items.Quarter).value,
                ItemMetaStorage.Instance.GetPointsObject(25, true),
                ItemMetaStorage.Instance.GetPointsObject(25, true)
            ];

            floorObj.maxItemValue = 50;
            floorObj.lowEndCutoff = 80;
            floorObj.singleEntranceItemVal = 15;
            floorObj.noHallItemVal = 25;

            floorObj.minEvents = 0;
            floorObj.maxEvents = 0;

            floorObj.randomEvents = f1.levelObject.randomEvents;

            floorObj.elevatorPre = f1.levelObject.elevatorPre;
            floorObj.elevatorRoom = f1.levelObject.elevatorRoom;
            floorObj.hallBuffer = 2;
            floorObj.edgeBuffer = 1;
            floorObj.timeBonusLimit = 80;
            floorObj.timeBonusVal = 45;

            floorObj.MarkAsNeverUnload();

            var scene = ScriptableObject.CreateInstance<SceneObject>();
            scene.name = "MainLevel_0";
            scene.manager = f1.manager;
            scene.levelObject = floorObj; //Placeholder
            scene.skybox = f1.skybox;
            scene.skyboxColor = Color.white;
            scene.nextLevel = f1;
            scene.levelTitle = "F0";
            scene.nameKey = "Level_Main0";
            scene.levelNo = 0;
            scene.mapPrice = 50;
            scene.totalShopItems = 2;
            scene.shopItems = f1.shopItems;
            scene.MarkAsNeverUnload();
            //GeneratorManagement.Invoke("F0", 0, scene);
            Debug.Log("Invoking SceneObject(F0)(0) Generation Changes for Main0 (MTM101BaldAPI.CustomLevelObject)!");
            SceneLoader.F0 = scene;
        }
    }
}

﻿using MTM101BaldAPI.Reflection;
using nbbpfe.FundamentalsManager.Loaders;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using System.Collections.Generic;
using UnityEngine;

namespace nbbpfe.FundamentalsManager
{
    public static class FundamentalLoaderManager
    {
        public static void Initialize()
        {
            GenericThrowSound = AssetsLoader.CreateSound("throwSound", Paths.GetPath(PathsEnum.Misc), "", SoundType.Effect, Color.white, 1);
            GenericDrinkingSound = AssetsLoader.CreateSound("drinkingSound1", Paths.GetPath(PathsEnum.Misc), "", SoundType.Effect, Color.white, 1);
            GenericEatSound = (SoundObject)Items.ZestyBar.ToItem().item.GetComponent<ITM_ZestyBar>().ReflectionGetVariable("audEat");

            ItemsLoader.LoadItems();
            CustomObjectsLoader.Load();
            RoomsLoader.LoadRooms();
            NPCLoader.SetupNPCs();
            SceneLoader.Load();
        }

        public static List<FloorData> floors = new List<FloorData>{
            new FloorData("F1"),
            new FloorData("F2"),
            new FloorData("F3"),
            new FloorData("F4"),
            new FloorData("END")
        };

        public static SoundObject GenericThrowSound;
        public static SoundObject GenericDrinkingSound;
        public static SoundObject GenericEatSound;

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

            public List<WeightedItemObject> items = [];
            public List<WeightedItemObject> shopItems = [];
            public List<ItemObject> forcedItems = [];

            public List<WeightedNPC> NPCs = [];
            public List<NPC> forcedNPCs = [];
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

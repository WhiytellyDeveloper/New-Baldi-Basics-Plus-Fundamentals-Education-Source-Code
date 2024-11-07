using BepInEx.Bootstrap;
using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.CustomContent.CustomItems;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.CustomData;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.ModsCompabilitys;
using nbppfe.PrefabSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static nbppfe.FundamentalsManager.FundamentalLoaderManager;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class ItemsLoader
    {
        public static Dictionary<ItemObject, bool> isDiet = [];

        public static void LoadItems()
        {
            LoadItem<ITM_CommonTeleporter>("CommonTeleporter", CustomItemsEnum.CommonTeleporter)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 60, 45, 48, 89])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [75, 40, 50, 65, 80])
            .AddInEditor();

            LoadItem<ITM_GenericHammer>("GenericHammer", CustomItemsEnum.GenericHammer)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 70, 50, 20, 60])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 75, 40, 30, 60])
            .AddInEditor();

            LoadItem<ITM_BaseBall>("Baseball", CustomItemsEnum.Baseball)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 45, 50, 45, 40])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [60, 65, 75, 65, 60])
            .AddInEditor();

            LoadItem<ITM_Soda>("Soda", CustomItemsEnum.Soda)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 15, 30, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 30, 20, 45, 55])
            .MakeItPartyItem(45)
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_Soda>("DietSoda", CustomItemsEnum.DietSoda, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 40, 38, 55, 57])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 53, 40, 38, 65])
            .AddInEditor();

            LoadItem<ITM_CoffeAndSugar>("CoffeAndSugar", CustomItemsEnum.CoffeAndSugar)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 25, 34, 40, 75])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 45, 50, 40, 60])
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_SweepWhistle>("SweepWhistle", CustomItemsEnum.SweepWhistle)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 60, 70, 40, 50, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 70, 66, 40, 56])
            .AddInEditor()
            .RequiresANpcToExist(Character.Sweep);

            LoadItem<Item>("BullyPresent", CustomItemsEnum.BullyPresent)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 50, 44, 48, 60])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 50, 55, 48, 52])
            .AddInEditor()
            .RequiresANpcToExist(Character.Bully);

            LoadItem<ITM_Glue>("Glue", CustomItemsEnum.StckyGlue)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 30, 15, 20, 23])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 35, 42, 30, 40])
            .MakeItFieldTripItem(57)
            .AddInEditor();

            LoadItem<ITM_Glue>("GlueStick", CustomItemsEnum.StickGlue, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 40, 63, 40, 38, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [15, 23, 20, 15, 20])
            .MakeItFieldTripItem(35)
            .AddInEditor();

            LoadItem<ITM_Pretzel>("Pretzel", CustomItemsEnum.Pretzel)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 50, 30, 40, 12])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
            .MakeItFieldTripItem(58)
            .AddInEditor();

            LoadItem<ITM_NoClipController>("NoClipController", CustomItemsEnum.NoClipController)
            .MakeItWeightedItem(["F3", "F4", "END"], [20, 10, 40])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [35, 35, 15, 50])
            .MakeItPartyItem(175)
            .MakeItMysteryItem(50)
            .MakeItGuaranteedFieldTripItem()
            .AddInEditor();

            LoadItem<ITM_Walkman>("Walkman", CustomItemsEnum.Walkman)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [42, 30, 40, 32, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [52, 40, 50, 64, 80])
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_WhiteZesty>("WhiteZesty", CustomItemsEnum.WhiteZesty)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 60, 75, 40, 85])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [75, 40, 50, 80, 60])
            .MakeItForcedItem(["F1", "F2", "END"], [1, 2, 1])
            .MakeItFieldTripItem(100)
            .AddInEditor();

            LoadItem<ITM_Present>("Present", CustomItemsEnum.Present)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 20, 30, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 40, 30, 20, 10])
            .MakeItForcedItem(["F2", "F3", "END"], [1, 1, 1])
            .MakeItFieldTripItem(66)
            .AddInEditor();

            LoadItem<ITM_Cheese>("Cheese", CustomItemsEnum.Cheese)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [25, 35, 25, 35, 45])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [27, 37, 27, 37, 57])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(35)
            .AddInEditor();

            LoadItem<ITM_Cookie>("Cookie", CustomItemsEnum.Cookie)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [75, 50, 60, 30, 85])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 50, 60, 45, 50])
            .AddInEditor();

            LoadItem<ITM_Magnet>("Magnet", CustomItemsEnum.Magnet)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 20, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [35, 40, 20, 40, 50])
            .MakeItPartyItem(72)
            .MakeItFieldTripItem(85)
            .AddInEditor();

            LoadItem<ITM_Homework>("HomeworkTierA", CustomItemsEnum.HomeworkTierA)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [44, 55, 44, 60])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [48, 50, 24, 48])
            .MakeItFieldTripItem(85)
            .AddInEditor();

            LoadItem<ITM_BoxPortal>("BoxPortal", CustomItemsEnum.BoxPortal)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [50, 40, 60, 75])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [40, 51, 52, 60])
            .MakeItPartyItem(100)
            .MakeItFieldTripItem(50)
            .AddInEditor();

            LoadItem<ITM_Coffe>("Coffe", CustomItemsEnum.Coffe)
            .MakeItWeightedItem(["F3", "F4", "END"], [15, 20, 30])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [40, 45, 50, 55])
           .MakeItMysteryItem(95)
           .MakeItPartyItem(185)
           .MakeItGuaranteedFieldTripItem()
           .AddInEditor();

            LoadItem<ITM_Box>("Box", CustomItemsEnum.Box)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [35, 40, 35, 44, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 40, 30, 50, 40])
            .MakeItFieldTripItem(55)
            .AddInEditor();

            LoadItem<ITM_SupernaturalPudding>("Pudding", CustomItemsEnum.SupernaturalPudding)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 20, 10, 22, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 18, 22, 30, 48])
            .MakeItPartyItem(75)
            .MakeItFieldTripItem(80)
            .AddInEditor();

            LoadItem<ITM_ColoredVisionPad>("PurpleVisionPad", CustomItemsEnum.PurpleVisonPad)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 40, 35, 30, 33])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 42, 30, 40, 20])
            .MakeItFieldTripItem(48)
            .AddInEditor()
            .item.GetComponent<ITM_ColoredVisionPad>().itemEnum = CustomItemsEnum.PurpleVisonPad;

            LoadItem<ITM_ColoredVisionPad>("OrangeVisionPad", CustomItemsEnum.OrangeVisionPad)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 30, 20, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [33, 40, 50, 33, 20])
            .MakeItFieldTripItem(49)
            .AddInEditor()
            .item.GetComponent<ITM_ColoredVisionPad>().itemEnum = CustomItemsEnum.OrangeVisionPad;

            LoadItem<ITM_DuctTape>("DuctTape", CustomItemsEnum.DuctTape)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [30, 20, 35, 44])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [45, 35, 30, 50])
            .MakeItPartyItem(24)
            .MakeItFieldTripItem(100)
            .AddInEditor();

            LoadItem<ITM_Shovel>("Shovel", CustomItemsEnum.Shovel)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 50, 42, 44, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [42, 52, 44, 48, 52])
            .MakeItFieldTripItem(45)
            .AddInEditor();

            LoadItem<ITM_WaterBucket>("WaterBucket", CustomItemsEnum.WaterBucket)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [55, 40, 30, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 50, 30, 40, 50])
            .MakeItFieldTripItem(65)
            .AddInEditor();

            LoadItem<ITM_Umbrella>("Umbrella", CustomItemsEnum.Umbrella)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 40, 20, 40, 60])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 80, 40, 80, 75])
            .MakeItFieldTripItem(46)
            .MakeItPartyItem(50)
            .AddInEditor();

            LoadItem<ITM_FidgetSpinner>("FidgetSpinner", CustomItemsEnum.OutdatedFidgetSpinner)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 15, 30, 20, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 33, 40, 20, 70])
            .MakeItPartyItem(52)
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_HandHook>("HandHook", CustomItemsEnum.HandHook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 30, 45, 25, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 55, 52, 48, 65])
            .MakeItPartyItem(40)
            .MakeItFieldTripItem(50)
            .AddInEditor();

            LoadItem<ITM_GrilledCheese>("GrilledCheese", CustomItemsEnum.GrilledCheese)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [45, 32, 22, 50, 39])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [32, 38, 44, 80, 70])
            .AddInEditor();

            LoadItem<ITM_GenericSoda>("GenericSoda", CustomItemsEnum.GenericSoda)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 20, 22, 30, 20, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 30, 20, 15, 30])
            .MakeItFieldTripItem(45)
            .MakeItPartyItem(80)
            .AddInEditor();

            LoadItem<ITM_GenericSoda>("DietGenericSoda", CustomItemsEnum.DietGenericSoda, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [35, 40, 34, 45, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 50, 48, 52, 58])
            .AddInEditor();

            LoadItem<ITM_Tea>("Tea", CustomItemsEnum.Tea)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 10, 30, 30, 20])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 30, 15, 20, 40])
            .MakeItFieldTripItem(80)
            .MakeItPartyItem(25)
            .AddInEditor();

            LoadItem<ITM_Tea>("DietTea", CustomItemsEnum.DietTea, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 30, 20, 18, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 30, 45, 30, 45])
            .MakeItFieldTripItem(40)
            .AddInEditor();

            LoadItem<ITM_AdvertenceBook>("AdvertenceBook", CustomItemsEnum.AdvertenceBook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 65, 30, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 56, 30, 20, 40])
            .MakeItFieldTripItem(40)
            .AddInEditor();

            LoadItem<ITM_AdvertenceBook>("ConnectedAdvertenceBook", CustomItemsEnum.ConnectedAdvertenceBook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [22, 30, 20, 15, 25])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 20, 14, 30, 44])
            .MakeItFieldTripItem(40)
            .MakeItPartyItem(75)
            .AddInEditor();

            //You who ask yourself, why don't you use the diet which is practically the same thing. It simply doesn't work for ANY reason. WHY??????
            CustomItemsEnum.ConnectedAdvertenceBook.ToItem().item.GetComponent<ITM_AdvertenceBook>().connected = true;

            LoadItem<ITM_Compass>("Compass", CustomItemsEnum.Compass)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 30, 25, 33, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 50, 30, 50, 66])
            .MakeItFieldTripItem(22)
            .AddInEditor();

            LoadItem<ITM_MapPoint>("MapPoint", CustomItemsEnum.MapPoint)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [45, 60, 55, 48, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 50, 45, 45, 60])
            .AddInEditor();

            LoadItem<ITM_BaseballBat>("BaseballBat", CustomItemsEnum.BaseballBat)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 40, 15, 45])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [38, 30, 45, 30, 15])
            .MakeItFieldTripItem(40)
            .AddInEditor();

            LoadItem<ITM_SafteyGlasses>("SafteyGlasses", CustomItemsEnum.SafteyGlasses)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [28, 20, 15, 30, 40])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [38, 50, 60, 44, 50])
            .MakeItFieldTripItem(45)
            .AddInEditor();

            LoadItem<ITM_FreezeClock>("FreezeClock", CustomItemsEnum.FreezeClock)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [5, 20, 15, 8, 13])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
            .MakeItMysteryItem(50)
            .MakeItPartyItem(80)
            .MakeItFieldTripItem(90)
            .AddInEditor();

            LoadItem<ITM_Horn>("Horn", CustomItemsEnum.Horn)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [45, 60, 5, 40, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 30, 20, 15, 30])
            .AddInEditor();

            LoadItem<ITM_BanHammer>("BanHammer", CustomItemsEnum.BanHammer)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 5, 22, 18, 20])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 18, 44, 30, 44])
            .MakeItGuaranteedFieldTripItem()
            .MakeItMysteryItem(60)
            .AddInEditor();

            LoadItem<ITM_Swapper>("Swapper", CustomItemsEnum.Swapper)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 40, 30, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 30, 20, 40, 45])
            .MakeItFieldTripItem(32)
            .AddInEditor();

            LoadItem<ITM_PlayerCardboard>("PlayerCardboard", CustomItemsEnum.PlayerCardboard)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 15, 30, 33, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 30, 20, 20, 40])
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_PlayerCardboard>("FakePlayerCardboard", CustomItemsEnum.FakePlayerCardboard, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 42, 44, 48, 75])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 32, 40, 44, 45])
            .MakeItFieldTripItem(45)
            .AddInEditor()
            .item.GetComponent<ITM_PlayerCardboard>().fake = true;

            LoadItem<ITM_GatwayTeleporter>("GatewayTeleporter", CustomItemsEnum.GatewayTeleporter)
            .MakeItWeightedItem(["F2", "F3", "F4"], [40, 30, 20])
            .MakeItWeightedItemInShop(["F2", "F3", "F4"], [40, 30, 20])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(65)
            .AddInEditor();

            LoadItem<ITM_IceCreamStick>("FreezingIceCream", CustomItemsEnum.StickIceCream)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 15, 30, 40, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [33, 20, 15, 30, 40])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(65)
            .AddInEditor();

            LoadItem<ITM_Traumatized>("Traumatized", CustomItemsEnum.Traumatized)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [38, 30, 20, 45])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [45, 30, 25, 38])
            .MakeItFieldTripItem(72)
            .AddInEditor();

            LoadItem<ITM_EntityTeleporter>("EntityTeleporter", CustomItemsEnum.EntityTeleporter)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [1, 20, 15, 30, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [5, 30, 20, 34, 74])
            .MakeItFieldTripItem(72)
            .AddInEditor();

            LoadItem<ITM_JumpBoots>("JumpBoots", CustomItemsEnum.JumpBoots)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [5, 20, 14, 8, 20])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [23, 21, 30, 22, 20])
            .MakeItPartyItem(60)
            .MakeItFieldTripItem(100)
            .AddInEditor();
           
            LoadItem<ITM_YTPsMultiplier>("MultiplierMachine", CustomItemsEnum.YTPsMultiplier)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [5, 25, 30, 42, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 34, 43, 45, 40])
            .MakeItPartyItem(24)
            .MakeItFieldTripItem(25)
            .AddInEditor();      

            LoadItem<ITM_InvisblePaintBucket>("InvisiblePaintBucket", CustomItemsEnum.InvisiblePaintBucket)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 10, 30, 20, 43])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 20, 60, 40, 86])
            .MakeItPartyItem(80)
            .MakeItGuaranteedFieldTripItem()
            .AddInEditor();

            LoadItem<ITM_PaintGun>("PaintGun", CustomItemsEnum.PaintGun)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 40, 50, 75])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 20, 45, 50, 75])
            .MakeItFieldTripItem(70)
            .AddInEditor();

            LoadItem<ITM_PlaceableTeleporter>("PlaceableTeleporter", CustomItemsEnum.PlaceableTeleporter)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 40, 30, 65])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 25, 30, 40, 65])
            .MakeItFieldTripItem(60)
            .AddInEditor();

            LoadItem<ITM_Rollerblades>("Rollerblades", CustomItemsEnum.Rollerblades)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [44, 30, 47, 50, 68])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 48, 58, 52, 50])
            .MakeItFieldTripItem(35)
            .AddInEditor();

            LoadItem<ITM_SwapperHook>("SwapperHook", CustomItemsEnum.SwapperHook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 40, 45, 50, 75])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [52, 45, 50, 40, 53])
            .MakeItPartyItem(100)
            .MakeItFieldTripItem(60)
            .AddInEditor();

            LoadItem<ITM_ImmatureApple>("ImmatureApple", CustomItemsEnum.ImmatureApple)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 44, 50, 62, 48])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [48, 44, 50, 52, 75])
            .MakeItPartyItem(25)
            .MakeItFieldTripItem(75)
            .AddInEditor();

            LoadItem<Item>("HallPass", CustomItemsEnum.HallPass)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 40, 10, 65])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 30, 40, 33, 40])
            .MakeItForcedItem(["F2", "F3", "F4", "END"], [1, 1, 1, 1])
            .AddInEditor();

            LoadItem<ITM_Bag>("Bag", CustomItemsEnum.Bag)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 25, 30, 28, 36])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 42, 44, 45, 53])
            .MakeItGuaranteedFieldTripItem()
            .MakeItPartyItem(50)
            .AddInEditor();

            var item = LoadItem<ITM_Bag>("Bag", CustomItemsEnum.OpenBag);
            item.itemSpriteLarge = AssetsLoader.CreateSprite("block!BagOpen_IconLarge", Paths.GetPath(PathsEnum.Items, "Bag"), 50);
            item.itemSpriteSmall = AssetsLoader.CreateSprite("block!BagOpen_IconSmall", Paths.GetPath(PathsEnum.Items, "Bag"), 1);
            item.nameKey = "Itm_OpenBag";

            LoadItem<ITM_IceCreamMask>("IceCreamMask", CustomItemsEnum.IceCreamMask)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
           .AddInEditor();

            LoadItem<Item>("PowerTube", CustomItemsEnum.PowerTube)
            .MakeItWeightedItem(["F2", "F3", "F4"], [30, 34, 20])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4"], [50, 75, 66, 50]);

            LoadItem<ITM_BalloonPacket>("BalloonPacket", CustomItemsEnum.BallonPacket)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [0, 0, 0, 0, 0])
           .MakeItFieldTripItem(70)
           .AddInEditor();

            //Nothing like a little trick doesn't solve it
            var full = LoadItem<ITM_1LBottle>("1LBottle", CustomItemsEnum.W1LBottle)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [28, 38, 48, 47, 50])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 20, 15, 40, 44])
           .AddInEditor();
            full.item.GetComponent<ITM_1LBottle>().drinkable = true;
            full.itemSpriteLarge = AssetsLoader.CreateSprite("FullWaterGallon_IconLarge", Paths.GetPath(PathsEnum.Items, "1LBottle"), 50);
            full.itemSpriteSmall = AssetsLoader.CreateSprite("FullWaterGallon_IconSmall", Paths.GetPath(PathsEnum.Items, "1LBottle"), 50);

            var half = LoadItem<ITM_1LBottle>("1LBottle", CustomItemsEnum.W1LBottleHalf);
            half.item.GetComponent<ITM_1LBottle>().drinkable = true;
            half.itemSpriteLarge = AssetsLoader.CreateSprite("block!HalfwayWaterGallon_IconLarge", Paths.GetPath(PathsEnum.Items, "1LBottle"), 50);
            half.itemSpriteSmall = AssetsLoader.CreateSprite("block!HalfwayWaterGallon_IconSmall", Paths.GetPath(PathsEnum.Items, "1LBottle"), 50);
            half.nameKey = "Itm_1LBottleHalf";
            half.name = "1LBottleHalf";
            half.AddInEditor();

            var empty = LoadItem<ITM_1LBottle>("1LBottle", CustomItemsEnum.W1LBottleEmpty);
            empty.nameKey = "Itm_1LBottleEmpty";
            empty.name = "1LBottleEmpty";
            empty.itemSpriteLarge = AssetsLoader.CreateSprite("block!EmptyWaterGallon_IconLarge", Paths.GetPath(PathsEnum.Items, "1LBottle"), 50);
            empty.AddInEditor();

            full.item.GetComponent<ITM_1LBottle>().previousItem = half;
            half.item.GetComponent<ITM_1LBottle>().nextItem = full;
            half.item.GetComponent<ITM_1LBottle>().previousItem = empty;
            empty.item.GetComponent<ITM_1LBottle>().nextItem = half;

            LoadItem<ITM_Mirror>("Mirror", CustomItemsEnum.Mirror)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 10, 30, 33, 20])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [15, 30, 20, 34, 45])
           .MakeItFieldTripItem(45)
           .AddInEditor();

            LoadItem<ITM_Pickaxe>("Pickaxe", CustomItemsEnum.Pickaxe)
            .AddInEditor();

            LoadItem<ITM_Acceptable>("BlueLocker", CustomItemsEnum.BlueLocker)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 33, 20, 40])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 40, 32, 33, 45])
           .AddInEditor();

            LoadItem<ITM_TeleportArrow>("TeleportArrow", CustomItemsEnum.TeleportationArrow)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 15, 30, 33])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [24, 30, 22, 38, 42])
           .AddInEditor();

            LoadItem<ITM_PercussionHammer>("PercussionHammer", CustomItemsEnum.PercussionHammer)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [10, 15, 22, 17, 22])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [22, 20, 14, 20, 32])
           .AddInEditor();

            LoadItem<ITM_PercussionHammer>("PlasticPercussionHammer", CustomItemsEnum.PlasticPercussionHammer, true)
           .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 20, 33, 20, 40])
           .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 20, 33, 20, 33])
           .AddInEditor();
        }

        public static ItemObject LoadItem<T>(string path, CustomItemsEnum itemEnum, bool diet = false) where T : Item
        {
            string json = File.ReadAllText(Paths.GetPath(PathsEnum.Items, [path, "ItemData.data"]));
            FileItemData item = JsonUtility.FromJson<FileItemData>(json);

            ItemBuilder itemBuilder = new ItemBuilder(BasePlugin.Instance.Info);
            itemBuilder.SetNameAndDescription(item.itemNameKey, item.itemDescriptionKey);
            itemBuilder.SetEnum(itemEnum.ToString());

            string[] sprites = Directory.GetFiles(Paths.GetPath(PathsEnum.Items, path), "*.png", SearchOption.AllDirectories);
            Dictionary<string, Sprite> spritesDic = [];
            foreach (string file in sprites)
            {
                if (file.Contains(item.postfixIconSmall) && !file.Contains("block1") && !spritesDic.ContainsKey("small"))
                {
                    spritesDic.Add("small", AssetsLoader.CreateSprite(Path.GetFileNameWithoutExtension(file), Paths.GetPath(PathsEnum.Items, path), 1));
                }
                else if (file.Contains(item.postfixIconLarge) && !file.Contains("block!") && !spritesDic.ContainsKey("large"))
                    spritesDic.Add("large", AssetsLoader.CreateSprite(Path.GetFileNameWithoutExtension(file), Paths.GetPath(PathsEnum.Items, path), 50));
            }

            string[] sounds = Directory.GetFiles(Path.Combine(AssetLoader.GetModPath(BasePlugin.Instance), "Items", path), "*.wav", SearchOption.AllDirectories);

            foreach (string file in sounds)
            {
                if (file.Contains(item.pickupItemSoundName) && item.pickupItemSoundName != "")
                    itemBuilder.SetPickupSound(AssetsLoader.CreateSound(item.pickupItemSoundName, Paths.GetPath(PathsEnum.Items, [path, item.pickupItemSoundName + ".wav"]), "", SoundType.Effect, Color.white, 1));
            }

            itemBuilder.SetSprites(spritesDic["small"], spritesDic["large"]);
            itemBuilder.SetMeta(ItemFlags.None, item.tags);
            if (item.isMultipleUse) itemBuilder.SetMeta(ItemFlags.MultipleUse, ["multiple"]);
            if (item.autoUse) itemBuilder.SetAsInstantUse();
            itemBuilder.SetShopPrice(item.price);
            itemBuilder.SetGeneratorCost(item.cost);
            ItemObject itemObj = itemBuilder.SetItemComponent<T>().Build();

            itemObj.name = item.itemNameKey.Replace("Itm_", "");

            if (itemObj.item.GetComponent<DietItemVariation>() != null) itemObj.item.GetComponent<DietItemVariation>().diet = diet;

            isDiet.Add(itemObj, diet);
            if (diet)
                itemObj.name.StartsWith("DietItem_");

            if (itemObj.item.GetComponent<IItemPrefab>() != null) itemObj.item.GetComponent<IItemPrefab>().Setup();

            if (itemObj.item.GetComponent<ITM_Acceptable>()) itemObj.item.GetComponent<ITM_Acceptable>().ReflectionSetVariable("item", itemEnum.ToItemEnum());

            if (item.isFood) itemObj.GetMeta().tags.Add("Food");
            if (item.isDrink) itemObj.GetMeta().tags.Add("Drink");

            if (itemObj.GetMeta().tags.Contains("none")) itemObj.GetMeta().tags.Remove("none");

            return itemObj;
        }

        public static ItemObject MakeItWeightedItem(this ItemObject item, string[] floors, int[] weights)
        {
            int weight = 0;
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                {
                    floor.items.Add(new WeightedItemObject { selection = item, weight = weights[weight]});
                    weight++;
                }
            }
            return item;
        }

        public static ItemObject MakeItWeightedItemInShop(this ItemObject item, string[] floors, int[] weights)
        {
            int weight = 0;
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                {
                    floor.shopItems.Add(new WeightedItemObject { selection = item, weight = weights[weight]});
                    weight++;
                }
            }
            return item;
        }

        public static ItemObject MakeItForcedItem(this ItemObject item, string[] floors, int[] multipliyer)
        {
            int quantity = 0;
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                {
                    for (int i = 0; i < multipliyer[quantity]; i++)
                        floor.forcedItems.Add(item);
                    quantity++;
                }
            }
            return item;
        }

        public static ItemObject MakeItMysteryItem(this ItemObject item, int weight)
        {
            var array = (WeightedItemObject[])Resources.FindObjectsOfTypeAll<MysteryRoom>().First().ReflectionGetVariable("items");
            WeightedItemObject[] newArray = array.AddRangeToArray([new WeightedItemObject { selection = item, weight = weight }]);
            Resources.FindObjectsOfTypeAll<MysteryRoom>().First().ReflectionSetVariable("items", newArray);
            return item;
        }
        public static ItemObject MakeItPartyItem(this ItemObject item, int weight)
        {
            var array = (WeightedItemObject[])Resources.FindObjectsOfTypeAll<PartyEvent>().First().ReflectionGetVariable("potentialItems");
            WeightedItemObject[] newArray = array.AddRangeToArray([new WeightedItemObject { selection = item, weight = weight }]);
            Resources.FindObjectsOfTypeAll<PartyEvent>().First().ReflectionSetVariable("potentialItems", newArray);
            return item;
        }

        public static ItemObject MakeItFieldTripItem(this ItemObject item, int weight)
        {
            var array = (WeightedItemObject[])Resources.FindObjectsOfTypeAll<FieldTripBaseRoomFunction>().First().ReflectionGetVariable("potentialItems");
            WeightedItemObject[] newArray = array.AddRangeToArray([new WeightedItemObject { selection = item, weight = weight }]);
            Resources.FindObjectsOfTypeAll<FieldTripBaseRoomFunction>().First().ReflectionSetVariable("potentialItems", newArray);
            return item;
        }

        public static ItemObject MakeItGuaranteedFieldTripItem(this ItemObject item)
        {
            List<ItemObject> newArray = (List<ItemObject>)Resources.FindObjectsOfTypeAll<FieldTripBaseRoomFunction>().First().ReflectionGetVariable("guaranteedItems");
            newArray.Add(item); ;
            return item;
        }

        public static ItemObject AddInEditor(this ItemObject item)
        {
            if (Chainloader.PluginInfos.ContainsKey("mtm101.rulerp.baldiplus.leveleditor"))
                EditorCompability.items.Add(new(item));
            return item;
        }

        public static ItemObject RequiresANpcToExist(this ItemObject item, Character character)
        {
            return item;
        }
    }
}

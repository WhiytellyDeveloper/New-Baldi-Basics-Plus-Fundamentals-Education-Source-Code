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
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static nbppfe.FundamentalsManager.FundamentalLoaderManager;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class ItemsLoader
    {

        public static Dictionary<Item, bool> isDiet = [];

        public static void LoadItems()
        {
            LoadItem<ITM_CommonTeleporter>("CommonTeleporter", CustomItemsEnum.CommonTeleporter)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 70, 100, 80, 95])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 70, 50, 40, 80]);

            LoadItem<ITM_GenericHammer>("GenericHammer", CustomItemsEnum.GenericHammer)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 40, 55, 60, 62])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 60, 50, 70, 80])
            .AddKeyTypeItem();

            LoadItem<ITM_BaseBall>("Baseball", CustomItemsEnum.Baseball)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [30, 36, 24, 52])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [55, 47, 62, 70, 25]);

            LoadItem<ITM_Soda>("Soda", CustomItemsEnum.Soda)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 50, 40, 30, 35])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [80, 30, 20, 10, 50])
            .MakeItPartyItem(45)
            .MakeItFieldTripItem(70);

            LoadItem<ITM_Soda>("DietSoda", CustomItemsEnum.DietSoda, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [53, 30, 50, 60, 70])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 15, 10, 5, 25]);

            LoadItem<ITM_CoffeAndSugar>("CoffeAndSugar", CustomItemsEnum.CoffeAndSugar)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [20, 30, 40, 55])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [30, 40, 50, 20])
            .MakeItForcedItem(["F3", "F4"], [1, 2])
            .MakeItFieldTripItem(70);

            LoadItem<ITM_SweepWhistle>("SweepWhistle", CustomItemsEnum.SweepWhistle)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 30, 22, 32, 18])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 30, 55, 32, 30]);

            LoadItem<Item>("BullyPresent", CustomItemsEnum.BullyPresent)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [35, 22, 40, 12, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [37, 30, 20, 40, 44]);

            LoadItem<ITM_Glue>("Glue", CustomItemsEnum.Glue)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 20, 10, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 10, 30, 20, 40])
            .MakeItFieldTripItem(57);

            LoadItem<ITM_Glue>("GlueStick", CustomItemsEnum.StickGlue, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [45, 30, 20, 10, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 28, 30, 20, 40])
            .MakeItFieldTripItem(35);

            LoadItem<ITM_Pretzel>("Pretzel", CustomItemsEnum.Pretzel)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [45, 60, 55, 80])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [70, 67, 15, 95])
            .MakeItFieldTripItem(58);

            LoadItem<ITM_NoClipController>("NoClipController", CustomItemsEnum.NoClipController)
            .MakeItWeightedItem(["F3", "F4", "END"], [10, 12, 30])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [20, 32, 5, 40])
            .MakeItPartyItem(175)
            .MakeItMysteryItem(50)
            .MakeItGuaranteedFieldTripItem();

            LoadItem<ITM_Walkman>("Walkman", CustomItemsEnum.Walkman)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [24, 54, 47, 40, 52])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [15, 20, 25, 30, 35])
            .MakeItFieldTripItem(70);

            LoadItem<ITM_WhiteZesty>("WhiteZesty", CustomItemsEnum.WhiteZesty)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 40, 60, 70, 65])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [54, 44, 64, 74, 24])
            .MakeItForcedItem(["F1", "F2", "END"], [1, 2, 1])
            .MakeItFieldTripItem(100);

            LoadItem<ITM_Present>("Present", CustomItemsEnum.Present)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 80, 55, 19, 70])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [80, 30, 70, 14, 100])
            .MakeItForcedItem(["F2", "F3", "END"], [1, 1, 1])
            .MakeItFieldTripItem(66);

            LoadItem<ITM_Cheese>("Cheese", CustomItemsEnum.Cheese)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [5, 10, 15, 20, 25])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 25, 30, 35, 40])
            .MakeItFieldTripItem(35);

            LoadItem<ITM_Cookie>("Cookie", CustomItemsEnum.Cookie)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [80, 70, 95, 40, 100])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 30, 45, 20, 50]);

            LoadItem<ITM_Magnet>("Magnet", CustomItemsEnum.Magnet)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 45, 32, 30, 80])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [76, 20, 65, 50, 75])
            .MakeItPartyItem(162)
            .MakeItFieldTripItem(85);

            LoadItem<ITM_Homework>("HomeworkTierA", CustomItemsEnum.HomeworkTierA)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [40, 80, 60, 70])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [35, 40, 48, 50])
            .MakeItPartyItem(80)
            .MakeItFieldTripItem(85);

            LoadItem<ITM_BoxPortal>("BoxPortal", CustomItemsEnum.BoxPortal)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [45, 55, 62, 54])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [40, 50, 60, 52])
            .MakeItPartyItem(130)
            .MakeItFieldTripItem(60);

            LoadItem<ITM_Coffe>("Coffe", CustomItemsEnum.Coffe)
           .MakeItWeightedItem(["F3", "F4", "END"], [15, 22, 20])
           .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [25, 75, 30, 42])
           .MakeItMysteryItem(95)
           .MakeItPartyItem(185)
           .MakeItGuaranteedFieldTripItem();

            LoadItem<ITM_Box>("Box", CustomItemsEnum.Box)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 75, 30, 43, 62])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 52, 22, 57, 62])
            .MakeItFieldTripItem(55);

            LoadItem<ITM_SupernaturalPudding>("Pudding", CustomItemsEnum.SupernaturalPudding)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 40, 20, 12, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [24, 50, 45, 30, 64])
            .MakeItPartyItem(75)
            .MakeItFieldTripItem(80);

            LoadItem<ITM_ColoredVisionPad>("PurpleVisionPad", CustomItemsEnum.PurpleVisonPad)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 40, 32, 40, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [22, 35, 52, 48, 50])
            .MakeItFieldTripItem(48)
            .item.GetComponent<ITM_ColoredVisionPad>().itemEnum = CustomItemsEnum.PurpleVisonPad;

            LoadItem<ITM_ColoredVisionPad>("OrangeVisionPad", CustomItemsEnum.OrangeVisionPad)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [27, 42, 34, 44, 35])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [24, 43, 56, 52, 60])
            .MakeItFieldTripItem(49)
            .item.GetComponent<ITM_ColoredVisionPad>().itemEnum = CustomItemsEnum.OrangeVisionPad;

            LoadItem<ITM_DuctTape>("DuctTape", CustomItemsEnum.DuctTape)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [34, 45, 30, 44])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [18, 30, 19, 31])
            .MakeItPartyItem(24)
            .MakeItFieldTripItem(100);

            LoadItem<ITM_Shovel>("Shovel", CustomItemsEnum.Shovel)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 45, 50, 55, 60])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [10, 15, 20, 25, 30])
            .MakeItFieldTripItem(45);

            LoadItem<ITM_WaterBucket>("WaterBucket", CustomItemsEnum.WaterBucket)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 60, 53, 25, 42])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 52, 48, 32, 70])
            .MakeItFieldTripItem(65);

            LoadItem<ITM_Umbrella>("Umbrella", CustomItemsEnum.Umbrella)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 70, 30, 40, 34])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 75, 35, 45, 55])
            .MakeItFieldTripItem(46);

            LoadItem<ITM_FidgetSpinner>("FidgetSpinner", CustomItemsEnum.OutdatedFidgetSpinner)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 20, 22, 12, 30])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 25, 27, 17, 35])
            .MakeItPartyItem(52)
            .MakeItFieldTripItem(70);

            LoadItem<ITM_HandHook>("HandHook", CustomItemsEnum.HandHook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 15, 30, 23, 40])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 30, 60, 46, 80])
            .MakeItPartyItem(40)
            .MakeItFieldTripItem(50);

            LoadItem<ITM_GrilledCheese>("GrilledCheese", CustomItemsEnum.GrilledCheese)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 25, 40, 40])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [35, 25, 20, 35, 35]);

            LoadItem<ITM_GenericSoda>("GenericSoda", CustomItemsEnum.GenericSoda)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [12, 21, 30, 24, 37])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [21, 12, 3, 42, 73])
            .MakeItFieldTripItem(45)
            .MakeItMysteryItem(80);

            LoadItem<ITM_GenericSoda>("DietGenericSoda", CustomItemsEnum.DietGenericSoda, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [22, 30, 38, 33, 45])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [44, 60, 76, 66, 90]);

            LoadItem<ITM_Tea>("Tea", CustomItemsEnum.Tea)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [12, 32, 20, 34, 38])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [20, 12, 22, 23, 34])
            .MakeItFieldTripItem(80)
            .MakeItPartyItem(25);

            LoadItem<ITM_Tea>("DietTea", CustomItemsEnum.DietTea, true)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 37, 40, 48, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 24, 44, 36, 68])
            .MakeItFieldTripItem(40);

            LoadItem<ITM_AdvertenceBook>("AdvertenceBook", CustomItemsEnum.AdvertenceBook)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [36, 24, 30, 28, 42])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 42, 30, 40, 55])
            .MakeItFieldTripItem(40);

            LoadItem<ITM_AdvertenceBook>("ConnectedAdvertenceBook", CustomItemsEnum.ConnectedAdvertenceBook)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [28, 24, 12, 15])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 24, 44, 36, 68])
            .MakeItFieldTripItem(40);

            //You who ask yourself, why don't you use the diet which is practically the same thing. It simply doesn't work for ANY reason. WHY??????
            CustomItemsEnum.ConnectedAdvertenceBook.ToItem().item.GetComponent<ITM_AdvertenceBook>().connected = true;

            LoadItem<ITM_Compass>("Compass", CustomItemsEnum.Compass)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 25, 40, 12, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [28, 34, 33, 48, 32])
            .MakeItFieldTripItem(22);

            LoadItem<ITM_MapPoint>("MapPoint", CustomItemsEnum.MapPoint)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 50, 30, 44, 60])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 40, 50, 20, 40]);

            LoadItem<ITM_BaseballBat>("BaseballBat", CustomItemsEnum.BaseballBat)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 40, 50, 42, 66])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [25, 42, 13, 45, 30])
            .MakeItFieldTripItem(40);

            LoadItem<ITM_SafteyGlasses>("SafteyGlasses", CustomItemsEnum.SafteyGlasses)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 20, 30, 20, 15])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [11, 22, 33, 22, 11])
            .MakeItFieldTripItem(45);

            LoadItem<ITM_FreezeClock>("FreezeClock", CustomItemsEnum.FreezeClock)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [5, 11, 10, 14, 10])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [13, 16, 17, 18, 14])
            .MakeItMysteryItem(50)
            .MakeItPartyItem(80)
            .MakeItFieldTripItem(90);

            LoadItem<ITM_Horn>("Horn", CustomItemsEnum.Horn)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 25, 40, 12, 48])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 32, 50, 12, 70]);

            LoadItem<ITM_BanHammer>("BanHammer", CustomItemsEnum.BanHammer)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 20, 30, 20, 15])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [11, 22, 33, 22, 11])
            .MakeItGuaranteedFieldTripItem()
            .MakeItMysteryItem(60);

            LoadItem<ITM_Swapper>("Swapper", CustomItemsEnum.Swapper)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [24, 30, 21, 38, 48])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [32, 45, 32, 38, 50])
            .MakeItFieldTripItem(32);

            /*
            LoadItem<ITM_Lantern>("Lantern", CustomItemsEnum.Lantern)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [24, 30, 21, 38, 48])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [32, 45, 32, 38, 50])
            .MakeItFieldTripItem(32);
            */

            LoadItem<ITM_PlayerCardboard>("PlayerCardboard", CustomItemsEnum.PlayerCardboard)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 28, 40, 25, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [45, 37, 42, 38, 52])
            .MakeItFieldTripItem(70);

            LoadItem<ITM_GatwayTeleporter>("GatewayTeleporter", CustomItemsEnum.GatewayTeleporter)
            .MakeItWeightedItem(["F2", "F3", "F4"], [20, 14, 22])
            .MakeItWeightedItemInShop(["F2", "F3", "F4"], [22, 34, 13])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(65);

            LoadItem<ITM_IceCreamStick>("FreezingIceCream", CustomItemsEnum.StickIceCream)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4"], [30, 20, 40, 48])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4"], [15, 10, 20, 24])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(65);

            LoadItem<ITM_Traumatized>("Traumatized", CustomItemsEnum.Traumatized)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [34, 40, 48, 52])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [33, 22, 11, 44])
            .MakeItFieldTripItem(72);

            LoadItem<ITM_EntityTeleporter>("EntityTeleporter", CustomItemsEnum.EntityTeleporter)
            .MakeItWeightedItem(["F1","F2", "F3", "F4", "END"], [1, 30, 42, 13, 38])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [6, 44, 22, 15, 38])
            .MakeItFieldTripItem(72);

            LoadItem<ITM_JumpBoots>("JumpBoots", CustomItemsEnum.JumpBoots)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [14, 5, 18, 2, 33])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 32, 55, 22, 60])
            .MakeItPartyItem(70)
            .MakeItFieldTripItem(100);

            /*
            LoadItem<ITM_YTPsMultiplier>("MultiplierMachine", CustomItemsEnum.YTPsMultiplier)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [6, 12, 11, 8, 10])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [12, 24, 22, 16, 20])
            .MakeItPartyItem(24)
            .MakeItFieldTripItem(25);
            */

            LoadItem<ITM_InvisblePaintBucket>("InvisiblePaintBucket", CustomItemsEnum.InvisiblePaintBucket)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 25, 44, 30, 28, 49])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 44, 50, 22, 30])
            .MakeItPartyItem(80)
            .MakeItGuaranteedFieldTripItem();

            LoadItem<ITM_PaintGun>("PaintGun", CustomItemsEnum.PaintGun)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [20, 25, 44, 30, 28, 49])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [30, 44, 50, 22, 30])
            .MakeItPartyItem(80)
            .MakeItGuaranteedFieldTripItem();

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
                if (file.Contains(item.postfixIconSmall) && !spritesDic.ContainsKey("small"))
                {
                    spritesDic.Add("small", AssetsLoader.CreateSprite(Path.GetFileNameWithoutExtension(file), Paths.GetPath(PathsEnum.Items, path), 1));
                }
                else if (file.Contains(item.postfixIconLarge) && !spritesDic.ContainsKey("large"))
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

            if (itemObj.item.GetComponent<DietItemVariation>() != null) itemObj.item.GetComponent<DietItemVariation>().diet = diet;

            if (itemObj.item.GetComponent<IItemPrefab>() != null) itemObj.item.GetComponent<IItemPrefab>().Setup();

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
                    floor.items.Add(new WeightedItemObject { selection = item, weight = Mathf.FloorToInt(weights[weight] * 0.75f) });
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
                    floor.shopItems.Add(new WeightedItemObject { selection = item, weight = Mathf.FloorToInt(weights[weight] * 0.75f)  });
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
    }
}

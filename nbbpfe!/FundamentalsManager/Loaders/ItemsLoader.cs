using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbbpfe.CustomData;
using nbbpfe.Enums;
using nbppfe.CustomContent.CustomItems;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.PrefabSystem;
using PixelInternalAPI;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static nbbpfe.FundamentalsManager.FundamentalLoaderManager;

namespace nbbpfe.FundamentalsManager.Loaders
{
    public static partial class ItemsLoader
    {
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
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [80, 30, 20, 10, 50]);

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

            LoadItem<ITM_BullyPresent>("BullyPresent", CustomItemsEnum.BullyPresent)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [35, 22, 40, 12, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [37, 30, 20, 40, 44]);

            LoadItem<ITM_Glue>("Glue", CustomItemsEnum.Glue)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 20, 10, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 10, 30, 20, 40])
            .MakeItPartyItem(75)
            .MakeItFieldTripItem(50);

            LoadItem<ITM_Pretzel>("Pretzel", CustomItemsEnum.Pretzel)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [45, 60, 55, 80])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [70, 67, 15, 95])
            .MakeItFieldTripItem(58);

            //LoadItem<ITM_SoupInCan>("SoupCan", CustomItemsEnum.CanSoup)
            //.MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [70, 80, 50, 2, 100])
            //.MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [100, 10, 80, 20, 70]);

            LoadItem<ITM_NoClipController>("NoClipController", CustomItemsEnum.NoClipController)
            .MakeItWeightedItem(["F3", "F4", "END"], [15, 12, 30])
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
            .MakeItWeightedItem(["F3", "F4", "END"], [15, 22, 40])
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
                    floor.items.Add(new WeightedItemObject { selection = item, weight = weights[weight] });
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
                    floor.shopItems.Add(new WeightedItemObject { selection = item, weight = weights[weight] });
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

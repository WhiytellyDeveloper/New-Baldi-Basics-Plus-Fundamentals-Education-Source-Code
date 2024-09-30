using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using nbbpfe.CustomData;
using nbbpfe.Enums;
using nbppfe.CustomContent.CustomItems;
using nbppfe.PrefabSystem;
using PixelInternalAPI;
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
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [30, 60, 50, 70, 55])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [80, 30, 20, 10, 50]);

            LoadItem<ITM_CoffeAndSugar>("CoffeAndSugar", CustomItemsEnum.CoffeAndSugar)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [20, 30, 40, 55])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [30, 40, 50, 20])
            .MakeItForcedItem(["F3", "F4"], [1, 2]);

            LoadItem<ITM_SweepWhistle>("SweepWhistle", CustomItemsEnum.SweepWhistle)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [15, 30, 22, 32, 18])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [40, 30, 55, 32, 30]);

            LoadItem<ITM_BullyPresent>("BullyPresent", CustomItemsEnum.BullyPresent)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [35, 22, 40, 12, 44])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [37, 30, 20, 40, 44]);

            LoadItem<ITM_Glue>("Glue", CustomItemsEnum.Glue)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [40, 30, 20, 10, 50])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [50, 10, 30, 20, 40]);

            LoadItem<ITM_Pretzel>("Pretzel", CustomItemsEnum.Pretzel)
            .MakeItWeightedItem(["F2", "F3", "F4", "END"], [45, 60, 55, 80])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [70, 67, 15, 95]);

            //LoadItem<ITM_SoupInCan>("SoupCan", CustomItemsEnum.CanSoup)
            //.MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [70, 80, 50, 2, 100])
            //.MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [100, 10, 80, 20, 70]);

            LoadItem<ITM_NoClipController>("NoClipController", CustomItemsEnum.NoClipController)
            .MakeItWeightedItem([ "F3", "F4", "END"], [15, 12, 30])
            .MakeItWeightedItemInShop(["F2", "F3", "F4", "END"], [20, 32, 5, 40]);

            LoadItem<ITM_Walkman>("Walkman", CustomItemsEnum.Walkman)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [24, 44, 37, 30, 45])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [15, 20, 25, 30, 35]);

            LoadItem<ITM_WhiteZesty>("WhiteZesty", CustomItemsEnum.WhiteZesty)
            .MakeItWeightedItem(["F1", "F2", "F3", "F4", "END"], [50, 40, 60, 70, 65])
            .MakeItWeightedItemInShop(["F1", "F2", "F3", "F4", "END"], [54, 44, 64, 74, 24])
            .MakeItForcedItem(["F1", "END"], [1, 1]);
        }

        public static ItemObject LoadItem<T>(string path, CustomItemsEnum itemEnum) where T : Item
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
                if (file.Contains(item.postfixIconSmall))
                    spritesDic.Add("small", AssetsLoader.CreateSprite(Path.GetFileNameWithoutExtension(file), Paths.GetPath(PathsEnum.Items, path), 1));
                else if (file.Contains(item.postfixIconLarge))
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
    }
}

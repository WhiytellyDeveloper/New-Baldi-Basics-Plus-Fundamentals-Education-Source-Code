using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Cookie : Item, IItemPrefab
    {
        public void Setup()
        {
            audUse = FundamentalLoaderManager.GenericEatSound;

            var usedItem = CustomItemsEnum.Cookie.ToItem().Duplicate();
            usedItem.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
            usedItem.nameKey = "Itm_Cookie1";
            usedItem.item = usedItem.item.DuplicatePrefab();
            usedItem.item.GetComponent<ITM_Cookie>().usedItem = null;
            this.usedItem = usedItem;
            CustomItemsEnum.Cookie.ToItem().itemSpriteLarge = AssetsLoader.CreateSprite("Cookie_IconLarge", Paths.GetPath(PathsEnum.Items, "Cookie"), 50);
            CustomItemsEnum.Cookie.ToItem().itemSpriteSmall = AssetsLoader.CreateSprite("Cookie_IconSmall", Paths.GetPath(PathsEnum.Items, "Cookie"), 1);
        }

        public override bool Use(PlayerManager pm)
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audUse);
            pm.plm.AddStamina(100, true);

            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }

        public SoundObject audUse;
        public ItemObject usedItem;
    }
}

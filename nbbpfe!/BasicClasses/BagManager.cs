using MTM101BaldAPI.Reflection;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.BasicClasses
{
    public class BagManager : Singleton<BagManager>
    {
        public void Awake()
        {
            audOpen = AssetsLoader.CreateSound("BagOpen", Paths.GetPath(PathsEnum.Items, "Bag"), "Sfx_BagOpen", SoundType.Effect, Color.white, 1);
            audClose = AssetsLoader.CreateSound("BagClose", Paths.GetPath(PathsEnum.Items, "Bag"), "Sfx_BagClose", SoundType.Effect, Color.white, 1);
        }

        public void Open(int player)
        {
            if (!openBag)
            {
                openBag = true;
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audOpen);
                var selectedPlayer = Singleton<CoreGameManager>.Instance.GetPlayer(player);
                playerItems = selectedPlayer.itm.items;
                selectedPlayer.itm.items = bagItems;
                selectedPlayer.itm.UpdateItems();
                selectedPlayer.itm.UpdateSelect();
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem++;
                int newSize = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem + 1;
                Singleton<CoreGameManager>.Instance.GetHud(0).UpdateInventorySize(newSize);
            }
        }

        public void Close(int player)
        {
            if (openBag)
            {
                openBag = false;
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audClose);
                var selectedPlayer = Singleton<CoreGameManager>.Instance.GetPlayer(player);
                bagItems = selectedPlayer.itm.items;
                selectedPlayer.itm.items = playerItems;
                selectedPlayer.itm.UpdateItems();
                selectedPlayer.itm.UpdateSelect();
                playerItems = bagItems;
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem--;
                int newSize = Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem + 1;
                Singleton<CoreGameManager>.Instance.GetHud(0).UpdateInventorySize(newSize);

            }
        }

        public void Switch(int player)
        {
            switch (openBag)
            {
                case true:
                    Close(player);
                    break;
                case false:
                    Open(player);
                    break;
            }
        }

        public ItemObject[] bagItems = [
            CustomItemsEnum.OpenBag.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem(),
            Items.None.ToItem()
        ];

        public ItemObject[] playerItems = [];
        public bool openBag;
        public SoundObject audOpen, audClose;
    }
}

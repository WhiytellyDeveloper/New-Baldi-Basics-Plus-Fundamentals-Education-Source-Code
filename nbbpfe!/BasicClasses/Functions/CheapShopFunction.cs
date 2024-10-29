using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace nbppfe.BasicClasses.Functions
{
    public class CheapShopFunction : RoomFunction
    {
        private void Awake()
        {
            var items = Resources.FindObjectsOfTypeAll<ItemObject>();

            foreach (var item in items)
                originalDescriptions.Add(item, item.descKey);

            audBell = (SoundObject)Resources.FindObjectsOfTypeAll<StoreRoomFunction>().FirstOrDefault().ReflectionGetVariable("audBell");
        }

        public override void OnGenerationFinished()
        {
            base.OnGenerationFinished();
            foreach (Pickup pickup in pickups)
                TogglePriceDescription(pickup, false);

            foreach (Pickup pickup in room.ec.items)
            {
                if (room.ec.CellFromPosition(pickup.transform.position).room == room)
                {
                    pickup.OnItemPurchased += ItemPurchased;
                    pickups.Add(pickup);
                }
            }

            foreach (MeshRenderer renderers in room.objectObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (renderers.name.Contains("Counter"))
                    renderers.material.mainTexture = AssetsLoader.Get<Texture2D>("CheapStoreCounterAtlas");
            }
            Restock();
        }

        public void Restock()
        {
            var sceneObject = Singleton<CoreGameManager>.Instance.sceneObject;
            var itemList = sceneObject != null ? sceneObject.shopItems : null;

            if (sceneObject != null && sceneObject.storeUsesNextLevelData)
                itemList = Singleton<CoreGameManager>.Instance.nextLevel.shopItems;

            if (itemList == null || itemList.Length == 0)
            {
                sceneObject = Resources.FindObjectsOfTypeAll<SceneObject>().Where(x => x.name.Contains("3")).FirstOrDefault();
                itemList = sceneObject != null ? sceneObject.shopItems : null;
            }

            if (itemList == null || itemList.Length == 0)
            {
                Debug.LogWarning("Nenhum item disponível para reabastecer a loja.");
                return;
            }

            List<WeightedSelection<ItemObject>> availableItems = itemList
                .Select(item => new WeightedSelection<ItemObject> { selection = item.selection, weight = item.weight })
                .ToList();

            foreach (Pickup pickup in pickups)
            {
                if (availableItems.Count == 0)
                    break;

                ItemObject selectedItem = WeightedSelection<ItemObject>.ControlledRandomSelectionList(availableItems, new System.Random(Singleton<CoreGameManager>.Instance.Seed()));

                pickup.icon.spriteRenderer.enabled = false;
                pickup.AssignItem(selectedItem);
                pickup.free = false;
                pickup.price = ((int)(pickup.item.price / 2.2f));
                pickup.showDescription = true;
                TogglePriceDescription(pickup, true);
                availableItems.RemoveAll(w => w.selection == selectedItem);
            }
        }



        private void TogglePriceDescription(Pickup pickup, bool showPrice)
        {
            if (showPrice)
            {
                string currentDescription = originalDescriptions[pickup.item];
                if (!currentDescription.Contains("\nCost:"))
                    pickup.item.descKey = $"{Singleton<LocalizationManager>.Instance.GetLocalizedText(currentDescription)}\nCost: {pickup.price}";
            }
            else
                pickup.item.descKey = originalDescriptions[pickup.item];
        }

        public void DisableCheapStore(bool value)
        {
            foreach (var cell in room.cells)
                cell.SetLight(!value);

            foreach (Door door in room.doors)
            {
                if (value)
                    door.Lock(false);
                else
                    door.Unlock();
            }
        }

        public override void OnPlayerEnter(PlayerManager player)
        {
            base.OnPlayerEnter(player);
            Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).PointsAnimator.ShowDisplay(true);

            if (closeOnExit)
                DisableCheapStore(false);

            foreach (Pickup pickup in pickups)
                TogglePriceDescription(pickup, true);
        }

        public override void OnPlayerExit(PlayerManager player)
        {
            base.OnPlayerExit(player);
            Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).PointsAnimator.ShowDisplay(false);

            foreach (Pickup pickup in pickups)
                TogglePriceDescription(pickup, false);

            if (closeOnExit)
                DisableCheapStore(true);
        }

        private void OnDestroy()
        {
            foreach (Pickup pickup in pickups)
                TogglePriceDescription(pickup, false);
        }

        private void ItemPurchased(Pickup pickup, int player)
        {
            foreach (Pickup _pickup in pickups)
                _pickup.gameObject.SetActive(false);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audBell);
            closeOnExit = true;
        }

        private Dictionary<ItemObject, string> originalDescriptions = [];
        private List<Pickup> pickups = [];
        public SoundObject audBell;
        public bool closeOnExit;
    }
}

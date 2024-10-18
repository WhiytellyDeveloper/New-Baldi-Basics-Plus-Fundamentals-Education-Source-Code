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

            foreach(var item in items)
                originalDescriptions.Add(item, item.descKey);
            
        }

        public override void OnGenerationFinished()
        {
            base.OnGenerationFinished();

            foreach (Pickup pickup in room.ec.items)
            {
                if (room.ec.CellFromPosition(pickup.transform.position).room == room)
                    pickups.Add(pickup);
            }

            Restock();
        }

        public void Restock()
        {
            var itemList = Resources.FindObjectsOfTypeAll<SceneObject>().Where(x => x.name.Contains("2")).FirstOrDefault().shopItems;

            if (Singleton<CoreGameManager>.Instance.sceneObject.storeUsesNextLevelData)
                itemList = Singleton<CoreGameManager>.Instance.nextLevel.shopItems;

            if (itemList.Length == 0 || itemList == null)
                itemList = Resources.FindObjectsOfTypeAll<SceneObject>().Where(x => x.name.Contains("1")).FirstOrDefault().shopItems;

            foreach (Pickup pickup in pickups)
            {
                pickup.icon.spriteRenderer.enabled = false;
                pickup.AssignItem(WeightedSelection<ItemObject>.RandomSelection(itemList));
                pickup.free = false;
                pickup.price = ((int)(pickup.item.price / UnityEngine.Random.Range(2, 4.2f)));
                pickup.showDescription = true;
                TogglePriceDescription(pickup, true);
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

        public override void OnPlayerEnter(PlayerManager player)
        {
            base.OnPlayerEnter(player);

            Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).PointsAnimator.ShowDisplay(true);
        }

        private Dictionary<ItemObject, string> originalDescriptions = [];
        private List<Pickup> pickups = [];
    }
}

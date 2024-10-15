using MTM101BaldAPI.Registers;
using nbbpfe.Enums;
using nbppfe.Extensions;
using nbppfe.Patches;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Present : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (PickupPatch.lastPíckupClicked.item.itemType == CustomItemsEnum.Present.ToItemEnum())
            {
                for (int i = 0; i < ItemMetaStorage.Instance.All().Length; i++)
                    items.Add(ItemMetaStorage.Instance.All()[i].value);

                Dictionary<ItemObject, int> itemCounts = new Dictionary<ItemObject, int>();

                foreach (ItemObject item in items)
                {
                    if (itemCounts.ContainsKey(item))
                        itemCounts[item]++;
                    else
                        itemCounts[item] = 1;
                }

                foreach (var pair in itemCounts)
                {
                    if (pair.Value > 1)
                    {
                        for (int i = 0; i < pair.Value - 1; i++)
                            items.Remove(pair.Key);
                    }
                }

                items.Shuffle();
                items.Remove(Items.BusPass.ToItem());

                pm.ec.CreateItem(pm.plm.Entity.CurrentRoom, items[Random.Range(0, items.Count)], new Vector2(PickupPatch.lastPíckupClicked.transform.position.x, PickupPatch.lastPíckupClicked.transform.position.z));
                Destroy(gameObject);
            }
            return false;
        }



        public List<ItemObject> items = [];
    }
}

using MTM101BaldAPI.Registers;
using nbbpfe.Enums;
using nbppfe.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Present : Item
    {
        public override bool Use(PlayerManager pm)
        {
            for (int i = 0; i < ItemMetaStorage.Instance.All().Length; i++)
            {
                if (ItemMetaStorage.Instance.All()[i].value.addToInventory)
                    items.Add(ItemMetaStorage.Instance.All()[i].value);
            }

            items.Shuffle();
            pm.itm.AddItem(items[Random.Range(0, items.Count - 1)]);
            Destroy(gameObject);
            return false;
        }


        public List<ItemObject> items;
    }
}

using MTM101BaldAPI.Registers;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_BullyPresent : Item
    {
        public override bool Use(PlayerManager pm)
        {
            for (int i = 0; i < ItemMetaStorage.Instance.All().Length; i++)
            {
                if (ItemMetaStorage.Instance.All()[i].value.addToInventory && ItemMetaStorage.Instance.All()[i].value.price <= 200)
                    items.Add(ItemMetaStorage.Instance.All()[i].value);
            }

            pm.itm.RemoveItem(pm.itm.selectedItem);
            items.Shuffle();
            pm.itm.AddItem(items[Random.Range(0, items.Count - 1)]);
            Destroy(gameObject);
            return false;
        }


        public List<ItemObject> items;
    }
}

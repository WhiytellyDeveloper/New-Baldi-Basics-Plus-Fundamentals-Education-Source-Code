using MTM101BaldAPI.Registers;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_BaseballBat : NPCItem, IItemPrefab
    {
        public void Setup()
        {
            hitSound = AssetsLoader.CreateSound("BaseballHitSound", Paths.GetPath(PathsEnum.Items, "BaseballBat"), "Sfx_BaseballHit", SoundType.Effect, Color.white, 1);
            windHitSound = FundamentalLoaderManager.GenericAirSound;
            var previousItem = (ItemObject)null;

            for (int i = 1; i <= 7; i++)
            {
                var currentItem = CustomItemsEnum.BaseballBat.ToItem().Duplicate();
                currentItem.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                currentItem.nameKey = $"Itm_BaseballBat{i}";
                currentItem.item = currentItem.item.DuplicatePrefab();
                currentItem.item.GetComponent<ITM_BaseballBat>().hitSound = hitSound;
                if (previousItem != null)
                    currentItem.item.GetComponent<ITM_BaseballBat>().usedItem = previousItem;                
                previousItem = currentItem;
            }

            usedItem = previousItem;
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(hitSound);
            npc.Navigator.Entity.AddForce(new Force(pm.GetPlayerCamera().transform.forward, 40, -28));

            if (usedItem == null)
                return true;
            else
                pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }

        public override void OnMissNPC()
        {
            base.OnMissNPC();
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(windHitSound);
        }

        public ItemObject usedItem;
        public SoundObject hitSound, windHitSound;
    }
}

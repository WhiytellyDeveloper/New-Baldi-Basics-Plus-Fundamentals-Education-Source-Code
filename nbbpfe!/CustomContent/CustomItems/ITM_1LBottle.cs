using MTM101BaldAPI.Reflection;
using nbppfe.PrefabSystem;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_1LBottle : Item, IItemPrefab
    {
        public void Setup() 
        {
            var fountain = Resources.FindObjectsOfTypeAll<WaterFountain>().FirstOrDefault();
            drikingSound = (SoundObject)fountain.ReflectionGetVariable("audSip");
        }

        public override bool Use(PlayerManager pm)
        {
            Destroy(gameObject);

            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out var hit, pm.pc.reach))
            {
                var water = hit.transform.GetComponent<WaterFountain>();
                if (water)
                {
                    this.pm = pm;
                    InteractWithFountain(water);

                    if (nextItem == null)
                        return false;

                    pm.itm.SetItem(nextItem, pm.itm.selectedItem);
                    return false;
                }
            }

            if (drinkable)
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(drikingSound);
                pm.plm.AddStamina(100, true);

                if (previousItem == null)
                    return false;

                pm.itm.SetItem(previousItem, pm.itm.selectedItem);
            }

            return false;
        }

       public void InteractWithFountain(WaterFountain fountain)
       {
            var audMan = fountain.ReflectionGetVariable("audMan") as AudioManager;
            audMan.PlaySingle(drikingSound);
       }

        public SoundObject drikingSound;
        public ItemObject nextItem, previousItem;
        public bool drinkable;
    }

}

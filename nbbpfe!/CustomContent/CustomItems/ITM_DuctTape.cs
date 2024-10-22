using nbppfe.Patches;
using nbppfe.PrefabSystem;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_DuctTape : Item, IItemPrefab
    {
        public void Setup() =>
            slapSound = Resources.FindObjectsOfTypeAll<SoundObject>().Where(x => x.name == "Slap").First();


        //-------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            if (LastRemovedItemPatch.lastRemovedItem != null)
            {
                pm.itm.SetItem(LastRemovedItemPatch.lastRemovedItem, pm.itm.selectedItem);
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(slapSound);
            }
            return false;
        }

        public SoundObject slapSound;
    }
}

using nbbpfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_WhiteZesty : Item, IItemPrefab
    {
        public void Setup() =>
            eatSound = FundamentalLoaderManager.GenericEatSound;



        public override bool Use(PlayerManager pm)
        {
            pm.plm.stamina = Random.Range(1, 250);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(eatSound);
            return true;
        }

        public SoundObject eatSound;
    }
}

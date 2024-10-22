using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Cheese : Item, IItemPrefab
    {
        public void Setup() =>
            useSound = AssetsLoader.CreateSound("eat_cheese", Paths.GetPath(PathsEnum.Items, "Cheese"), "", SoundType.Effect, Color.white, 1);


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(useSound);
            pm.plm.AddStamina(pm.plm.stamina, false);
            Destroy(gameObject);
            return true;
        }

        public SoundObject useSound;
    }
}

using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_CoffeAndSugar : Item, IItemPrefab
    {
        public void Setup()
        {
            drinkingSound = FundamentalLoaderManager.GenericDrinkingSound;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            pm.plm.stamina = 350;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(drinkingSound);
            Destroy(gameObject);
            return true;
        }

        public SoundObject drinkingSound;
    }
}

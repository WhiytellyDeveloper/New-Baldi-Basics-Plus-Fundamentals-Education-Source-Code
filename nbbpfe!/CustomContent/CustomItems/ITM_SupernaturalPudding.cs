using nbppfe.FundamentalsManager;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_SupernaturalPudding : Item
    {
        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericEatSound);
            initialStamina = pm.plm.stamina;
            pm.SetInvisible(true);
            return true;
        }

        private void Update()
        {
            if (pm.plm.stamina <= initialStamina / 2)
            {
                pm.SetInvisible(false);
                Destroy(gameObject);
            }
        }

        public float initialStamina;
    }
}

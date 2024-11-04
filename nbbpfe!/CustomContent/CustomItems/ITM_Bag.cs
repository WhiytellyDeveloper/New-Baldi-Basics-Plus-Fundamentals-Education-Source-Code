using MTM101BaldAPI;
using nbppfe.BasicClasses;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.PrefabSystem;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Bag : Item
    {
        public override bool Use(PlayerManager pm)
        {
            CheckForBagManager().Switch(pm.playerNumber);
            Destroy(gameObject);
            return false;
        }

        public BagManager CheckForBagManager()
        {
            var bagManager = Singleton<CoreGameManager>.Instance.gameObject.GetOrAddComponent<BagManager>();
            return bagManager;
        }
    }
}

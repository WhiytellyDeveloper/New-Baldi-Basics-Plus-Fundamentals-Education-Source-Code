
namespace nbppfe.CustomItems
{
    public class ITM_SoupInCan : Item
    {
        public override bool Use(PlayerManager pm)
        {
            pm.plm.AddStamina(50, true);
            return true;
        }
    }
}

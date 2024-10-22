using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Components;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Coffe : Item
    {
        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = EffectsEnd;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericDrinkingSound);
            pm.plm.stamina = 500;
            pm.GetMovementStatModifier().AddModifier("staminaDrop", staminaDrop);
            pm.GetMovementStatModifier().AddModifier("staminaRise", staminaRise);
            return true;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);


        public void EffectsEnd()
        {
            pm.GetMovementStatModifier().RemoveModifier(staminaDrop);
            pm.GetMovementStatModifier().RemoveModifier(staminaRise);
            Destroy(gameObject);
        }

        public Cooldown cooldown = new Cooldown(30, 0);
        public ValueModifier staminaDrop = new ValueModifier(0.2f), staminaRise = new ValueModifier(1.75f);
    }
}

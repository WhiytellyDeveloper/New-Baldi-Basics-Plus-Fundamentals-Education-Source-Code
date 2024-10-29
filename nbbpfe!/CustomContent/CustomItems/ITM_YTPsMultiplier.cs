using nbppfe.FundamentalSystems;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_YTPsMultiplier : Item
    {
        public override bool Use(PlayerManager pm)
        {
            cooldonw.endAction = OnCooldownEnd;
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.AddMultiplier(1.8f);
            return true;
        }

        private void Update() =>
            cooldonw.UpdateCooldown(pm.ec.EnvironmentTimeScale);

        private void OnCooldownEnd()
        {
            Singleton<CoreGameManager>.Instance.RemoveMultiplier(1.8f);
            Destroy(gameObject);
        }

        public Cooldown cooldonw = new Cooldown(10, 0);
    }
}

using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_GrilledCheese : Item
    {
        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = CooldownEnd;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericEatSound);
            foreach(NPC npc in pm.ec.Npcs)
            {
                if (npc.GetComponent<PropagatedAudioManager>() != null)
                {
                    var audMan = npc.GetComponent<PropagatedAudioManager>();
                    float newMinDistance = (float)audMan.ReflectionGetVariable("minDistance") * 2f;
                    float newMaxDistance = (float)audMan.ReflectionGetVariable("maxDistance") * 2f;

                    audMan.ReflectionSetVariable("minDistance", newMinDistance);
                    audMan.ReflectionSetVariable("maxDistance", newMaxDistance);
                }
            }

            return true;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        
        public void CooldownEnd()
        {
            foreach (NPC npc in pm.ec.Npcs)
            {
                var audMan = npc.GetComponent<PropagatedAudioManager>();
                float newMinDistance = (float)audMan.ReflectionGetVariable("minDistance") / 1.8f;
                float newMaxDistance = (float)audMan.ReflectionGetVariable("maxDistance") / 1.8f;

                audMan.ReflectionSetVariable("minDistance", newMinDistance);
                audMan.ReflectionSetVariable("maxDistance", newMaxDistance);
            }
        }

        public Cooldown cooldown = new Cooldown(25, 0);
    }
}

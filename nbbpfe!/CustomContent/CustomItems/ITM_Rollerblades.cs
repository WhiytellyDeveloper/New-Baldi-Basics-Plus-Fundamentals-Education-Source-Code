using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Rollerblades : Item, IItemPrefab
    {
        public void Setup()
        {
            var audMan = gameObject.CreateAudioManager(40, 70);
            var soundObj = AssetsLoader.CreateSound("RollerbladesLoop", Paths.GetPath(PathsEnum.Items, "Rollerblades"), "", SoundType.Effect, Color.white, 1);
            audMan.AddStartingAudiosToAudioManager(true, [soundObj]);
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;
            pm.Am.moveMods.Add(moveMod);
            return true;
        }


        private void Update()
        {
            speed = 22;
            if (Singleton<InputManager>.Instance.GetDigitalInput("Run", false) && pm.plm.stamina > 0)
            {
                speed = 35f;
                pm.plm.stamina = Mathf.Max(pm.plm.stamina - pm.plm.staminaDrop * Time.deltaTime * pm.PlayerTimeScale, 0f);
                pm.RuleBreak("Running", 0.1f);
            }

            moveMod.movementAddend = pm.GetPlayerCamera().transform.forward * speed;

            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        }

        public void OnCooldownEnd()
        {
            pm.Am.moveMods.Remove(moveMod);
            Destroy(gameObject);
        }

        public float speed = 22f;
        public Cooldown cooldown = new(30, 0);
        private MovementModifier moveMod = new(Vector3.zero, 0f) { forceTrigger = true };
    }
}

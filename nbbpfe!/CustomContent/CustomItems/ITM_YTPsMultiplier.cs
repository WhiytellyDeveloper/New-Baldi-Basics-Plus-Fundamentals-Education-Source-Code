using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_YTPsMultiplier : Item, IItemPrefab
    {
        public void Setup()
        {
            audMan = gameObject.CreateAudioManager(100, 100);
            var sound = AssetsLoader.CreateSound("MachineLoop", Paths.GetPath(PathsEnum.Items, "MultiplierMachine"), "", SoundType.Effect, Color.white, 1);
            audMan.AddStartingAudiosToAudioManager(false, [sound]);
            audMan.MakeAudioManagerNonPositional();
        }

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
        public AudioManager audMan;
    }
}

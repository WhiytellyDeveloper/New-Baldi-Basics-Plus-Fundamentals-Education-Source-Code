using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomEvents
{
    public class RandomiclyTimeEvent : RandomEvent, IEventPrefab
    {
        public void Setup() {
            slowSound = AssetsLoader.CreateSound("Slowdown", Paths.GetPath(PathsEnum.Events, "RadomiclyTime"), "", SoundType.Effect, Color.white, 1);
            speedUpSound = AssetsLoader.CreateSound("Speedup", Paths.GetPath(PathsEnum.Events, "RadomiclyTime"), "", SoundType.Effect, Color.white, 1);
        }

//---------------------------------------------------------------------------------------------------------------

        public override void Initialize(EnvironmentController controller, System.Random rng)
        {
            base.Initialize(controller, rng);
            cooldown = new Cooldown(5, 0, ChangeTimeScale, null, true, true);
        }   

        public override void Begin()
        {
            base.Begin();
            cooldown.Pause(false);
        }

        public override void End()
        {
            base.End();
            cooldown.Pause(true);
            ec.RemoveTimeScale(timeScale);
        }

        public void ChangeTimeScale()
        {
            if (timeScale != null)
                ec.RemoveTimeScale(timeScale);

            float randomTimeScale = Random.Range(0.4f, 2.4f);

            if (timeScale != null)
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(randomTimeScale < timeScale.environmentTimeScale ? slowSound : speedUpSound);

            timeScale = new TimeScaleModifier
            {
                environmentTimeScale = randomTimeScale,
                npcTimeScale = randomTimeScale,
                playerTimeScale = randomTimeScale
            };

            ec.AddTimeScale(timeScale);
            cooldown.Restart();
        }

        private void Update()
        {
            cooldown.UpdateCooldown(1);
        }

        public TimeScaleModifier timeScale;
        public Cooldown cooldown;
        public SoundObject slowSound, speedUpSound;
    }
}

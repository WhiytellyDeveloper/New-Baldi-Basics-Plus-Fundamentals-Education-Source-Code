using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_ImmatureApple : Item, IItemPrefab
    {
        public void Setup()
        { 
            appleSprites = TextureExtensions.LoadSpriteSheet(2, 1, 32, Paths.GetPath(PathsEnum.Items, ["ImmatureApple", "BaldiGreenApple.png"]));
            Debug.Log(appleSprites.Length);
        }

        public void TakeGreenApple(Baldi baldi)
        {
            appleSprites = TextureExtensions.LoadSpriteSheet(2, 1, 32, Paths.GetPath(PathsEnum.Items, ["ImmatureApple", "BaldiGreenApple.png"]));
            baldi.behaviorStateMachine.ChangeState(new Baldi_GreenApple(baldi, baldi, baldi.behaviorStateMachine.CurrentState, appleSprites));
            base.StopAllCoroutines();
            baldi.Navigator.SetSpeed(0f);
            var audMan = baldi.ReflectionGetVariable("audMan") as AudioManager;
            var appleThanks = baldi.ReflectionGetVariable("audAppleThanks") as SoundObject;
            audMan.FlushQueue(true);
            audMan.QueueAudio(appleThanks);
            baldi.GetComponent<Animator>().enabled = false;
            baldi.spriteRenderer[0].sprite = appleSprites[0];
        }


       protected Sprite[] appleSprites = [];
    }

    public class Baldi_GreenApple : Baldi_SubState
    {
        protected Sprite[] appleSprites = [];
        public Baldi_GreenApple(NPC npc, Baldi baldi, NpcState previousState, Sprite[] appleSprites) : base(npc, baldi, previousState)
        {
            this.appleSprites = appleSprites;
        }

        public override void Initialize()
        {
            base.Initialize();
            time = 5;
            
        }

        public IEnumerator EatingAnnimation()
        {
            while (startedEating)
            {
                baldi.spriteRenderer[0].sprite = appleSprites[0];
                yield return new WaitForSeconds(0.05f);
                baldi.EatSound();
                baldi.spriteRenderer[0].sprite = appleSprites[1];
                yield return new WaitForSeconds(0.05f);
            }
        }

        public override void Update()
        {
            base.Update();

            if (startedEating)
            {
                time -= Time.deltaTime * npc.TimeScale;
                if (time <= 0f)
                {
                    baldi.GetComponent<Animator>().enabled = true;
                    startedEating = false;
                    npc.behaviorStateMachine.ChangeState(previousState);
                }
            }

            if (!baldi.AudMan.QueuedAudioIsPlaying && !startedEating)
            {
                startedEating = true;
                baldi.StartCoroutine(EatingAnnimation());
            }
        }

        public float time;
        public bool startedEating;
    }

}

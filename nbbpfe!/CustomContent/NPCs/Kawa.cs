using nbbpfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using System.Collections;
using UnityEngine;
using static UnityEngine.UIElements.UIR.Implementation.UIRStylePainter;

namespace nbppfe.CustomContent.NPCs
{
    public class Kawa : NPC, INPCPrefab
    {
        public void Setup()
        {
            idleSequence[0] = AssetsLoader.Get<Sprite>("Kawa_0");
            idleSequence[1] = AssetsLoader.Get<Sprite>("Kawa_1");
            idleSequence[2] = AssetsLoader.Get<Sprite>("Kawa_2");
            shieldSequence[0] = AssetsLoader.Get<Sprite>("Kawa_3");
            shieldSequence[1] = AssetsLoader.Get<Sprite>("Kawa_4");
            shieldSequence[2] = AssetsLoader.Get<Sprite>("Kawa_5");

            audMan = GetComponent<AudioManager>();
            pushSound = AssetsLoader.Get<SoundObject>("Kawa_Nope");
            pickupSound = AssetsLoader.Get<SoundObject>("Kawa_shieldPickup");
        }

        public void PostLoading() =>
            cooldown = new Cooldown(15, 0, SwapState);

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new Kawa_Wandering(this));
            StartCoroutine(BaseAnimation());
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();

            if (!useShield)
                spriteRenderer[0].sprite = idleSequence[index];
            if (useShield)
                spriteRenderer[0].sprite = shieldSequence[index];

            cooldown.UpdateCooldown(ec.NpcTimeScale);
        }

        public IEnumerator BaseAnimation()
        {
            int[] indices = { 0, 1, 0, 2, 0, 1, 0, 2 };

            while (true)
            {
                foreach (int idx in indices)
                {
                    index = idx;
                    yield return new WaitForSeconds(0.2f);
                }
            }

        }

        public void SwapState()
        {
            useShield = !useShield;
            audMan.PlaySingle(pickupSound);
            cooldown.Restart();
        }

        protected override void VirtualOnTriggerEnter(Collider other)
        {
            base.VirtualOnTriggerEnter(other);

            if (other.CompareTag("Player") && useShield)
            {
                if (!other.GetComponent<PlayerManager>().tagged)
                {
                    other.GetComponent<PlayerManager>().plm.Entity.AddForce(new Force((other.GetComponent<PlayerManager>().plm.Entity.transform.position - transform.position).normalized, 42, -42));
                    audMan.PlaySingle(pushSound);
                }
            }
        }

        public Cooldown cooldown;
        public Sprite[] idleSequence = new Sprite[3];
        public Sprite[] shieldSequence = new Sprite[3];
        public int index;
        public bool useShield;
        public AudioManager audMan;
        public SoundObject pickupSound;
        public SoundObject pushSound;
    }

    public class Kawa_StateBase : NpcState
    {
        protected Kawa kw;
        public Kawa_StateBase(Kawa kawa) : base(kawa) { npc = kawa; kw = kawa; }
    }

    public class Kawa_Wandering : Kawa_StateBase
    {
        public Kawa_Wandering(Kawa kawa) : base (kawa) { }

        public override void Initialize()
        {
            base.Initialize();
            ChangeNavigationState(new NavigationState_WanderRandom(kw, 0));
        }
    }
}

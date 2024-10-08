using nbbpfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs
{
    public class Kawa : NPC, INPCPrefab
    {
        public void Setup()
        {
            for (int i = 0; i < 3; i++)
            {
                idleSequence[i] = AssetsLoader.Get<Sprite>($"Kawa_{i}");
                shieldSequence[i] = AssetsLoader.Get<Sprite>($"Kawa_{i + 3}");
            }

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

            spriteRenderer[0].sprite = useShield ? shieldSequence[index] : idleSequence[index];
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

        public void Push(Entity entity)
        {
            audMan.PlaySingle(pushSound);
            entity.AddForce(new Force((entity.transform.position - transform.position).normalized, 62, -62));
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

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.CompareTag("Player") && kw.useShield)
            {
                PlayerManager pm = other.GetComponent<PlayerManager>();
                if (!pm.tagged)
                    kw.Push(pm.plm.Entity);
            }
        }
    }
}

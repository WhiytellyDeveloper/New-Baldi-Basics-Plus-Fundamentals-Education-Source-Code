using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs
{
    public class DigitalArtist : NPC, INPCPrefab
    {
        public void Setup()
        {
            idle = AssetsLoader.Get<Sprite>("DigitalArtistic_0");
            hey = AssetsLoader.Get<Sprite>("DigitalArtistic_1");
            happy = AssetsLoader.Get<Sprite>("DigitalArtistic_3");
            look = AssetsLoader.Get<Sprite>("DigitalArtistic_2");
            unlooked = AssetsLoader.Get<Sprite>("DigitalArtistic_4");

            audMan = GetComponent<AudioManager>();

            heyVoiceline = AssetsLoader.Get<SoundObject>("Dia_Hey");
            SubtitleTimedKey heyTimedKey = new SubtitleTimedKey { key = "Dia_Hey2", time = 0.292f };
            heyVoiceline.additionalKeys = [heyTimedKey];

            unlookedVoiceline = AssetsLoader.Get<SoundObject>("Dia_Unlooked");
        }

        public void PostLoading()
        {
            lookingTime = new Cooldown(20, 0);
            cooldown = new Cooldown(30, 0, null, null, false, true);
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new DigitalArtist_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            cooldown.UpdateCooldown(TimeScale);
        }

        public IEnumerator SpotSpriteSequence()
        {
            audMan.QueueAudio(heyVoiceline);
            spriteRenderer[0].sprite = hey;
            yield return new WaitForSeconds(0.292f);
            spriteRenderer[0].sprite = look;
        }

        public Sprite idle, hey, look, happy, unlooked;
        public AudioManager audMan;
        public SoundObject heyVoiceline, unlookedVoiceline;
        public Cooldown cooldown, lookingTime;
    }

    public class DigitalArtist_BaseState : NpcState
    {
        protected DigitalArtist dia;
        public DigitalArtist_BaseState(DigitalArtist npc) : base(npc) { this.npc = npc; dia = npc; }
    }

    public class DigitalArtist_Wandering : DigitalArtist_BaseState
    {
        public DigitalArtist_Wandering(DigitalArtist npc) : base(npc) { }

        public override void Initialize()
        {
            base.Initialize();
            dia.Navigator.SetSpeed(12, 12);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }

        public override void PlayerInSight(PlayerManager player)
        {
            base.PlayerInSight(player);

            if (dia.cooldown.cooldownIsEnd)
                dia.behaviorStateMachine.ChangeState(new DigitalArtist_Spot(dia, player));
        }
    }

    public class DigitalArtist_Spot : DigitalArtist_BaseState
    {
        protected PlayerManager player;

        public DigitalArtist_Spot(DigitalArtist npc, PlayerManager player) : base(npc) { this.player = player; }

        public override void Initialize()
        {
            base.Initialize();
            dia.Navigator.SetSpeed(5, 7);
            dia.lookingTime.Restart();
            dia.StartCoroutine(dia.SpotSpriteSequence());
        }

        public override void Update()
        {
            base.Update();
            ChangeNavigationState(new NavigationState_TargetPlayer(dia, 0, player.transform.position));
            dia.lookingTime.UpdateCooldown(dia.TimeScale);

            if (dia.lookingTime.cooldownIsEnd)
            {
                dia.spriteRenderer[0].sprite = dia.happy;
                dia.lookingTime.Restart();
                dia.cooldown.Restart();
                dia.behaviorStateMachine.ChangeState(new DigitalArtist_Wandering(dia));
            }
        }

        public override void Unsighted()
        {
            base.Unsighted();
            dia.behaviorStateMachine.ChangeState(new DigitalArtist_Unlooked(dia, player));
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == player.gameObject)
                dia.Navigator.Entity.SetFrozen(true);
        }

        public override void OnStateTriggerExit(Collider other)
        {
            base.OnStateTriggerExit(other);

            if (other.gameObject == player.gameObject)
                dia.Navigator.Entity.SetFrozen(false);
        }
    }

    public class DigitalArtist_Unlooked : DigitalArtist_BaseState
    {
        protected PlayerManager pm;
        public DigitalArtist_Unlooked(DigitalArtist npc, PlayerManager player) : base(npc) { pm = player; }

        public override void Initialize()
        {
            base.Initialize();
            dia.Navigator.SetSpeed(14, 14);
            dia.audMan.FlushQueue(true);
            dia.audMan.QueueAudio(dia.unlookedVoiceline);
            dia.spriteRenderer[0].sprite = dia.unlooked;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == pm.gameObject)
            {
                Singleton<CoreGameManager>.Instance.AddPoints(-25, pm.playerNumber, true);
                dia.cooldown.Restart();
                dia.spriteRenderer[0].sprite = dia.idle;
                dia.behaviorStateMachine.ChangeState(new DigitalArtist_Wandering(dia));
            }
        }
    }
}

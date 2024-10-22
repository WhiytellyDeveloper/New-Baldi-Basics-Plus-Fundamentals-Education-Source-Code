using UnityEngine;
using MTM101BaldAPI.Components;
using PixelInternalAPI.Extensions;
using System.Linq;
using nbppfe.PrefabSystem;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.Enums;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.BasicClasses.CustomObjects;

namespace nbppfe.CustomContent.NPCs
{
    public class EmillyGutter : NPC, INPCPrefab
    {
        public void Setup()
        {
            Poster.textData[0].size = new IntVector2(Poster.textData[0].size.x + 35, Poster.textData[0].size.z);
            Poster.textData[0].position = new IntVector2(25, 48);
            Poster.textData[1].position = new IntVector2(140, 100);
            Poster.textData[1].size = new IntVector2(104, 122);
            animator = spriteRenderer[0].AddAnimatorToSprite<CustomSpriteAnimator>();
            sprites = NPCLoader.spritesFormSpritesheet[CustomNPCsEnum.EmillyGutter];
            voicelines = NPCLoader.soundsObjects[CustomNPCsEnum.EmillyGutter].Take(4).ToArray();
            sounds = NPCLoader.soundsObjects[CustomNPCsEnum.EmillyGutter].Skip(4).ToArray();
            audMan = GetComponent<AudioManager>();
        }

        public void PostLoading()
        {
            cageCooldown.endAction = ToWandering;
            atackedCooldown.endAction = RemoveInPlayerEfects;

            animator.animations.Add("Idle", new(1, [sprites[0]]));
            animator.animations.Add("Start-Scared", new(2, [sprites[7], sprites[1]]));
            animator.animations.Add("Scared", new(1, [sprites[1]]));
            animator.animations.Add("Pre-Atack", new([sprites[2]], 0.5f));
            animator.animations.Add("Running", new(10, [sprites[3], sprites[4], sprites[5], sprites[4]]));
            animator.animations.Add("Sleeping", new(1, [sprites[6]]));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            atackedCooldown.Pause(true);
            FindCage();
            behaviorStateMachine.ChangeState(new EmillyGutter_InCage(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            atackedCooldown.UpdateCooldown(TimeScale);
        }

        public void ToWandering()
        {
            cage.Open();
            lookingCooldown.Restart();
            behaviorStateMachine.ChangeState(new EmillyGutter_Wandering(this));
        }

        public void FindCage()
        {
            foreach (EmillyCage cage in FindObjectsOfType<EmillyCage>())
            {
                if (cage.owner == null)
                {
                    this.cage = cage;
                    cagePosition = cage.transform.position;
                    cage.SetOwner(this);
                    navigator.Entity.Teleport(cagePosition);
                }
            }
        }

        public void RemoveInPlayerEfects() =>
            pm.plm.am.moveMods.Remove(movMod);

        public void AtackPlayer(Collider other)
        {
            var player = other.GetComponent<PlayerManager>();
            player.plm.am.moveMods.Add(movMod);
            pm = player;
            behaviorStateMachine.ChangeState(new EmillyGutter_AfterAtack(this));
        }

        public Cooldown cageCooldown = new Cooldown(18, 0), lookingCooldown = new Cooldown(5.654f, 0), atackedCooldown = new Cooldown(14, 0);
        public MovementModifier movMod = new MovementModifier(Vector3.zero, 0.45f);
        public CustomSpriteAnimator animator;
        public Sprite[] sprites;
        public SoundObject[] voicelines;
        public SoundObject[] sounds;
        public AudioManager audMan;
        public PlayerManager pm;
        public EmillyCage cage;
        public Vector3 cagePosition;
    }

    public class EmillyGutter_StateBase : NpcState
    {
        public EmillyGutter emi;
        public EmillyGutter_StateBase(EmillyGutter npc) : base(npc)
        {
            this.npc = npc;
            emi = npc;
        }
    }

    public class EmillyGutter_InCage : EmillyGutter_StateBase
    {
        public EmillyGutter_InCage(EmillyGutter npc) : base(npc) { }

        public override void Initialize()
        {
            base.Initialize();
            emi.animator.SetDefaultAnimation("Sleeping", 1);
            ChangeNavigationState(new NavigationState_DoNothing(npc, 0));
            npc.Navigator.Entity.SetFrozen(true);
            emi.cage.Close();
        }

        public override void Update()
        {
            base.Update();
            emi.cageCooldown.UpdateCooldown(emi.TimeScale);
        }
    }

    public class EmillyGutter_Wandering : EmillyGutter_StateBase
    {
        public EmillyGutter_Wandering(EmillyGutter npc) : base(npc) { }

        public override void Initialize()
        {
            base.Initialize();
            npc.Navigator.Entity.SetFrozen(false);
            emi.audMan.QueueAudio(emi.voicelines[1]);
            emi.animator.SetDefaultAnimation("Idle", 1);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
            npc.Navigator.SetSpeed(12, 12);
        }

        public override void InPlayerSight(PlayerManager player)
        {
            base.InPlayerSight(player);
            npc.behaviorStateMachine.ChangeState(new EmillyGutter_Looking(emi, player));
        }
    }

    public class EmillyGutter_Looking : EmillyGutter_StateBase
    {
        protected PlayerManager pm;
        public EmillyGutter_Looking(EmillyGutter npc, PlayerManager player) : base(npc) { pm = player; }

        public override void Initialize()
        {
            base.Initialize();
            emi.animator.Play("Start-Scared", 1);
            emi.animator.SetDefaultAnimation("Scared", 1);
            emi.audMan.FlushQueue(true);
            emi.audMan.QueueAudio(emi.voicelines[2]);
            npc.Navigator.Entity.SetFrozen(true);
        }

        public override void Update()
        {
            base.Update();
            ChangeNavigationState(new NavigationState_TargetPlayer(npc, 0, pm.transform.position));
            emi.lookingCooldown.UpdateCooldown(pm.ec.NpcTimeScale);

            if (emi.lookingCooldown.cooldownIsEnd)
                npc.behaviorStateMachine.ChangeState(new EmillyGutter_InCathingTime(emi, pm));
        }

        public override void Sighted()
        {
            base.Sighted();
            emi.lookingCooldown.Pause(false);
            npc.Navigator.Entity.SetFrozen(true);
        }

        public override void Unsighted()
        {
            base.Unsighted();
            emi.lookingCooldown.Restart();
            emi.lookingCooldown.Pause(true);
            npc.Navigator.SetSpeed(18, 18);
            npc.Navigator.Entity.SetFrozen(false);
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == pm.gameObject)
                emi.AtackPlayer(other);
        }

        public override void Exit()
        {
            base.Exit();
            npc.Navigator.Entity.SetFrozen(false);
        }
    }

    public class EmillyGutter_AfterAtack : EmillyGutter_StateBase
    {
        public EmillyGutter_AfterAtack(EmillyGutter npc) : base(npc) { }

        public override void Initialize()
        {
            base.Initialize();
            emi.atackedCooldown.Restart();
            emi.atackedCooldown.Pause(false);
            emi.cageCooldown.Restart();
            emi.audMan.FlushQueue(true);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(emi.sounds[0]);
            emi.audMan.QueueAudio(emi.voicelines[3], true);
            emi.animator.SetDefaultAnimation("Idle", 1);
            ChangeNavigationState(new NavigationState_TargetPosition(npc, 0, emi.cagePosition));
            npc.Navigator.SetSpeed(14, 14);
        }

        public override void DestinationEmpty()
        {
            base.DestinationEmpty();
            npc.behaviorStateMachine.ChangeState(new EmillyGutter_InCage(emi));
        }
    }

    public class EmillyGutter_InCathingTime : EmillyGutter_StateBase
    {
        protected PlayerManager pm;
        public EmillyGutter_InCathingTime(EmillyGutter npc, PlayerManager player) : base(npc) { pm = player; }

        public override void Initialize()
        {
            base.Initialize();
            emi.audMan.FlushQueue(true);
            emi.audMan.QueueAudio(emi.voicelines[0]);
            emi.animator.Play("Pre-Atack", 1);
            emi.animator.SetDefaultAnimation("Running", 10);
            npc.Navigator.SetSpeed(12, 18);
        }

        public override void Update()
        {
            base.Update();
            ChangeNavigationState(new NavigationState_TargetPlayer(npc, 0, pm.transform.position));
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == pm.gameObject)
                emi.AtackPlayer(other);
        }
    }
}

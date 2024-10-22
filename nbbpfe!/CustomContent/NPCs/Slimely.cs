using MTM101BaldAPI.Components;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.PrefabSystem;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs
{
    public class Slimely : NPC, INPCPrefab
    {
        public void Setup()
        {
            animator = spriteRenderer[0].AddAnimatorToSprite<CustomSpriteAnimator>();
            sprites = NPCLoader.spritesFormSpritesheet[CustomNPCsEnum.Slimely];
        }

        public void PostLoading()
        {
            animator.animations.Add("Idle", new(sprites.Take(1).ToArray(), 0));
            animator.animations.Add("Squishing", new(sprites.Skip(1).Take(5).ToArray(), 0.25f));
            animator.animations.Add("WaterForm", new(sprites.Skip(6).Take(3).ToArray(), 0.87f));
            animator.animations.Add("Unsquishing", new(sprites.Skip(1).Take(5).Reverse().ToArray(), 0.25f));
        }

        public override void Initialize()
        {
            base.Initialize();
            navigator.SetRoomAvoidance(false);
            behaviorStateMachine.ChangeState(new Slimely_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
        }

        public void SquishAnimation(bool reverse)
        {
            animator.Play(reverse ? "Unsquishing" : "Squishing", 1);
            animator.SetDefaultAnimation(reverse ? "Idle" : "WaterForm", 1);
        }

        public CustomSpriteAnimator animator;
        public Sprite[] sprites;
        public MovementModifier movMod = new(Vector3.zero, 0.5f);
    }

    public class Slimely_StateBase : NpcState
    {
        public Slimely sli;
        public Slimely_StateBase(Slimely sli) : base(sli)
        {
            npc = sli;
            this.sli = sli;
        }
    }

    public class Slimely_Wandering : Slimely_StateBase
    {
        bool test;
        public Slimely_Wandering(Slimely sli) : base(sli) { }

        public override void Initialize()
        {
            base.Initialize();
            sli.SquishAnimation(false);
            npc.Navigator.SetSpeed(7, 9);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.CompareTag("Player") || other.CompareTag("NPC") && !test)
            {
                test = true;
                npc.behaviorStateMachine.ChangeState(new Slimely_Squished(sli, other.GetComponent<Entity>()));
            }
        }
    }

    public class Slimely_Squished : Slimely_StateBase
    {
        private Entity target;
        public Slimely_Squished(Slimely sli, Entity target) : base(sli)
        {
            this.target = target;
        }

        public override void Initialize()
        {
            base.Initialize();
            ChangeNavigationState(new NavigationState_DoNothing(npc, 0));
            target.GetComponent<ActivityModifier>().moveMods.Add(sli.movMod);
        }

        public override void Update()
        {
            base.Update();
            npc.Navigator.Entity.Teleport(target.transform.position);
        }
    }
}

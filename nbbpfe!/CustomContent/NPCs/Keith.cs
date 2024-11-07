using MTM101BaldAPI.Components;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.PrefabSystem;
using UnityEngine;
using nbppfe.Enums;
using System.Linq;
using nbppfe.CustomContent.CustomItems;
using PixelInternalAPI.Extensions;
using nbppfe.FundamentalsManager;
using MTM101BaldAPI;
using nbppfe.FundamentalSystems;
using System.Collections;
using System.Collections.Generic;

namespace nbppfe.CustomContent.NPCs
{
    public class Keith : NPC, INPCPrefab
    {
        public void Setup() 
        {
            var sprite = AssetsLoader.Get<Sprite>("crack");
            var endSprite = AssetsLoader.Get<Sprite>("crack2");
            var crackRenderer = ObjectCreationExtensions.CreateSpriteBillboard(sprite, false).AddSpriteHolder(out var renderer, 0.01f);

            crackPrefab = new GameObject("groundCrack").AddComponent<CrakredArea>();
            crackRenderer.transform.SetParent(crackPrefab.transform);
            crackPrefab.renderer = crackRenderer.renderers[0].GetComponent<SpriteRenderer>();
            crackPrefab.endSprite = endSprite;
            crackPrefab.transform.localPosition += new Vector3(0, 0.001f, 0);
            crackPrefab.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            crackPrefab.gameObject.CreatePropagatedAudioManager(50, 122).AddStartingAudiosToAudioManager(false, [NPCLoader.soundsObjects[CustomNPCsEnum.Keith][0]]);
            crackPrefab.gameObject.ConvertToPrefab(true);

            animator = spriteRenderer[0].AddAnimatorToSprite<CustomSpriteAnimator>();
            sprites = NPCLoader.spritesFormSpritesheet[CustomNPCsEnum.Keith];
            audMan = GetComponent<AudioManager>();
            audBang = CustomItemsEnum.PlasticPercussionHammer.ToItem().item.GetComponent<ITM_PercussionHammer>().squishSound;
        }

        public void PostLoading() 
        {
            animator.animations.Add("Idle", new(sprites.Take(1).ToArray(), 0));
            animator.animations.Add("Bang", new(sprites.Skip(1).Take(3).Concat(sprites.Skip(1).Take(3).Reverse()).ToArray(), 0.4f));

            cooldown.endAction = Switch;
        }

        //------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new Keith_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            navigator.Entity.SetFrozen(animator.currentAnimationName == "Bang");
            cooldown.UpdateCooldown(TimeScale);
        }

        public void Bang(Entity entity)
        {
            var hitGround = entity == null;
            animator.Play("Bang", 1);
            animator.SetDefaultAnimation("Idle", 1);
            audMan.PlaySingle(audBang);

            if (hitGround)
            {
                var position = ec.CellFromPosition(transform.position).FloorWorldPosition;
                var floorCrack = Instantiate<CrakredArea>(crackPrefab);
                floorCrack.Initialize(ec, 5, ec.EnvironmentTimeScale);
                floorCrack.transform.position += position;
                floorCrack.transform.rotation = Quaternion.Euler(90f, Random.Range(-180f, 180f), 0f);
            }
            else
                entity.Squish(15);
        }

        public void OnHitCooldownRestarted() =>
            hitCooldown.cooldown = Random.Range(1, 5);
        
        public IEnumerator MultipleHits(Entity entity)
        {
            hitCooldown.Pause(true);
            int hitvalue = Random.Range(1, 4);
            for (int i = 0; i < hitvalue; i++)
            {
                Bang(entity);
                yield return new WaitForSeconds(0.1f);
            }
            hitCooldown.Pause(false);
        }

        public void Switch()
        {
            behaviorStateMachine.ChangeState(angry ? new Keith_Wandering(this) : new Keith_Crazy(this));
            angry = !angry;
            cooldown.Restart();
        }

        public CrakredArea crackPrefab;
        public CustomSpriteAnimator animator;
        public Sprite[] sprites;
        public AudioManager audMan;
        public SoundObject audBang;
        public Cooldown cooldown = new(32, 0), hitCooldown = new(4, 0, null, null, false, true);
        public bool angry;
    }

    public class Keith_StateBase : NpcState {
        protected Keith keith;
        public  Keith_StateBase(Keith keith) : base(keith) { npc = keith; this.keith = keith; }
    }

    public class Keith_Wandering : Keith_StateBase
    {
        public Keith_Wandering(Keith keith) : base(keith) { }

        public override void Initialize()
        {
            base.Initialize();
            npc.Navigator.SetSpeed(10, 15);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }
    }

    public class Keith_Crazy : Keith_StateBase
    {
        public Entity entity;

        public Keith_Crazy(Keith keith) : base(keith) { }

        public override void Initialize()
        {
            base.Initialize();
            keith.hitCooldown.endAction = Bang;
            keith.hitCooldown.restartAction = keith.OnHitCooldownRestarted;
            npc.Navigator.SetSpeed(15, 22);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }

        public override void OnStateTriggerStay(Collider other)
        {
            base.OnStateTriggerStay(other);

            if  (other.CompareTag("Player") || other.CompareTag("NPC"))
                entity = other.GetComponent<Entity>();          
        }

        public override void OnStateTriggerExit(Collider other)
        {
            base.OnStateTriggerStay(other);

            if (entity != null)
            {
                if (other.transform == entity.transform)
                    entity = null;
            }
        }

        public override void Update()
        {
            base.Update();
            keith.hitCooldown.UpdateCooldown(keith.TimeScale);
        }

        public void Bang()
        {
            keith.StartCoroutine(keith.MultipleHits(entity));
            keith.hitCooldown.Restart();
        }

        public override void Exit()
        {
            base.Exit();
            keith.hitCooldown.Restart();
        }

    }
}

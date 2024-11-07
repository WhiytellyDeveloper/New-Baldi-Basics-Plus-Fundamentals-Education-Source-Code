using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.PrefabSystem;
using UnityEngine;
using nbppfe.Enums;
using System.Linq;
using nbppfe.FundamentalSystems;
using nbppfe.FundamentalsManager;

namespace nbppfe.CustomContent.NPCs
{
    public class StellaLog : NPC, INPCPrefab
    {
        public void Setup() {
            audMan = GetComponent<AudioManager>();
            audSlap = Resources.FindObjectsOfTypeAll<SoundObject>().Where(x => x.name == "Slap").First();
            audThrow = FundamentalLoaderManager.GenericThrowSound;
            sprites = NPCLoader.spritesFormSpritesheet[CustomNPCsEnum.StellaLog];
        }

        public void PostLoading() { }

//-------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new StellaLog_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            spriteRenderer[0].sprite = (cathedEntity == null && cooldown.cooldownIsEnd) ? sprites[1] : sprites[0];
        }

        public void ThrowEntity()
        {
            Direction dir = (Direction)Random.Range(0, 3);
            cathedEntity.SetInteractionState(true);
            cathedEntity.SetTrigger(true);
            cathedEntity.AddForce(new(dir.ToVector3(), 30, -20));
            audMan.PlaySingle(audThrow);
            cathedEntity = null;
            behaviorStateMachine.ChangeState(new StellaLog_Wandering(this));
        }

        public AudioManager audMan;
        public SoundObject audSlap, audThrow;

        public Sprite[] sprites;

        public Entity cathedEntity;

        public Cooldown cooldown = new(15, 0);
    }

    public class StellaLog_StateBase : NpcState {
        protected StellaLog stl;
        public StellaLog_StateBase(StellaLog log) : base(log) { npc = log; this.stl = log; }
    }

    public class StellaLog_Wandering : StellaLog_StateBase
    {
        public StellaLog_Wandering(StellaLog log) : base(log) { }

        public override void Initialize()
        {
            base.Initialize();
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
            npc.Navigator.SetSpeed(16, 16);
        }

        public override void Update()
        {
            base.Update();
            stl.cooldown.UpdateCooldown(stl.TimeScale);
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (!stl.cooldown.cooldownIsEnd)
                return;

            if (other.CompareTag("Player") || other.CompareTag("NPC"))
            {
                var entity = other.GetComponent<Entity>();
                stl.cooldown.Restart();
                stl.audMan.PlaySingle(stl.audSlap);
                stl.behaviorStateMachine.ChangeState(new StellaLog_Cathed(stl, entity));
            }
        }
    }

    public class StellaLog_Cathed : StellaLog_StateBase
    {
        protected Entity cathedEntity;
        protected Vector3 target;

        public StellaLog_Cathed(StellaLog log, Entity entity) : base(log) { cathedEntity = entity; }

        public override void Initialize()
        {
            base.Initialize();
            stl.cathedEntity = cathedEntity;

            target = stl.ec.rooms[Random.Range(0, stl.ec.rooms.Count - 1)].RandomEntitySafeCellNoGarbage().FloorWorldPosition;

            cathedEntity.SetInteractionState(false);
            cathedEntity.SetTrigger(false);

            ChangeNavigationState(new NavigationState_TargetPosition(npc, 0, target));
            npc.Navigator.SetSpeed(22, 65);
        }

        public override void Update()
        {
            base.Update();
            cathedEntity.Teleport(stl.transform.position);

            if (target.GetVector2DistanceFrom(stl.transform.position) == 0)
                stl.ThrowEntity();

        }
    }

}


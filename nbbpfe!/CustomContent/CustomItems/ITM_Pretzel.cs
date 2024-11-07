using PixelInternalAPI.Extensions;
using UnityEngine;
using MTM101BaldAPI.Reflection;
using PixelInternalAPI.Classes;
using nbppfe.PrefabSystem;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.Enums;
using MTM101BaldAPI.Components;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Pretzel : Item, IEntityTrigger, IItemPrefab, IClickable<int>
    {
        public void Setup()
        {
            spriteRenderer = ObjectCreationExtensions.CreateSpriteBillboard(CustomItemsEnum.Pretzel.ToItem().itemSpriteLarge);
            spriteRenderer.transform.SetParent(transform);

            entity = gameObject.CreateEntity(1f, 1f, spriteRenderer.transform);
            gameObject.layer = LayerStorage.standardEntities;
            stickySound = AssetsLoader.CreateSound("PretzelCatchSound", Paths.GetPath(PathsEnum.Items, "Pretzel"), "Sfx_CatchPretzel", SoundType.Effect, Color.white, 1);
        }

        public void Clicked(int player)
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericEatSound);
            pm.plm.AddStamina(100, true);
            Destroy(gameObject);
        }
        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return false; }
        public bool ClickableRequiresNormalHeight() { return true; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            cooldown.endAction = RemoveEffect;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericThrowSound);

            entity.OnEntityMoveInitialCollision += (hit) =>
                Destroy(gameObject);

            return true;
        }

        private void Update() =>
            entity.UpdateInternalMovement(transform.forward * 35 * pm.ec.EnvironmentTimeScale);

        public void EntityTriggerEnter(Collider other)
        {
            if (!other.CompareTag("NPC") || active)
            {
                active = true;
                npc = other.GetComponent<NPC>();

                if (npc != null)
                {
                    if (npc.looker.enabled)
                    {
                        if (npc.Character != Character.Principal)
                        {
                            npc.GetComponent<AudioManager>().PlaySingle(stickySound);
                            npc.PlayerLost(pm);

                            npc.GetNPCContainer().AddLookerMod(blindMod);

                            spriteRenderer.enabled = false;

                            if (npc.Character == Character.Playtime && npc.GetComponent<Playtime>().Navigator.maxSpeed == 0)
                                npc.GetComponent<Playtime>().EndJumprope(true);
                        }
                        else
                        {
                            npc.behaviorStateMachine.ChangeState(new Principal_Pretzel(npc.GetComponent<Principal>(), npc.behaviorStateMachine.currentState, pm));
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }


        public void EntityTriggerStay(Collider other)
        {
        }
        public void EntityTriggerExit(Collider other)
        {
        }

        public void RemoveEffect()
        {
            npc.GetNPCContainer().RemoveLookerMod(blindMod);
            Destroy(gameObject);
        }

        public ValueModifier blindMod = new(0f, 0f);
        public SoundObject stickySound;
        public Entity entity;
        public bool active;
        public NPC npc;
        public SpriteRenderer spriteRenderer;
        public Cooldown cooldown = new(20, 0);
    }

    public class Principal_Pretzel : Principal_SubState
    {
        protected PlayerManager pm;
        protected Cooldown cooldown = new(1, 0);

        public Principal_Pretzel(Principal pri, NpcState state, PlayerManager player) : base(pri, state)
        {
            principal = pri;
            npc = pri;
            previousState = state;
            pm = player;
        }

        public override void Initialize()
        {
            base.Initialize();
            pm.ClearGuilt();
            var audMan = (AudioManager)principal.ReflectionGetVariable("audMan");
            audMan.PlaySingle(FundamentalLoaderManager.GenericEatSound);
            principal.Navigator.Entity.SetFrozen(true);
            cooldown.endAction = EndCooldown;
        }

        public override void Update()
        {
            base.Update();
            cooldown.UpdateCooldown(principal.ec.NpcTimeScale * principal.ec.EnvironmentTimeScale);
        }

        public void EndCooldown()
        {
            principal.Navigator.Entity.SetFrozen(false);
            principal.behaviorStateMachine.ChangeState(new Principal_Wandering(principal));
        }
    }
}

using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Mirror : NPCItem, IItemPrefab
    {
        public void Setup() =>
            reflectSound = AssetsLoader.CreateSound("Mirror_Reflect", Paths.GetPath(PathsEnum.Items, "Mirror"), "Sfx_Mirror", SoundType.Effect, Color.white, 1);

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            this.npc = npc;
            cooldown.endAction = OnCooldownEnd;

            if (!npc.Navigator.enabled)
                return false;

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(reflectSound);
            DijkstraMap map = new(pm.ec, PathType.Const, pm.transform);
            map.Activate();
            map.QueueUpdate();
            NavigationState_WanderFleeOverride navigationState_WanderFlee = new(npc, 100, map);
            flee = navigationState_WanderFlee;
            npc.navigationStateMachine.ChangeState(navigationState_WanderFlee);
            FlipNPCSprites(npc.spriteRenderer, true);
            return true;
        }

        private void Update()
        {
            npc.looker.Blink();
            cooldown.UpdateCooldown(npc.ec.EnvironmentTimeScale * npc.TimeScale);
        }

        public void FlipNPCSprites(SpriteRenderer[] sprites, bool flip)
        {
            foreach(SpriteRenderer sprite in sprites)
                sprite.flipX = flip;         
        }

        public void OnCooldownEnd()
        {
            FlipNPCSprites(npc.spriteRenderer, false);
            flee.End();
            Destroy(gameObject);
        }

        protected NavigationState_WanderFleeOverride flee;
        public NPC npc;
        public SoundObject reflectSound;
        public Cooldown cooldown = new Cooldown(25, 0);
    }
}

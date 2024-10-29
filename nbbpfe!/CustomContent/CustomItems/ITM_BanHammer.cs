using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_BanHammer : NPCItem, IItemPrefab
    {
        public void Setup()
        {
            banSound = AssetsLoader.CreateSound("BanHammer_Use", Paths.GetPath(PathsEnum.Items, "BanHammer"), "", SoundType.Effect, Color.white, 1);
            unbanSound = AssetsLoader.CreateSound("BanHammer_End", Paths.GetPath(PathsEnum.Items, "BanHammer"), "", SoundType.Effect, Color.white, 1);
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            base.OnUse(pm, npc);
            this.pm = pm;
            cooldown.endAction = EndCooldown;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(banSound);
            Cell cell = new Cell();
            foreach (Cell _cell in pm.ec.cells)
            {
                if (_cell.Null)
                    cell = _cell;
            }

            lastPosition = npc.transform.position;
            npc.Navigator.Entity.Teleport(cell.FloorWorldPosition);
            npc.Navigator.Entity.SetFrozen(true);
            lastNPC = npc;
            return true;
        }
        
        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        
        public void EndCooldown() 
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(unbanSound);
            lastNPC.Navigator.Entity.Teleport(lastPosition);
            lastNPC.Navigator.Entity.SetFrozen(false);
            Destroy(gameObject);
        }

        protected NPC lastNPC;
        protected Vector3 lastPosition;
        public Cooldown cooldown = new (30, 0);
        public SoundObject banSound, unbanSound;
    }
}

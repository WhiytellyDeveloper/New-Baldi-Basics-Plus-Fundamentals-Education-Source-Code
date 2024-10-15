using nbbpfe.FundamentalsManager;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Shovel : NPCItem, IItemPrefab
    {
        public void Setup() =>
           usedSound = AssetsLoader.CreateSound("ShovelSound", Paths.GetPath(PathsEnum.Items, "Shovel"), "Sfx_Bang", SoundType.Effect, Color.white, 1);
        

//---------------------------------------------------------------------------------------------------

        public override void OnUse(PlayerManager pm, NPC npc)
        {
            base.OnUse(pm, npc);
            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(usedSound);
            npcShoveld = npc;
            force = new Force(-pm.GetPlayerCamera().transform.forward, 75f, 45f);
            npc.Navigator.Entity.IgnoreEntity(pm.plm.Entity, true);
            npc.Navigator.Am.moveMods.Add(movMod);
            npc.Navigator.Entity.AddForce(force);
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        
        private void OnCooldownEnd()
        {
            npcShoveld.Navigator.Entity.RemoveForce(force);
            npcShoveld.Navigator.Entity.IgnoreEntity(pm.plm.Entity, false);
            npcShoveld.Navigator.Am.moveMods.Remove(movMod);
        }


        public MovementModifier movMod = new MovementModifier(Vector3.zero, 0);
        public Force force;
        public Cooldown cooldown = new Cooldown(4, 0);
        public NPC npcShoveld;
        public SoundObject usedSound;
    }
}

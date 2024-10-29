using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_EntityTeleporter : NPCItem, IItemPrefab
    {
        public void Setup() =>
            teleportSound = (SoundObject)Items.Teleporter.ToItem().item.GetComponent<ITM_Teleporter>().ReflectionGetVariable("audTeleport");

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            base.OnUse(pm, npc);
            Vector3 cellPos = pm.ec.RandomCell(true, false, true).CenterWorldPosition;
            npc.Navigator.Entity.Teleport(cellPos);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(teleportSound);
            Destroy(gameObject);
            return true;
        }


        public SoundObject teleportSound;
    }
}

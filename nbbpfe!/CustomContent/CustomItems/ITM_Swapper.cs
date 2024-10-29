using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Swapper : NPCItem, IItemPrefab
    {
        public void Setup()
        {
            teleportingSound = (SoundObject)Items.Teleporter.ToItem().item.GetComponent<ITM_Teleporter>().ReflectionGetVariable("audTeleport");
            trackingSound = FundamentalLoaderManager.GenericTrackerSound;
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            base.OnUse(pm, npc);

            foreach (ITM_Swapper swapper in FindObjectsOfType<ITM_Swapper>())
            {
                if (swapper != this)
                {
                    swapper.npcs[1] = npc;
                    swapper.bugFixVaule = true;
                    Destroy(gameObject);
                    return false;
                }
            }

            npcs[0] = npc;

            if (npcs[0] == null || npcs[1] == null)
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(trackingSound);
            }
            return false;
        }

        private void Update()
        {
            if (npcs[0] != null && npcs[1] != null)
            {
                Vector3 npcPosition1 = npcs[0].transform.position;
                Vector3 npcPosition2 = npcs[1].transform.position;

                npcs[0].Navigator.Entity.Teleport(npcPosition2);
                npcs[1].Navigator.Entity.Teleport(npcPosition1);

                bugFixVaule = true;

                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(teleportingSound);

                pm.itm.RemoveItem(pm.itm.selectedItem);
                Destroy(gameObject);
            }
        }

        public override void OnMissNPC()
        {
            base.OnMissNPC();
            Destroy(gameObject);
        }

        public NPC[] npcs = new NPC[2];
        public bool bugFixVaule;
        public SoundObject teleportingSound, trackingSound;
    }
}

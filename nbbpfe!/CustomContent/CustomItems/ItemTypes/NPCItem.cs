using System;
using UnityEngine;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace nbppfe.CustomContent.CustomItems.ItemTypes
{
    public class NPCItem : Item
    {
        public override bool Use(PlayerManager pm)
        {
            PostUseItem();

            RaycastHit hit;
            LayerMask clickMask = new LayerMask() { value = 131073 };
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach * 3, clickMask))
            {
                NPC hitNPC = hit.transform.GetComponent<NPC>();
                if (hitNPC)
                {
                    if (!elligableNPCs.Contains(hitNPC))
                        elligableNPCs.Add(hitNPC);
                }
            }

            bool guiltSet = false;

            elligableNPCs.Do(x =>
            {
                if (notAllowedCharacters.Contains(x.Character)) return;
                if (!x.GetMeta().flags.HasFlag(NPCFlags.HasTrigger)) return;
                OnUse(pm, x);
                guiltSet = true;

                if (destroyOnUse)
                    Destroy(base.gameObject);
            });

            if (!guiltSet && destroyOnUse)
                Destroy(base.gameObject);

            return guiltSet && returnTrue;
        }

        public virtual void OnUse(PlayerManager pm, NPC npc)
        {

        }

        public void AddNotAllowedCharacter(params Character[] npcs)
        {
            notAllowedCharacters.AddRange(npcs);
        }

        public virtual void PostUseItem()
        {

        }

        public List<Character> notAllowedCharacters = new List<Character>()
        {
            Character.Chalkles
        };

        protected List<NPC> elligableNPCs = new List<NPC>();
        public bool destroyOnUse;
        public bool returnTrue = true;
    }
}

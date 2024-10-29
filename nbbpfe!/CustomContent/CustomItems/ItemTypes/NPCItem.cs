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
            this.pm = pm;
            PreUseItem();
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
                _return = OnUse(pm, x);
                PostUseItem();
                guiltSet = true;

                if (destroyOnUse)
                    Destroy(gameObject);
            });

            if (!guiltSet && destroyOnUse)
                Destroy(gameObject);

            if (!guiltSet)
                OnMissNPC();

            return guiltSet && _return;
        }

        public virtual bool OnUse(PlayerManager pm, NPC npc)
        {

            return false;
        }

        public void AddNotAllowedCharacter(params Character[] npcs) =>
            notAllowedCharacters.AddRange(npcs);
        

        public virtual void PostUseItem()
        {

        }

        public virtual void PreUseItem()
        {

        }

        public virtual void OnMissNPC()
        {

        }

        public List<Character> notAllowedCharacters = [Character.Chalkles];

        protected List<NPC> elligableNPCs = new List<NPC>();
        public bool destroyOnUse;
        private bool _return = false;
    }
}

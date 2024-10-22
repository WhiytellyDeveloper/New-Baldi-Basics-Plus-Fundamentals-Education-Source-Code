using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.Patches;
using nbppfe.PrefabSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_ColoredVisionPad : Item, IItemPrefab
    {
        public void Setup()
        {
            onUse = CustomItemsEnum.NoClipController.ToItem().item.GetComponent<ITM_NoClipController>().beep;
            onEnding = CustomItemsEnum.NoClipController.ToItem().item.GetComponent<ITM_NoClipController>().end;
            onMistake = CustomItemsEnum.NoClipController.ToItem().item.GetComponent<ITM_NoClipController>().error;
        }

        //-----------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            ITM_ColoredVisionPad[] pads = FindObjectsOfType<ITM_ColoredVisionPad>();

            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;

            bool notUse = false;

            if (pads.Length != 1 || pm.ec.Npcs.Count == 0 && itemEnum == CustomItemsEnum.PurpleVisonPad)
                notUse = true;

            if (pads.Length != 1 || pm.ec.items.Count == 0 && itemEnum == CustomItemsEnum.OrangeVisionPad)
                notUse = false;

            if (notUse)
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(onMistake);
                Destroy(gameObject);
                return false;
            }

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(onUse);
            if (itemEnum == CustomItemsEnum.PurpleVisonPad)
            {
                foreach (NPC npc in pm.ec.Npcs)
                {
                    foreach (SpriteRenderer renderer in npc.spriteRenderer)
                    {
                        layers.Add(renderer, renderer.gameObject.layer);
                        renderer.gameObject.layer = LayerMask.NameToLayer("Overlay");
                    }
                }
            }
            if (itemEnum == CustomItemsEnum.OrangeVisionPad)
            {
                foreach (Pickup item in pm.ec.items)
                    item.itemSprite.gameObject.layer = LayerMask.NameToLayer("Overlay");
            }

            return true;
        }

        private void Update() =>
          cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);



        public void OnCooldownEnd()
        {
            if (itemEnum == CustomItemsEnum.PurpleVisonPad)
            {
                foreach (NPC npc in pm.ec.Npcs)
                {
                    foreach (SpriteRenderer renderer in npc.spriteRenderer)
                        renderer.gameObject.layer = layers[renderer];
                }
            }
            if (itemEnum == CustomItemsEnum.OrangeVisionPad)
            {
                foreach (Pickup item in pm.ec.items)
                    item.itemSprite.gameObject.layer = Resources.FindObjectsOfTypeAll<Pickup>().Last().itemSprite.gameObject.layer;
            }
            Destroy(gameObject);
        }

        public Dictionary<SpriteRenderer, int> layers = [];
        public Cooldown cooldown = new Cooldown(25, 0);
        public SoundObject onUse, onEnding, onMistake;
        public CustomItemsEnum itemEnum;
    }
}

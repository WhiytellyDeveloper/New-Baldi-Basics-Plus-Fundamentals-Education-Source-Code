using HarmonyLib;
using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.Patches;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_AdvertenceBook : NPCItem, IItemPrefab
    {
        public void Setup()
        {
            if (!connected)
                writeSound = AssetsLoader.CreateSound("AdvertenceBookWrite", Paths.GetPath(PathsEnum.Items, "AdvertenceBook"), "Sfx_Write", SoundType.Effect, Color.white, 1);
            else
                writeSound = CustomItemsEnum.AdvertenceBook.ToItem().item.GetComponent<ITM_AdvertenceBook>().ReflectionGetVariable("writeSound") as SoundObject;
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            AccessTools.Method(typeof(NPC), "SetGuilt").Invoke(npc, new object[] { 15, "Bullying" });
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(writeSound);

            if (connected)
                Instantiate<Item>(Items.PrincipalWhistle.ToItem().item).Use(pm);

            return true;
            
        }
        public override void PreUseItem()
        {
            base.PostUseItem();
            destroyOnUse = true;
            notAllowedCharacters.Add(Character.Principal);
        }

        public SoundObject writeSound;
        public bool connected;
    }
}

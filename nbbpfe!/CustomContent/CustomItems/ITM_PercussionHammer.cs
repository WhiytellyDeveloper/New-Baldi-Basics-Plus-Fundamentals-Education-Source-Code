using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_PercussionHammer : NPCItem, DietItemVariation, IItemPrefab
    {
        public void Setup()
        {
            squishSound = AssetsLoader.CreateSound("PlayerPercussionHammer", Paths.GetPath(PathsEnum.Items, "PercussionHammer"), "Sfx_Effect_Bang", SoundType.Effect, Color.white, 1);
            time = diet ? 15 : 45;
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            if (npc.Navigator.Entity.Squished)
                return false;

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(squishSound);
            npc.Navigator.Entity.Squish(time);
            Destroy(gameObject);
            return true;
        }

        public SoundObject squishSound;
        public int time;
        public bool diet { get; set; }
    }
}

using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Horn : Item, IItemPrefab
    {
        public void Setup() =>
            hornSound = AssetsLoader.CreateSound("HornSound", Paths.GetPath(PathsEnum.Items, "Horn"), "", SoundType.Effect, Color.white, 1);
        
        public override bool Use(PlayerManager pm)
        {
            pm.ec.MakeNoise(pm.transform.position, 127);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(hornSound);
            return true;
        }

        public SoundObject hornSound;
    }
}

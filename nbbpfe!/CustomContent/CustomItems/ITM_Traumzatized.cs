using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Traumatized : Item, IItemPrefab
    {
        public void Setup()
        {
            var holder = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("NewTraumatized", Paths.GetPath(PathsEnum.Items, "Traumatized"), 14)).AddSpriteHolder(out var renderer, 3.28f, LayerStorage.ignoreRaycast);
            holder.transform.SetParent(transform);
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = EndCooldown;
            //I just want to say thank you to PixelGuy who saved this item. If he wasn't him, he won't be in the mod.
            pm.ec.BlockAllDirs(pm.ec.CellFromPosition(pm.transform.position).position, true);
            transform.position = pm.ec.CellFromPosition(pm.transform.position).FloorWorldPosition;
            return true;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

        public void EndCooldown() 
        {
            pm.ec.BlockAllDirs(pm.ec.CellFromPosition(transform.position).position, false);
            Destroy(gameObject);
        }

        public Cooldown cooldown = new(22, 0);
    }
}

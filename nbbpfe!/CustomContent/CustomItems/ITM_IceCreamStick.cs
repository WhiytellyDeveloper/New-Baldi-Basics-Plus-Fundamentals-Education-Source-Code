using MTM101BaldAPI.Components;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_IceCreamStick : Item, IItemPrefab
    {
        public void Setup()
        {
            freezeStaminatorSprite = AssetsLoader.CreateSprite("StaminometerSheet_OverlayFreeze", Paths.GetPath(PathsEnum.Items, "FreezingIceCream"), 1);
            slurp = AssetsLoader.CreateSound("IceCreamSlurp", Paths.GetPath(PathsEnum.Items, "FreezingIceCream"), "Sfx_Slurp", SoundType.Effect, Color.white, 1);

            var usedItem1 = CustomItemsEnum.StickIceCream.ToItem().Duplicate();
            var usedItem2 = CustomItemsEnum.StickIceCream.ToItem().Duplicate();

            foreach (var (item, nameKey, linkedItem) in new[]
            {
                (usedItem1, "Itm_IceCreamStick1", null),
                (usedItem2, "Itm_IceCreamStick2", usedItem1)})
            {
                item.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                item.nameKey = nameKey;
                item.item = item.item.DuplicatePrefab();
                item.item.GetComponent<ITM_IceCreamStick>().nextItem = linkedItem;
            }

            nextItem = usedItem2;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;

            ITM_IceCreamStick[] iceCreamSticks = FindObjectsOfType<ITM_IceCreamStick>();
            if (iceCreamSticks.Length != 1)
            {
                Destroy(gameObject);
                return false;
            }

            pm.plm.stamina = 50;
            pm.GetMovementStatModifier().AddModifier("staminaDrop", modfire);
            pm.GetMovementStatModifier().AddModifier("staminaRise", modfire);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(slurp);

            HudManager hudMan = Singleton<CoreGameManager>.Instance.GetHud(pm.playerNumber);
            staminatorOverlay = hudMan.transform.Find("Staminometer").Find("Overlay").GetComponent<Image>();
            staminatorSprite = staminatorOverlay.sprite;
            staminatorOverlay.sprite = freezeStaminatorSprite;

            if (nextItem == null)
                return true;

            pm.itm.SetItem(nextItem, pm.itm.selectedItem);
            return false;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

        public void OnCooldownEnd() =>
            Destroy(gameObject);
        
        
        public void OnDestroy()
        {
            pm.GetMovementStatModifier().RemoveModifier(modfire);
            staminatorOverlay.sprite = staminatorSprite;
        }

        public ValueModifier modfire = new (0, 0);
        public Cooldown cooldown = new (18, 0);

        public ItemObject nextItem;

        private Sprite staminatorSprite;
        public Sprite freezeStaminatorSprite;
        private Image staminatorOverlay;

        public SoundObject slurp;
    }
}

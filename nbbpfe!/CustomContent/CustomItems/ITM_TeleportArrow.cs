using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_TeleportArrow : Item, IItemPrefab
    {
        public void Setup()
        {
            sprites[0] = AssetsLoader.CreateSprite("TeleportationArrow_Drop", Paths.GetPath(PathsEnum.Items, "TeleportArrow"), 45);
            sprites[1] = AssetsLoader.CreateSprite("TeleportationArrow_DropMiddle", Paths.GetPath(PathsEnum.Items, "TeleportArrow"), 45);
            sprites[2] = AssetsLoader.CreateSprite("TeleportationArrow_DropDown", Paths.GetPath(PathsEnum.Items, "TeleportArrow"), 45);
            var renderer = ObjectCreationExtensions.CreateSpriteBillboard(sprites[0]);
            renderer.transform.SetParent(transform);

            float spriteSize = sprites[0].GetSpriteSize() / 1.2f;
            entity = gameObject.CreateEntity(spriteSize, spriteSize);

            audThrow = FundamentalLoaderManager.GenericThrowSound;
            audTeleport = (SoundObject)Items.Teleporter.ToItem().item.GetComponent<ITM_Teleporter>().ReflectionGetVariable("audTeleport");

            var previousItem = (ItemObject)null;

            for (int i = 1; i <= 4; i++)
            {
                var currentItem = CustomItemsEnum.TeleportationArrow.ToItem().Duplicate();
                currentItem.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                currentItem.nameKey = $"Itm_TeleportArrow{i}";
                currentItem.item = currentItem.item.DuplicatePrefab();
                if (previousItem != null)
                    currentItem.item.GetComponent<ITM_TeleportArrow>().usedItem = previousItem;
                previousItem = currentItem;
            }

            usedItem = previousItem;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            StartCoroutine(InfiniteAnimation());

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audThrow);

            entity.OnEntityMoveInitialCollision += (hit) =>
            {
                if (CheckForDoor(hit.transform))
                    return;

                pm.Teleport(pm.ec.CellFromPosition(transform.position).CenterWorldPosition);
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audTeleport);
                Destroy(gameObject);
            };

            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }
        protected bool CheckForDoor(Transform t)
        {
            if (t == null)
                return false;


            var door = t.GetComponent<StandardDoor>();
            var value = false;

            if (door)
            {
                door.OpenTimed(door.DefaultTime, false);
                value = true;
            }
            else
            {
                var swingDoor = t.GetComponent<SwingDoor>();
                if (swingDoor)
                {
                    swingDoor.OpenTimed((float)swingDoor.ReflectionGetVariable("defaultTime"), false);
                    value = true;
                }
            }

            return value;
        }


        private IEnumerator InfiniteAnimation()
        {
            var renderer = GetComponentInChildren<SpriteRenderer>();
            int[] sequence = { 0, 1, 2, 1 }; 

            while (true)
            {
                foreach (int index in sequence)
                {
                    renderer.sprite = sprites[index];
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }


        private void Update()
        {
            entity.UpdateInternalMovement(transform.forward * 37 * pm.ec.EnvironmentTimeScale);

            var cell = pm.ec.CellFromPosition(transform.position);
            pm.ec.map.Find(cell.position.x, cell.position.z, cell.ConstBin, cell.room);
        }

        public Entity entity;
        public Sprite[] sprites = [null, null, null];
        public ItemObject usedItem;
        public SoundObject audThrow, audTeleport;
    }
}

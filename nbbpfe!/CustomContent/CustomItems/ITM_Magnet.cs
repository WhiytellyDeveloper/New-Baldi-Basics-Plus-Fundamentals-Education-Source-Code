using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    internal class ITM_Magnet : Item, IItemPrefab
    {
        public void Setup()
        {
            onThrow = FundamentalLoaderManager.GenericThrowSound;

            var spriteRenderer = ObjectCreationExtensions.CreateSpriteBillboard(CustomItemsEnum.Magnet.ToItem().itemSpriteLarge);
            spriteRenderer.transform.SetParent(transform);
            entity = gameObject.CreateEntity(spriteRenderer.GetSpriteSize(), spriteRenderer.GetSpriteSize() * 2, spriteRenderer.transform).SetEntityCollisionLayerMask(LayerStorage.gumCollisionMask);

            var usedItem = CustomItemsEnum.Magnet.ToItem().Duplicate();
            usedItem.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
            usedItem.nameKey = "Itm_Magnet1";
            usedItem.item = usedItem.item.DuplicatePrefab();
            usedItem.item.GetComponent<ITM_Magnet>().usedItem = null;
            this.usedItem = usedItem;
        }

        //-------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            direction = pm.GetPlayerCamera().transform.forward;
            entity = GetComponent<Entity>();
            entity.Initialize(pm.ec, transform.position);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(onThrow);
            entity.OnEntityMoveInitialCollision += (onHit) =>
            {
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = direction;

                bool checker = false;
                foreach (Pickup item in pm.ec.items)
                {
                    if (pm.ec.CellFromPosition(transform.position).room == pm.ec.CellFromPosition(item.transform.position).room && item.GetComponentInChildren<SphereCollider>().enabled)
                    {
                        checker = true;
                        item.Clicked(pm.playerNumber);
                    }
                }

                foreach (Notebook notebook in pm.ec.notebooks)
                {
                    bool cancel = false;
                    if (pm.ec.CellFromPosition(transform.position).room == pm.ec.CellFromPosition(notebook.transform.position).room && notebook.GetComponentInChildren<SphereCollider>().enabled)
                    {
                        foreach (Activity activity in pm.ec.activities)
                        {
                            if (pm.ec.CellFromPosition(transform.position).room == pm.ec.CellFromPosition(activity.transform.position).room)
                                cancel = true;

                        }

                        if (pm.ec.activities.Count == 0 && !cancel)
                        {
                            checker = true;
                            notebook.Clicked(pm.playerNumber);
                        }
                    }
                }

                if (checker)
                {
                    foreach (Cell cell in pm.ec.CellFromPosition(transform.position).room.cells)
                    {
                        if (!cell.Hidden)
                            pm.ec.map.Find(cell.position.x, cell.position.z, cell.ConstBin, cell.room);
                    }
                }

                Physics.Raycast(ray, out var hit, 10f);

                if (CheckForDoor(hit.transform))
                    return;

                Destroy(gameObject);
            };
            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }

        private void Update() =>
            entity.UpdateInternalMovement(direction * 45 * pm.ec.EnvironmentTimeScale);

        protected bool CheckForDoor(Transform t)
        {
            if (t == null)
            {
                Debug.LogError("Transform é null, não há porta para verificar.");
                return false;
            }

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


        public Entity entity;
        public Vector3 direction;
        public ItemObject usedItem;
        public SoundObject onThrow;
    }
}

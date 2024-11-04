using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_SwapperHook : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup()
        {
            var spritePath = Paths.GetPath(PathsEnum.Items, "SwapperHook");
            var sprite = AssetsLoader.CreateSprite("SwapperHook_Drop", spritePath, 45);
            var renderer = ObjectCreationExtensions.CreateSpriteBillboard(sprite);
            var rendererBase = renderer.transform;
            rendererBase.SetParent(transform);
            rendererBase.localPosition = Vector3.zero;
            gameObject.layer = LayerMask.NameToLayer("Default");


            float rendererSize = renderer.GetSpriteSize() / 2f;
            entity = gameObject.CreateEntity(rendererSize, rendererSize, rendererBase);

            var grap = Resources.FindObjectsOfTypeAll<ITM_GrapplingHook>().FirstOrDefault();
            if (grap != null)
            {
                var line = (LineRenderer)grap.ReflectionGetVariable("lineRenderer");
                lineRenderer = line.SafeInstantiate();
                if (lineRenderer != null)
                {
                    lineRenderer.transform.SetParent(transform);
                    lineRenderer.transform.localPosition = Vector3.zero;
                    lineRenderer.gameObject.SetActive(true);
                }
            }

            var usedItem1 = CustomItemsEnum.SwapperHook.ToItem().Duplicate();
            var usedItem2 = CustomItemsEnum.SwapperHook.ToItem().Duplicate();
            var usedItem3 = CustomItemsEnum.SwapperHook.ToItem().Duplicate();

            foreach (var (item, nameKey, linkedItem) in new[]
            {
                (usedItem1, "Itm_SwapperHook1", null),
                (usedItem2, "Itm_SwapperHook2", usedItem1),
                (usedItem3, "Itm_SwapperHook3", usedItem2)})
            {
                item.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                item.nameKey = nameKey;
                item.item = item.item.DuplicatePrefab();
                item.item.GetComponent<ITM_SwapperHook>().usedItem = linkedItem;
                this.usedItem = item;
            }

        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            dir = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);

            var audLaunch = Resources.FindObjectsOfTypeAll<ITM_GrapplingHook>().FirstOrDefault()?.ReflectionGetVariable("audLaunch") as SoundObject;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audLaunch);

            entity.OnEntityMoveInitialCollision += (hit) =>
                Destroy(gameObject);

            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }

        private void Update()
        {
            entity?.UpdateInternalMovement(dir * 65 * pm.ec.EnvironmentTimeScale);
        }

        private void LateUpdate()
        {
            positions[0] = transform.position;
            positions[1] = pm.transform.position - Vector3.up * 1f;
            lineRenderer.SetPositions(positions);
        }

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                var audTeleport = Resources.FindObjectsOfTypeAll<ITM_CommonTeleporter>().FirstOrDefault().teleportSound;
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audTeleport);

                var npc = other.GetComponent<NPC>();
                Vector3 pos = npc.transform.position;
                Vector3 pos2 = pm.transform.position;
                pm.plm.Entity.Teleport(pos);
                npc.Navigator.Entity.Teleport(pos2);
                Destroy(gameObject);
            }
        }

        public void OnDestroy()
        {
            var audSnap = Resources.FindObjectsOfTypeAll<ITM_GrapplingHook>().FirstOrDefault()?.ReflectionGetVariable("audSnap") as SoundObject;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audSnap);
        }

        public void EntityTriggerExit(Collider other) { }

        public void EntityTriggerStay(Collider other) { }
        public LineRenderer lineRenderer;
        public Vector3[] positions = new Vector3[2];
        public Entity entity;
        public Vector3 dir;
        public ItemObject usedItem;
    }
}

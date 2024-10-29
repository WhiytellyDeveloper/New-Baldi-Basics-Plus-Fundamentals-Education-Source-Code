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
    public class ITM_HandHook : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup()
        {
            var spritePath = Paths.GetPath(PathsEnum.Items, "HandHook");
            var sprite = AssetsLoader.CreateSprite("HandHook_Drop", spritePath, 45);
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
                lineRenderer = line?.SafeInstantiate();
                if (lineRenderer != null)
                {
                    lineRenderer.transform.SetParent(transform);
                    lineRenderer.transform.localPosition = Vector3.zero;
                    lineRenderer.gameObject.SetActive(true);
                }
            }

            var usedItem1 = CustomItemsEnum.HandHook.ToItem().Duplicate();
            var usedItem2 = CustomItemsEnum.HandHook.ToItem().Duplicate();

            foreach (var (item, nameKey, linkedItem) in new[]
            {
                (usedItem1, "Itm_HandHook1", null),
                (usedItem2, "Itm_HandHook2", usedItem1)})
            {
                item.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                item.nameKey = nameKey;
                item.item = item.item.DuplicatePrefab();
                item.item.GetComponent<ITM_HandHook>().usedItem = linkedItem;
                this.usedItem = item;
            }


        }

        public override bool Use(PlayerManager pm)
        {
            if (pm == null) return false;

            this.pm = pm;
            transform.position = pm.transform.position;
            direction = pm.GetPlayerCamera().transform.forward;
            entity?.Initialize(pm.ec, transform.position);
            speed = 80;
            var audLaunch = Resources.FindObjectsOfTypeAll<ITM_GrapplingHook>().FirstOrDefault()?.ReflectionGetVariable("audLaunch") as SoundObject;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audLaunch);
            entity.OnEntityMoveInitialCollision += (hit) =>
            {
                if (!cathed && !isReturning)
                {
                    isReturning = true;
                    direction = -pm.GetPlayerCamera().transform.forward;
                    speed = 120;
                }
            };

            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }

        private void Update()
        {
            if (isReturning && pm.transform.position.GetDistanceFrom(transform.position) <= distance)
                Destroy(gameObject);

            if (isReturning && !cathed)
            {
                time += Time.deltaTime * pm.ec.EnvironmentTimeScale;
                if (time > 5f)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            entity?.UpdateInternalMovement(direction * speed * pm.ec.EnvironmentTimeScale);

            if (cathed)     
                entityCathed?.Teleport(transform.position);         
        }

        private void LateUpdate()
        {
            if (lineRenderer == null || positions == null || pm == null) return;

            positions[0] = transform.position;
            positions[1] = pm.transform.position - Vector3.up * 1f;
            lineRenderer.SetPositions(positions);
        }

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isReturning)
                Destroy(gameObject);

            if (other.CompareTag("NPC"))
            {
                var npc = other.GetComponent<NPC>();
                if (npc != null)
                {
                    entityCathed = npc.Navigator.Entity;
                    cathed = true;
                    isReturning = true;
                    direction = -pm.GetPlayerCamera().transform.forward;
                    speed = 170;
                    distance = 12;
                }
            }
        }

        public void EntityTriggerExit(Collider other) { }

        public void EntityTriggerStay(Collider other) { }

        public void OnDestroy()
        {
            var audSnap = Resources.FindObjectsOfTypeAll<ITM_GrapplingHook>().FirstOrDefault()?.ReflectionGetVariable("audSnap") as SoundObject;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audSnap);
        }

        public LineRenderer lineRenderer;
        public Vector3[] positions = new Vector3[2];
        public Entity entity;
        private Vector3 direction;
        public float speed, time, distance = 17;
        public bool cathed, isPickup, isReturning;
        public Entity entityCathed;
        public MovementModifier movMod = new MovementModifier(Vector3.zero, 0f);
        public ItemObject usedItem;
    }
}

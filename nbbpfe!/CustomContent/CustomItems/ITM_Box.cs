using System.Collections;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Box : Item, IItemPrefab, IEntityTrigger, IClickable<int>
    {
        public void Setup()
        {
            closed = AssetsLoader.CreateSprite("Box", Paths.GetPath(PathsEnum.Items, "Box"), 100);
            open = AssetsLoader.CreateSprite("Box_Open", Paths.GetPath(PathsEnum.Items, "Box"), 100);
            renderer = ObjectCreationExtensions.CreateSpriteBillboard(open);
            renderer.transform.SetParent(transform);
            entity = gameObject.CreateEntity(renderer.GetSpriteSize(), renderer.GetSpriteSize(), renderer.transform).SetEntityCollisionLayerMask(LayerStorage.gumCollisionMask);
        }

        //----------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            direction = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            entity.OnEntityMoveInitialCollision += (hit) =>
            {
                if (!isNotDestroyIfHitAWallOrSomethingLikeThat)
                    Destroy(gameObject);
                else
                    HitWall();
            };
            return true;
        }

        private void Update() =>
            entity.UpdateInternalMovement(direction * 45 * pm.ec.EnvironmentTimeScale);

        private IEnumerator ThrowAnim()
        {
            inAnimation = true;
            float fallSpeed = 10f;
            bool falling = true;

            while (falling)
            {
                fallSpeed -= pm.ec.EnvironmentTimeScale * Time.deltaTime * 28f;
                renderer.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;

                if (renderer.transform.localPosition.y <= fallLimit)
                {
                    renderer.transform.localPosition = new Vector3(renderer.transform.localPosition.x, -6, renderer.transform.localPosition.z);
                    falling = false;
                }

                yield return null;
            }

            Vector3 newPos = pm.ec.CellFromPosition(transform.position).FloorWorldPosition;
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

            fallSpeed = 0f;
            falling = true;

            while (falling)
            {
                fallSpeed += pm.ec.EnvironmentTimeScale * Time.deltaTime * 48f;
                renderer.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;

                if (renderer.transform.localPosition.y >= 0)
                {
                    renderer.transform.localPosition = Vector3.up * 0;
                    break;
                }

                yield return null;
            }

            visibleAndClickable = true;
            isNotDestroyIfHitAWallOrSomethingLikeThat = true;
            yield return new WaitForSeconds(10f);
            inAnimation = false;
            HitWall();
        }

        public void Clicked(int player)
        {
            if (visibleAndClickable)
            {
                direction = pm.GetPlayerCamera().transform.forward;
                entity.SetFrozen(false);
            }
        }

        public void HitWall()
        {
            npcCathed.spriteBase.SetActive(true);
            npcCathed.Navigator.Entity.Teleport(transform.position);
            npcCathed.Navigator.Entity.SetFrozen(false);
            Destroy(gameObject);
        }

        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return !visibleAndClickable; }
        public bool ClickableRequiresNormalHeight() { return true; }

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") && !catchNPC)
            {
                var npc = other.GetComponent<NPC>();

                var newCell = new Cell();

                foreach (Cell cell in pm.ec.cells)
                    if (cell.Null)
                        newCell = cell;

                npc.spriteBase.SetActive(false);
                npc.Navigator.Entity.Teleport(newCell.FloorWorldPosition);
                npc.Navigator.Entity.SetFrozen(true);
                npcCathed = npc;
                renderer.sprite = closed;
                entity.SetFrozen(true);
                StartCoroutine(ThrowAnim());
                catchNPC = true;
            }
        }

        public void EntityTriggerStay(Collider other) { }

        public void EntityTriggerExit(Collider other) { }

        public NPC npcCathed;
        public Entity entity;
        public Vector3 direction;
        public bool catchNPC, inAnimation, visibleAndClickable, isNotDestroyIfHitAWallOrSomethingLikeThat;
        public SpriteRenderer renderer;
        public Sprite closed, open;
        public float fallLimit = -6f;
    }
}

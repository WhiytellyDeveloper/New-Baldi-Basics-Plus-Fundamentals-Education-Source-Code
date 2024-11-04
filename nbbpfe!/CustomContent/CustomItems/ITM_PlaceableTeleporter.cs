using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_PlaceableTeleporter : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup() 
        {
            var sprite = AssetsLoader.CreateSprite("placeableteleporterdrop", Paths.GetPath(PathsEnum.Items, "PlaceableTeleporter"), 49);
            var renderer = ObjectCreationExtensions.CreateSpriteBillboard(sprite);
            this.renderer = renderer;
            renderer.transform.SetParent(transform);

            float spriteSize = sprite.GetSpriteSize();
            entity = gameObject.CreateEntity(spriteSize, spriteSize);
            audMan = gameObject.CreatePropagatedAudioManager(50, 75);

            throwSound = FundamentalLoaderManager.GenericThrowSound;
            teleportSound = CustomItemsEnum.CommonTeleporter.ToItem().item.GetComponent<ITM_CommonTeleporter>().teleportSound;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(throwSound);
            transform.position = pm.transform.position;
            renderer.transform.localPosition -= Vector3.up * 4f;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            StartCoroutine(Fall());
            return true;
        }

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") && useable || other.CompareTag("Player") && useable)
            {
                var audMan = other.CompareTag("Player") ? Singleton<CoreGameManager>.Instance.audMan : this.audMan;
                audMan.PlaySingle(teleportSound);
                var npc = other.GetComponent<Entity>();
                var position = pm.ec.RandomCell(true, false, true).CenterWorldPosition;
                npc.Teleport(position);
                renderer.enabled = false;

                StartCoroutine(Destoy(teleportSound.subDuration));
            }
        }

        public void EntityTriggerStay(Collider other) { }
        public void EntityTriggerExit(Collider other) { }

        private IEnumerator Fall()
        {
            float fallSpeed = 20f;
            while (true)
            {
                fallSpeed -= pm.ec.EnvironmentTimeScale * Time.deltaTime * 28f;
                renderer.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;
                if (renderer.transform.localPosition.y <= -4.6)
                {
                    renderer.transform.localPosition = Vector3.up * -4.6f;
                    entity.SetFrozen(true);
                    useable = true;
                    break;
                }
                yield return null;
            }
        }

        private void Update() =>
            entity.UpdateInternalMovement(transform.forward * pm.ec.EnvironmentTimeScale * 18);

        public IEnumerator Destoy(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        public Entity entity;
        public AudioManager audMan;
        public SoundObject teleportSound, throwSound;
        public Renderer renderer;
        public bool useable;
    }
}

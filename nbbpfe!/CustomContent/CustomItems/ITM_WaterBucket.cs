using nbbpfe.Enums;
using nbbpfe.FundamentalsManager;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;
using System.Collections;
using PixelInternalAPI.Classes;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_WaterBucket : Item, IItemPrefab
    {
        public void Setup() {
            var sprite = CustomItemsEnum.WaterBucket.ToItem().itemSpriteLarge;
            floatingSpr = ObjectCreationExtensions.CreateSpriteBillboard(sprite, true).AddSpriteHolder(0, LayerStorage.billboardLayer);
            floatingSpr.flipY = true;
            floatingSpr.transform.SetParent(transform);

            var waterGrounded = gameObject.AddComponent<SlipArea>();
            waterGrounded.audMan = waterGrounded.gameObject.CreatePropagatedAudioManager(22, 75);
           // glueGrounded.onEnterSound = AssetsLoader.CreateSound("StickyGlue", Paths.GetPath(PathsEnum.Items, "Glue"), "Sfx_StickyGlue", SoundType.Effect, Color.white, 1);
            groundedSpr = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("water", Paths.GetPath(PathsEnum.Items, "WaterBucket"), 15), false).AddSpriteHolder(-5 + 0.01f);
            groundedSpr.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            groundedSpr.transform.SetParent(waterGrounded.transform);
            entity = gameObject.CreateEntity(1.5f, 1.5f);
            gameObject.layer = LayerStorage.standardEntities;
        }

        //-------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            GetComponent<SlipArea>().enabled = false;
            groundedSpr.enabled = false;
            StartCoroutine(Fall());
            return true;
        }

        private IEnumerator Fall()
        {
            moving = true;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericThrowSound);
            float fallSpeed = 10f;
            while (true)
            {
                fallSpeed -= pm.ec.EnvironmentTimeScale * Time.deltaTime * 28f;
                floatingSpr.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;
                if (floatingSpr.transform.localPosition.y <= fallLimit)
                {
                    floatingSpr.transform.localPosition = Vector3.up * fallLimit;
                    break;
                }
                yield return null;
            }
            moving = false;
            entity.Teleport(pm.ec.CellFromPosition(transform.position).FloorWorldPosition);
            GetComponent<SlipArea>().enabled = true;
            GetComponent<SlipArea>().force = new Force(transform.forward, 10f, 25f, 5f);
            groundedSpr.enabled = true;
            floatingSpr.enabled = false;
            yield break;
        }

        private void Update()
        {
            if (moving)
                entity.UpdateInternalMovement(transform.forward * 15 * pm.ec.EnvironmentTimeScale);
            else
                entity.SetFrozen(true);
        }


        public SpriteRenderer groundedSpr;
        public SpriteRenderer floatingSpr;
        public Entity entity;
        public bool moving;
        public float fallLimit = -4f;
    }
}

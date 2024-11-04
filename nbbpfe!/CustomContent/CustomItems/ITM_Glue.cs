using PixelInternalAPI.Extensions;
using PixelInternalAPI.Classes;
using UnityEngine;
using System.Collections;
using nbppfe.PrefabSystem;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.Enums;
using nbppfe.CustomContent.CustomItems.ItemTypes;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Glue : Item, IItemPrefab, DietItemVariation
    {
        public void Setup()
        {
            var sprite = diet ? CustomItemsEnum.StickGlue.ToItem().itemSpriteLarge : CustomItemsEnum.StckyGlue.ToItem().itemSpriteLarge;
            var holder = ObjectCreationExtensions.CreateSpriteBillboard(sprite, true).AddSpriteHolder(out var renderer, 0, LayerStorage.billboardLayer);
            floatingSpr = holder.renderers[0].GetComponent<SpriteRenderer>();
            floatingSpr.flipY = true;
            holder.transform.SetParent(transform);

            var glueGrounded = gameObject.AddComponent<StickyArea>();
            glueGrounded.audMan = glueGrounded.gameObject.CreatePropagatedAudioManager(22, 75);

            if (!diet)
                glueGrounded.onEnterSound = AssetsLoader.CreateSound("StickyGlue", Paths.GetPath(PathsEnum.Items, "Glue"), "Sfx_StickyGlue", SoundType.Effect, Color.white, 1);
            else
                glueGrounded.onEnterSound = AssetsLoader.Get<SoundObject>("StickyGlue");

            var holder2 = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite(diet ? "StickGlue" : "Glue", Paths.GetPath(PathsEnum.Items, diet ? "GlueStick" : "Glue"), 30), false).AddSpriteHolder(out var renderer2, -5 + 0.01f);
            groundedSpr = holder2.renderers[0].GetComponent<SpriteRenderer>();
            groundedSpr.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            holder2.transform.SetParent(transform);
            entity = gameObject.CreateEntity(1.5f, 1.5f);
            gameObject.layer = LayerStorage.standardEntities;
            modfire = diet ? 0.2f : 0.04f;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            GetComponent<StickyArea>().moveMod = new (Vector3.zero, modfire);
            GetComponent<StickyArea>().enabled = false; //lol
            groundedSpr.enabled = false;
            StartCoroutine(Fall());
            return true;
        }

        private void Update()
        {
            if (moving)
                entity.UpdateInternalMovement(transform.forward * 15 * pm.ec.EnvironmentTimeScale);
            else
                entity.SetFrozen(true);
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
            GetComponent<StickyArea>().enabled = true;
            GetComponent<StickyArea>().audMan.PlaySingle(GetComponent<StickyArea>().onEnterSound);
            groundedSpr.enabled = true;
            floatingSpr.enabled = false;
            yield break;
        }

        public SpriteRenderer groundedSpr;
        public SpriteRenderer floatingSpr;
        public Entity entity;
        public bool moving;
        public float fallLimit = -4f, modfire = 0.08f;
        public bool diet { get; set; }
    }
}

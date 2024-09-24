using nbbpfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using PixelInternalAPI.Classes;
using UnityEngine;
using nbppfe.FundamentalSystems;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;
using nbbpfe.Enums;
using nbppfe.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace nbppfe.CustomItems
{
    public class ITM_Glue : Item, IItemPrefab
    {
        public void Setup()
        {
            var sprite = CustomItemsEnum.Glue.ToItem().itemSpriteLarge;
            floatingSpr = ObjectCreationExtensions.CreateSpriteBillboard(sprite, true).AddSpriteHolder(0, LayerStorage.billboardLayer);
            floatingSpr.flipY = true;
            floatingSpr.transform.SetParent(transform);

            var glueGrounded = gameObject.AddComponent<StickyArea>();
            glueGrounded.audMan = glueGrounded.gameObject.CreatePropagatedAudioManager(22, 75);
            glueGrounded.onEnterSound = AssetsLoader.CreateSound("StickyGlue", Paths.GetPath(PathsEnum.Items, "Glue"), "Sfx_StickyGlue", SoundType.Effect, Color.white, 1);
            groundedSpr = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("Glue", Paths.GetPath(PathsEnum.Items, "Glue"), 30), false).AddSpriteHolder(-5 + 0.01f);
            groundedSpr.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            groundedSpr.transform.SetParent(glueGrounded.transform);
            entity = gameObject.CreateEntity(1.5f, 1.5f);
            gameObject.layer = LayerStorage.standardEntities;

        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.transform.position;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            GetComponent<StickyArea>().moveMod = new MovementModifier(Vector3.zero, 0.15f);
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
        public float fallLimit = -4f;
    }
}

using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using nbbpfe.FundamentalsManager;
using PixelInternalAPI.Classes;
using UnityEngine;
using System.Collections;
using nbppfe.FundamentalSystems;
using nbppfe.Extensions;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Soda : Item, IItemPrefab
    {
        public void Setup()
        {
            spr = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("SodaSprite", Paths.GetPath(PathsEnum.Items, "Soda"), 12)).AddSpriteHolder(0, LayerStorage.ignoreRaycast);
            spr.transform.SetParent(transform);

            entity = gameObject.CreateEntity(0.1f, 0.1f, spr.transform).SetEntityCollisionLayerMask(0);
            gameObject.layer = LayerStorage.standardEntities;
            splashSound = AssetsLoader.CreateSound("Soda_open", Paths.GetPath(PathsEnum.Items, "Soda"), "", SoundType.Effect, Color.white, 1);
            trashSound = AssetsLoader.CreateSound("Soda_end", Paths.GetPath(PathsEnum.Items, "Soda"), "", SoundType.Effect, Color.white, 1);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = End;
            transform.position = pm.transform.position;
            entity.Initialize(pm.ec, transform.position);
            pm.Am.moveMods.Add(speedModifier);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(splashSound);
            StartCoroutine(Fade(0.5f, 4f));
            return true;
        }

        private void Update()
        {
            cooldown.UpdateCooldown(Singleton<BaseGameManager>.Instance.Ec.EnvironmentTimeScale);
            entity.UpdateInternalMovement(pm.GetPlayerCamera().transform.forward * 30 * pm.ec.EnvironmentTimeScale);
        }

        private IEnumerator Fade(float speed, float duration)
        {
            float elapsedTime = 0f;
            Color color = spr.color;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(Color.white.a, 0, elapsedTime * speed / duration);
                spr.color = color;
                yield return null;
            }
        }

        public void End()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(trashSound);
            pm.Am.moveMods.Remove(speedModifier);
        }

        public Entity entity;
        public SpriteRenderer spr;
        public SoundObject splashSound;
        public SoundObject trashSound;
        public MovementModifier speedModifier = new MovementModifier(Vector3.zero, 2.1f);
        public Cooldown cooldown = new Cooldown(25, 0);
    }
}

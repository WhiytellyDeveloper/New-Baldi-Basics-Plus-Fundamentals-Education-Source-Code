using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_GenericSoda : Item, DietItemVariation, IItemPrefab, IEntityTrigger     
    {
        public void Setup() 
        {
            var spriteNormal = AssetsLoader.CreateSprite("GenerixSoda_Spray", Paths.GetPath(PathsEnum.Items, "GenericSoda"), 25);
            var dietSpriteNormal = AssetsLoader.CreateSprite("DietGenerixSoda_Spray", Paths.GetPath(PathsEnum.Items, "DietGenericSoda"), 25);

            var renderer = ObjectCreationExtensions.CreateSpriteBillboard(diet ? dietSpriteNormal : spriteNormal).AddSpriteHolder(out var _renderer, 0, LayerStorage.ignoreRaycast);
            renderer.gameObject.layer = LayerStorage.billboardLayer;
            renderer.transform.SetParent(transform);
            entity = gameObject.CreateEntity(1, 1, renderer.transform).SetEntityCollisionLayerMask(0);

            speed = diet ? 28 : 47;

            spraySound = (SoundObject)Items.Bsoda.ToItem().item.ReflectionGetVariable("sound");
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown = new(diet ? 5 : 60, 0f);
            cooldown.endAction = Destroy;
            transform.position = pm.transform.position - Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward;
            transform.forward = pm.GetPlayerCamera().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(spraySound);
            pm.RuleBreak("Drinking", 0.8f, 0.1f);
            return true;
        }

        private void Update()
        {
            moveMod.movementAddend = entity.ExternalActivity.Addend + transform.forward * speed * pm.ec.EnvironmentTimeScale;
            entity.UpdateInternalMovement(transform.forward * speed * pm.ec.EnvironmentTimeScale);
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        }

        public void EntityTriggerEnter(Collider other)
        {
            Entity component = other.GetComponent<Entity>();
            if (other.CompareTag("Player") && component != null)
            {
                component.ExternalActivity.moveMods.Add(this.moveMod);
                activityMods.Add(component.ExternalActivity);
            }
        }

        public void EntityTriggerStay(Collider other) { }

        public void EntityTriggerExit(Collider other)
        {
            Entity component = other.GetComponent<Entity>();
            if (component != null && other.CompareTag("Player"))
            {
                component.ExternalActivity.moveMods.Remove(moveMod);
                activityMods.Remove(component.ExternalActivity);
            }
        }

        public void Destroy()
        {
            foreach (ActivityModifier activityModifier in activityMods)
                activityModifier.moveMods.Remove(moveMod);
            Destroy(base.gameObject);
        }

        private MovementModifier moveMod = new(Vector3.zero, 1);
        public Entity entity;
        private List<ActivityModifier> activityMods = new();
        public SoundObject spraySound;
        public float speed = 47, time;
        public Cooldown cooldown;

        public bool diet { get; set; }
    }
}

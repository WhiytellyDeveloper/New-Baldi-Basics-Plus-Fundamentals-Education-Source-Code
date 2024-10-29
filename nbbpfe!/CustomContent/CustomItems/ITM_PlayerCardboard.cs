﻿using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_PlayerCardboard : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup()
        {
            var sprite = AssetsLoader.CreateSprite("PlayerCardboard", Paths.GetPath(PathsEnum.Items, "PlayerCardboard"), 19);
            var holder = ObjectCreationExtensions.CreateSpriteBillboard(sprite).AddSpriteHolder(out var renderer, -1.64f, LayerStorage.ignoreRaycast);
            holder.transform.SetParent(transform);

            float spriteSize = sprite.GetSpriteSize() / 2;
            entity = gameObject.CreateEntity(spriteSize, spriteSize);
            gameObject.layer = LayerStorage.ignoreRaycast;

            audMan = gameObject.CreatePropagatedAudioManager(40, 100);
            cuttingSound = AssetsLoader.CreateSound("CuttingCardboard", Paths.GetPath(PathsEnum.Items, "PlayerCardboard"), "Sfx_CuttingCardboard", SoundType.Effect, Color.white, 1);
            placeCardboardSound = AssetsLoader.CreateSound("CardboardPlacing", Paths.GetPath(PathsEnum.Items, "PlayerCardboard"), "", SoundType.Effect, Color.white, 1);
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(placeCardboardSound);
            cooldown.endAction = OnEndCooldown;
            transform.position = pm.ec.CellFromPosition(pm.transform.position).FloorWorldPosition;
            entity.Initialize(pm.ec, transform.position);
            return true;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC"))
            {
                var npc = other.GetComponent<NPC>();
                entity.SetFrozen(true);
                npc.Navigator.Entity.Teleport(transform.position);
                cooldown.Pause(false);
                audMan.SetLoop(true);
                audMan.PlaySingle(cuttingSound);
                StartCoroutine(StartShake(npc));
            }
        }

        public void EntityTriggerStay(Collider other) { }
        public void EntityTriggerExit(Collider other) { }

        public IEnumerator StartShake(NPC baldi)
        {
            while (true)
            {
                Vector3 originalPos = baldi.transform.localPosition;
                while (true)
                {
                    float shakeAmount = 1.5f;
                    baldi.Navigator.Entity.Teleport(originalPos + new Vector3(
                        Random.Range(-shakeAmount, shakeAmount),
                        originalPos.y,
                        Random.Range(-shakeAmount, shakeAmount)
                    ));

                    yield return null;
                }
            }
        }

        public void OnEndCooldown() =>        
            Destroy(gameObject);

        public Entity entity;
        public Cooldown cooldown = new Cooldown(4, 0, null, null, true);
        public AudioManager audMan;
        public SoundObject cuttingSound, placeCardboardSound;
    }
}

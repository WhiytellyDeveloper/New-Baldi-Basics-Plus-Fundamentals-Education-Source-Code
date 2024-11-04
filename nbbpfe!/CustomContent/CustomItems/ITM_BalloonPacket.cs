using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_BalloonPacket : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup()
        {
            randomSprites = TextureExtensions.LoadSpriteSheet(4, 1, 18, Paths.GetPath(PathsEnum.Items, ["BalloonPacket", "BallonSpritesheet.png"]));

            var holder = ObjectCreationExtensions.CreateSpriteBillboard(randomSprites[0]);
            holder.gameObject.AddComponent<PickupBob>();
            holder.transform.SetParent(transform);

            float spriteSize = randomSprites[0].GetSpriteSize() * 1.4f;
            var balloon = gameObject.AddComponent<Balloon>();
            var entity = gameObject.CreateEntity(spriteSize, spriteSize);
            balloon.ReflectionSetVariable("entity", entity);

            audMan = gameObject.CreatePropagatedAudioManager(75, 175);
            audBallonPop = AssetsLoader.CreateSound("AlexBallonPop", Paths.GetPath(PathsEnum.Items, "BalloonPacket"), "Sfx_Effects_Pop", SoundType.Effect, Color.white, 1);
            audBallonInflation = AssetsLoader.CreateSound("BallonInflation", Paths.GetPath(PathsEnum.Items, "BalloonPacket"), "Sfx_Inflation", SoundType.Effect, Color.white, 1);
            var previousItem = (ItemObject)null;

            for (int i = 1; i <= 5; i++)
            {
                var currentItem = CustomItemsEnum.BallonPacket.ToItem().Duplicate();
                currentItem.AddMeta(BasePlugin.Instance, ItemFlags.MultipleUse);
                currentItem.nameKey = $"Itm_BallonPacket{i}";
                currentItem.item = currentItem.item.DuplicatePrefab();
                if (previousItem != null)
                    currentItem.item.GetComponent<ITM_BalloonPacket>().usedItem = previousItem;
                previousItem = currentItem;
            }

            usedItem = previousItem;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            audMan.PlaySingle(audBallonInflation);
            GetComponent<Balloon>().Initialize(pm.plm.Entity.CurrentRoom);
            GetComponentInChildren<SpriteRenderer>().sprite = randomSprites.CatchRandomItem<Sprite>();
            transform.position = pm.transform.position;

            if (usedItem == null)
                return true;

            pm.itm.SetItem(usedItem, pm.itm.selectedItem);
            return false;
        }


        public void EntityTriggerEnter(Collider other)
        {
            if (playerEnter)
                return;

                Pop(other.GetComponent<Entity>());
        }

        public void EntityTriggerStay(Collider other) { }
        public void EntityTriggerExit(Collider other) 
        {
            if (other.CompareTag("Player"))
                playerEnter = false;
        }

        public void Pop(Entity entity)
        {
            Singleton<BaseGameManager>.Instance.Ec.MakeNoise(pm.transform.position, 120);
            entity.AddForce(new Force(-entity.transform.forward, 35, -45));
            audMan.PlaySingle(audBallonPop);
            GetComponent<Entity>().Enable(false);
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartCoroutine(Destoy(audBallonPop.subDuration));
        }

        public IEnumerator Destoy(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        public Sprite[] randomSprites = [];
        public bool playerEnter = true;
        public AudioManager audMan;
        public SoundObject audBallonPop, audBallonInflation;
        public ItemObject usedItem;
    }
}

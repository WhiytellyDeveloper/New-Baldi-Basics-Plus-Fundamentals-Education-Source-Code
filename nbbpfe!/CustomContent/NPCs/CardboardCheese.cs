using nbbpfe.FundamentalsManager;
using nbppfe.CustomContent.NPCs.FunctionalsManagers;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs
{
    public class CardboardCheese : NPC, INPCPrefab
    {
        public void Setup() {
            Sprite misc = AssetsLoader.Get<Sprite>("CartboardCheeseV2_Misc");
            SpriteRenderer renderer = ObjectCreationExtensions.CreateSpriteBillboard(misc, false);
            renderer.transform.localPosition = new Vector3(0.5f, 0, 2);
            renderer.transform.SetParent(transform);
            audMan = GetComponent<AudioManager>();
            helloVoicelines = [AssetsLoader.Get<SoundObject>("CardboardCheese_Hello1"), AssetsLoader.Get<SoundObject>("CardboardCheese_Hello2")];
            noItemsVoicelines = [AssetsLoader.Get<SoundObject>("CardboardCheese_NoItems1"), AssetsLoader.Get<SoundObject>("CardboardCheese_NoItems2"), AssetsLoader.Get<SoundObject>("CardboardCheese_NoItems3")];
            cheesedItemsVoicelines = [AssetsLoader.Get<SoundObject>("CardboardCheese_CheesedItems1"), AssetsLoader.Get<SoundObject>("CardboardCheese_CheesedItems2"), AssetsLoader.Get<SoundObject>("CardboardCheese_CheesedItems3")];
        }

        public void PostLoading() {
            cooldown = new Cooldown(10, 0, Active);
        }

//----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();

            if (Singleton<CardboardCheeseFunctionalManager>.Instance == null)
                Singleton<CoreGameManager>.Instance.gameObject.AddComponent<CardboardCheeseFunctionalManager>();

            transform.rotation = ec.CellFromPosition(transform.position).AllOpenNavDirections[0].ToRotation();

            behaviorStateMachine.ChangeState(new CardboardCheese_Disable(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            if (!active) 
                cooldown.UpdateCooldown(ec.NpcTimeScale);
        }

        public override void Despawn()
        {
            base.Despawn();

            for (int i = 0; i < Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem; i++)
                Singleton<CardboardCheeseFunctionalManager>.Instance.CheesedSlot(i, 0, true);
        }

        public void TryCheesedSlot(PlayerManager playerManager, ItemManager itemManager)
        {
            int selectedSlot;
            bool slotFound = false;
            int maxAttempts = itemManager.maxItem;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                selectedSlot = Random.Range(0, itemManager.maxItem + 1);
                if (!itemManager.IsSlotLocked(selectedSlot))
                {
                    Singleton<CardboardCheeseFunctionalManager>.Instance.CheesedSlot(selectedSlot, playerManager.playerNumber, false);
                    slotFound = true;
                    break;
                }
            }

            if (!slotFound)
            {
                playerManager.plm.Entity.AddForce(new Force((playerManager.plm.Entity.transform.position - transform.position).normalized, 62, -62));
                audMan.FlushQueue(true);
                audMan.QueueRandomAudio(noItemsVoicelines);
            }
            else
            {
                audMan.FlushQueue(true);
                audMan.QueueRandomAudio(cheesedItemsVoicelines);
            }
        }

        public void Active() =>
            behaviorStateMachine.ChangeState(new CardboardCheese_Active(this));


        public void Spawn()
        {
            tiles.Clear();
            for (int i = 0; i < ec.levelSize.x; i++)
            {
                for (int j = 0; j < ec.levelSize.z; j++)
                {
                    if (!ec.cells[i, j].Null &&ec.cells[i, j].room.type == RoomType.Hall && !ec.cells[i, j].open && !ec.cells[i, j].HasAnyHardCoverage)
                        tiles.Add(ec.cells[i, j]);         
                }
            }
            spawn =tiles[Random.Range(0, tiles.Count)];
            bool flag = false;
            while (!flag)
            {
                spawn = tiles[Random.Range(0, tiles.Count)];
                flag = true;
                int num = 0;
                while (num < Singleton<CoreGameManager>.Instance.setPlayers && flag)
                {
                    if ((spawn.FloorWorldPosition + Vector3.up * 5f - Singleton<CoreGameManager>.Instance.GetPlayer(num).transform.position).magnitude <= playerBuffer || ec.TrapCheck(this.spawn))
                        flag = false;
                    
                    num++;
                }
            }
            transform.position = spawn.FloorWorldPosition + Vector3.up * 5f;
        }



        public AudioManager audMan;
        public SoundObject[] helloVoicelines, wanderingVoicelines, noItemsVoicelines, cheesedItemsVoicelines;
        public Cooldown cooldown;
        public bool active;
        public List<Cell> tiles;
        private Cell spawn;
        private float playerBuffer = 20f;
    }

    public class CardboardCheese_BaseState : NpcState
    {
        protected CardboardCheese cardCheese;
        public CardboardCheese_BaseState(CardboardCheese cardCheese) : base(cardCheese)
        {
            npc = cardCheese;
            this.cardCheese = cardCheese;
        }
    }

    public class CardboardCheese_Active : CardboardCheese_BaseState
    {
        public CardboardCheese_Active(CardboardCheese cardCheese) : base(cardCheese) { }

        public override void Initialize()
        {
            base.Initialize();
            ChangeNavigationState(new NavigationState_Disabled(cardCheese));
            cardCheese.Navigator.Entity.SetFrozen(true);
            cardCheese.Navigator.Entity.SetActive(true);
            cardCheese.spriteBase.gameObject.SetActive(true);
            cardCheese.active = true;
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.tag.Equals("Player"))
            {
                cardCheese.TryCheesedSlot(other.GetComponent<PlayerManager>(), other.GetComponent<PlayerManager>().itm);
                cardCheese.behaviorStateMachine.ChangeState(new CardboardCheese_Disable(cardCheese));
            }
        }

        public override void PlayerSighted(PlayerManager player)
        {
            base.PlayerInSight(player);
            cardCheese.audMan.FlushQueue(true);
            cardCheese.audMan.QueueRandomAudio(cardCheese.helloVoicelines);
        }
    }

    public class CardboardCheese_Disable : CardboardCheese_BaseState
    {
        public CardboardCheese_Disable(CardboardCheese cardCheese) : base(cardCheese) { }

        public override void Initialize()
        {
            base.Initialize();
            cardCheese.Spawn();
            cardCheese.spriteRenderer[0].transform.position = new Vector3(0, -10, 0);
            cardCheese.Navigator.Entity.SetActive(false);
            cardCheese.active = false;
        }
    }
}


using nbppfe.CustomContent.NPCs.FunctionalsManagers;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using UnityEngine;
using nbppfe.Enums;
using System.Linq;
using MTM101BaldAPI;

namespace nbppfe.CustomContent.NPCs
{
    public class CardboardCheese : NPC, INPCPrefab
    {
        public void Setup()
        {
            Sprite misc = AssetsLoader.Get<Sprite>("CartboardCheeseV2_Misc");
            SpriteRenderer renderer = ObjectCreationExtensions.CreateSpriteBillboard(misc);
            renderer.transform.localPosition = new Vector3(1.5f, -3.55f, 1.2f);
            renderer.transform.SetParent(spriteBase.transform);
            audMan = GetComponent<AudioManager>();
            helloVoicelines = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(3).Take(3).ToArray();
            noItemsVoicelines = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(6).Take(3).ToArray();
            cheesedItemsVoicelines = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Take(3).ToArray();
            wanderingVoicelines = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(9).Take(5).ToArray();

            NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese][13].additionalKeys = [
                new SubtitleTimedKey
                {
                    key = "Looking Gouda!",
                    time = 2.940f
                },
                new SubtitleTimedKey
                {
                    key = "That joke was terrible...",
                    time = 5.198f
                }];

            Debug.Log(NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese][13].name);
        }

        public void PostLoading()
        {
            cooldown = new(13, 0, Active);
            wvCooldown = new(18, 0, SayRandomVoiceline);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            Singleton<CoreGameManager>.Instance.gameObject.GetOrAddComponent<CardboardCheeseFunctionalManager>();
            behaviorStateMachine.ChangeState(new CardboardCheese_Disable(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();
            if (!active)
                cooldown.UpdateCooldown(ec.NpcTimeScale);
            wvCooldown.UpdateCooldown(ec.NpcTimeScale);
        }

        public override void Despawn()
        {
            base.Despawn();

            for (int i = 0; i < Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.maxItem; i++)
                Singleton<CardboardCheeseFunctionalManager>.Instance.CheesedSlot(i, 0, true);
        }

        public bool TryCheesedSlot(PlayerManager playerManager)
        {
            int selectedSlot;
            bool slotFound = false;
            int maxAttempts = playerManager.itm.maxItem * 5;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                selectedSlot = Random.Range(0, playerManager.itm.maxItem + 1);
                if (!playerManager.itm.IsSlotLocked(selectedSlot))
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

            return slotFound;
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
                    if (!ec.cells[i, j].Null && ec.cells[i, j].room.type == RoomType.Hall && !ec.cells[i, j].open && !ec.cells[i, j].HasAnyHardCoverage && ec.cells[i, j].shape == TileShape.Straight)
                        tiles.Add(ec.cells[i, j]);
                }
            }
            spawn = tiles[Random.Range(0, tiles.Count)];
            bool flag = false;
            while (!flag)
            {
                spawn = tiles[Random.Range(0, tiles.Count)];
                flag = true;
                int num = 0;
                while (num < Singleton<CoreGameManager>.Instance.setPlayers && flag)
                {
                    if ((spawn.FloorWorldPosition + Vector3.up * 5f - Singleton<CoreGameManager>.Instance.GetPlayer(num).transform.position).magnitude <= playerBuffer || ec.TrapCheck(spawn))
                        flag = false;

                    num++;
                }
            }
            transform.position = spawn.FloorWorldPosition + Vector3.up * 5f;
        }

        public void SayRandomVoiceline()
        {
            if (!playerLooking && active)
            {
                audMan.FlushQueue(true);
                audMan.QueueRandomAudio(wanderingVoicelines);
            }
            wvCooldown.Restart();
        }

        public AudioManager audMan;
        public SoundObject[] helloVoicelines, wanderingVoicelines, noItemsVoicelines, cheesedItemsVoicelines;
        public Cooldown cooldown, wvCooldown;
        public bool active, playerLooking;
        public List<Cell> tiles = [];
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
            cardCheese.Spawn();
            cardCheese.active = true;
            cardCheese.spriteBase.gameObject.SetActive(true);
            cardCheese.Navigator.Entity.SetFrozen(true);
            cardCheese.Navigator.Entity.SetIgnoreAddend(true);
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.tag == "Player")
            {
                if (cardCheese.TryCheesedSlot(other.GetComponent<PlayerManager>()))
                    cardCheese.behaviorStateMachine.ChangeState(new CardboardCheese_Disable(cardCheese));
            }
        }

        public override void PlayerSighted(PlayerManager player)
        {
            base.PlayerInSight(player);
            cardCheese.audMan.FlushQueue(true);
            cardCheese.audMan.QueueRandomAudio(cardCheese.helloVoicelines);
        }

        public override void PlayerInSight(PlayerManager player)
        {
            base.PlayerInSight(player);
            cardCheese.playerLooking = true;
        }

        public override void PlayerLost(PlayerManager player)
        {
            base.PlayerLost(player);
            cardCheese.playerLooking = false;
        }
    }

    public class CardboardCheese_Disable : CardboardCheese_BaseState
    {
        public CardboardCheese_Disable(CardboardCheese cardCheese) : base(cardCheese) { }

        public override void Initialize()
        {
            base.Initialize();
            cardCheese.cooldown.Restart();
            ChangeNavigationState(new NavigationState_Disabled(npc));
            cardCheese.active = false;
            cardCheese.spriteBase.gameObject.SetActive(false);
        }
    }
}

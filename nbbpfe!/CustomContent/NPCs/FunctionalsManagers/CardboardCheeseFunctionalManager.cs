﻿using nbppfe.FundamentalsManager.Loaders;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using nbppfe.Enums;
using System;

namespace nbppfe.CustomContent.NPCs.FunctionalsManagers
{
    public class CardboardCheeseFunctionalManager : Singleton<CardboardCheeseFunctionalManager>
    {
        public void CheesedSlot(int slot, int player, bool reverse)
        {
            lockSlot = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(14).Take(3).ToArray()[2];
            unlockSlot = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(14).Take(3).ToArray()[0];
            tryUnlockSlot = NPCLoader.soundsObjects[CustomNPCsEnum.CardboardCheese].Skip(14).Take(3).ToArray()[1];


            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(reverse ? lockSlot : unlockSlot);
            int maxItem = Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.maxItem;
            GameObject itemCoverObject = GameObject.Find($"ItemCover_{slot}");
            Image imageComponent = itemCoverObject.GetComponent<Image>();

            int[] selectedSprite = slot == 0 ? initialSlotSprite : slot == maxItem ? endSlotSprite : middleSlotSprite;
            if (reverse)
                StartCoroutine(AnimateEndSlot(imageComponent, selectedSprite, slot, player));
            else
                StartCoroutine(AnimateStartSlot(imageComponent, selectedSprite));
            Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.LockSlot(slot, !reverse);

            if (!reverse)
                slots[slot] = UnityEngine.Random.Range(15, 25);
        }

        public IEnumerator AnimateStartSlot(Image image, int[] sequences)
        {
            float waitTime = 0.1f;
            foreach (var sprite in sequences)
            {
                image.sprite = getSlotSprite(sprite);
                yield return new WaitForSeconds(waitTime);
            }
        }
        public IEnumerator AnimateEndSlot(Image image, int[] sequences, int slot, int player)
        {
            float waitTime = 0.1f;
            for (int i = sequences.Length - 1; i >= 0; i--)
            {
                image.sprite = getSlotSprite(sequences[i]);
                yield return new WaitForSeconds(waitTime);
            }
            image.sprite = getDynamicSlotSprite(slot, player);
        }



        public Sprite getSlotSprite(int slot)
        {
            return Resources.FindObjectsOfTypeAll<Sprite>().Where(x => x.name == $"CheeseItemSlot_SpriteSheet_{slot}").First();
        }

        public Sprite getDynamicSlotSprite(int slot, int player)
        {
            int maxItem = Singleton<CoreGameManager>.Instance.GetPlayer(player).itm.maxItem;
            int index = slot == 0 ? 0 : slot == maxItem ? 2 : 1;
            return Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.name == $"ItemSlot_Dynamic_{index}");
        }

        public void tryUnlock(int slot, int player)
        {
            slots[slot] -= 1;

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(tryUnlockSlot);

            if (slots[slot] <= 0)
                CheesedSlot(slot, player, true);
        }

        public ItemSlotsManager slotManagers;

        public int[] initialSlotSprite = [0, 3, 6, 9];
        public int[] middleSlotSprite = [1, 4, 7, 10];
        public int[] endSlotSprite = [2, 5, 8, 11];

        public int[] slots = new int[99];

        public SoundObject tryUnlockSlot, lockSlot, unlockSlot;
    }
}

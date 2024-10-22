using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.NPCs.FunctionalsManagers;
using nbppfe.Enums;
using PixelInternalAPI.Extensions;
using System.Reflection;
using UnityEngine;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(Bully), nameof(Bully.StealItem))]
    public class BullyPresent
    {
        private static bool Prefix()
        {
            if (Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Has(EnumExtensions.GetFromExtendedName<Items>(CustomItemsEnum.BullyPresent.ToString())))
                return false;
            return true;
        }

        private static void Postfix(Bully __instance)
        {
            if (Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Has(EnumExtensions.GetFromExtendedName<Items>(CustomItemsEnum.BullyPresent.ToString())))
            {
                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Remove(EnumExtensions.GetFromExtendedName<Items>(CustomItemsEnum.BullyPresent.ToString()));
                __instance.Hide();

                AudioManager audMan = (AudioManager)__instance.ReflectionGetVariable("audMan");
                SoundObject[] sounds = (SoundObject[])__instance.ReflectionGetVariable("takeouts");

                audMan.PlayRandomAudio(sounds);

                Debug.Log("Bully Stole Present Forced");
            }

        }
    }

    [HarmonyPatch(typeof(ItemManager))]
    public class ItemManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemManager.UseItem))]
        public static bool UseItem_Prefix()
        {
            var player = Singleton<CoreGameManager>.Instance.GetPlayer(0);
            if (player.itm.IsSlotLocked(player.itm.selectedItem))
            {
                if (Singleton<CardboardCheeseFunctionalManager>.Instance != null)
                    Singleton<CardboardCheeseFunctionalManager>.Instance.tryUnlock(player.itm.selectedItem, player.playerNumber);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Pickup))]
    public class PickupPatch
    {
        public static Pickup lastPíckupClicked;
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Pickup.Clicked))]
        public static void Clicked_Prefix(Pickup __instance) =>
            lastPíckupClicked = __instance;

    }

    //I just copied this from the pixelGuy in the animation mod, out of pure laziness in programming it :/
    [HarmonyPatch(typeof(ItemManager), "RemoveItem")]
    public class LastRemovedItemPatch
    {
        [HarmonyPrefix]
        public static void Prefix(ItemManager __instance, int val) =>
            lastRemovedItem = __instance.items[val];

        public static ItemObject lastRemovedItem;
    }
}

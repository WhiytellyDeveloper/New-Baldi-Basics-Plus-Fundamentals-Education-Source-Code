using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.CustomContent.CustomItems;
using nbppfe.CustomContent.NPCs.FunctionalsManagers;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using PixelInternalAPI.Extensions;
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

    [HarmonyPatch(typeof(SubtitleController), "Hide")]
    public class CaptionTest
    {
        [HarmonyPrefix]
        public static bool Prefix(SubtitleController __instance)
        {
            if (__instance.text.text.Contains(Singleton<LocalizationManager>.Instance.GetLocalizedText("Sfx_CompassPoint")))
                return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(Baldi_Chase), nameof(Baldi_Chase.OnStateTriggerStay))]
    public class ImmatureApplePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Baldi_Chase __instance, Collider other)
        {
            var _instance = other.GetComponent<ItemManager>();

            if (_instance != null)
            {
                if (_instance.Has(CustomItemsEnum.ImmatureApple.ToItemEnum()))
                {
                    var baldi = (Baldi)__instance.ReflectionGetVariable("baldi");
                    Object.Instantiate<ITM_ImmatureApple>(CustomItemsEnum.ImmatureApple.ToItem().item.GetComponent<ITM_ImmatureApple>()).TakeGreenApple(baldi);
                    _instance.Remove(CustomItemsEnum.ImmatureApple.ToItemEnum());
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Principal), nameof(Principal.SendToDetention))]
    internal class FreeDetetion
    {
        public static SoundObject audAngelic = AssetsLoader.CreateSound("AngelicSound", Paths.GetPath(PathsEnum.Items, "HallPass"), "Vfx_AngelicSounds", SoundType.Voice, Color.white, 1);
        public static SoundObject principalVoiceline = AssetsLoader.CreateSound("pri_hallpass", Paths.GetPath(PathsEnum.Items, "HallPass"), "Vfx_PriHallPass", SoundType.Voice, Color.white, 1);

        static void Postfix(Principal __instance)
        {
            if (Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Has(CustomItemsEnum.HallPass.ToItemEnum()))
            {
                foreach (RoomController office in Singleton<BaseGameManager>.Instance.Ec.offices)
                    office.functionObject.GetComponent<DetentionRoomFunction>().ReflectionSetVariable("time", 1);

                foreach (DetentionUi ui in GameObject.FindObjectsOfType<DetentionUi>())
                    ui.gameObject.SetActive(false);

                foreach (RoomController office in Singleton<BaseGameManager>.Instance.Ec.offices)
                {
                    foreach (Door door in office.doors)
                        door.Unlock();
                }

                __instance.GetComponent<AudioManager>().FlushQueue(true);
                __instance.GetComponent<AudioManager>().QueueAudio(principalVoiceline);

                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audAngelic);

                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Remove(CustomItemsEnum.HallPass.ToItemEnum());
            }
        }
    }

    [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.LoadNextLevel))]
    internal class PowerTubePatch
    {

        public static void Prefix()
        {
            if (Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Has(CustomItemsEnum.PowerTube.ToItemEnum()))
            {
                var lives = Singleton<CoreGameManager>.Instance.Lives;
                if (lives == 2)
                    Singleton<CoreGameManager>.Instance.AddPoints(300, 0, false, true);
                else
                    Singleton<CoreGameManager>.Instance.SetLives(lives + 1);

                Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Remove(CustomItemsEnum.PowerTube.ToItemEnum());
            }
        }
    }
}


[HarmonyPatch(typeof(StandardDoor), "OnTriggerEnter")]
internal class LockDoorPatch
{
    public static bool Prefix(StandardDoor __instance)
    {
        if (__instance.locked)
            return false;

        return true;
    }
}

/*
[HarmonyPatch(typeof(ITM_NanaPeel), nameof(ITM_NanaPeel.EntityTriggerStay))]
internal class NanaPellFix
{
    public static bool Prefix(Collider other)
    {
        if (!other.CompareTag("NPC") || !other.CompareTag("Player"))
            return false;

        return true;
    }
}
*/
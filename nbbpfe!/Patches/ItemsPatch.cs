using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbbpfe.Enums;
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
}

using HarmonyLib;
using UnityEngine;

/*
namespace nbbpfe_ // Rename the namespace!
{
    /*
     * At this point, when you're ready to experiment, remove all of the example patches.
     * None of them should've exist in your published mod...
    [HarmonyPatch(typeof(ItemManager), "Awake")]
    internal class ExamplePrivateFuncPatch
    {
        static void Prefix(ItemManager __instance)
        {
            Debug.Log("Slots: " + __instance.items.Length);
        }

        static void Postfix(ItemManager __instance)
        {
            foreach (var item in __instance.items)
                if (item != null && item.itemType != Items.None) Debug.Log("Item: " + item);
        }
    }

    [HarmonyPatch(typeof(Baldi), nameof(Baldi.TakeApple))]
    internal class ExamplePublicPatch
    {
        [HarmonyPostfix]
        static void AnApple(Baldi __instance)
        {
            Debug.LogWarning("Gave Baldi an Apple!");
            __instance.spriteRenderer[0].color = Color.red;
        }
    }

    [HarmonyPatch]
    internal class ExampleMultipatch
    {
        [HarmonyPatch(typeof(Baldi), nameof(Baldi.SlapBreak)), HarmonyPostfix]
        static void AAAAAA()
        {
            Debug.LogWarning("Sounds like Baldi needs a new ruler!");
        }
        [HarmonyPatch(typeof(Baldi), nameof(Baldi.SlapBroken)), HarmonyPostfix]
        static void SlapSound(Baldi __instance, ref SoundObject ___slap)
        {
            __instance.AudMan.PlaySingle(___slap);
        }
        [HarmonyPatch(typeof(ArtsAndCrafters), nameof(ArtsAndCrafters.GetAngry)), HarmonyPostfix]
        static void Swoosh(ref float ___attackSpinSpeed)
        {
            Debug.LogWarning("Crafters attack!");
            ___attackSpinSpeed = 30f;
        }
    }
}
*/

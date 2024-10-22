using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.BasicClasses.Extensions;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(NPC), nameof(NPC.Initialize))]
    internal static class NPCsPatch
    {
        [HarmonyPostfix]
        internal static void AddExtension(NPC __instance)
        {
            if (__instance.gameObject.GetComponent<INPCPrefab>() != null)
                __instance.gameObject.GetComponent<INPCPrefab>().PostLoading();
        }
    }
}

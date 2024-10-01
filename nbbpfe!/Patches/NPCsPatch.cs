using HarmonyLib;
using MTM101BaldAPI;
using nbppfe.BasicClasses.Extensions;
using nbppfe.PrefabSystem;

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

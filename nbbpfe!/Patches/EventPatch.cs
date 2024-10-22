using HarmonyLib;
using MTM101BaldAPI;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalsManager.Loaders;
using UnityEngine;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(RandomEvent), nameof(RandomEvent.Begin))]
    internal static class EventStartPatch
    {
        [HarmonyPrefix]
        internal static void OnEventStart() =>
            EventEndPatch.active = true;

    }

    [HarmonyPatch(typeof(RandomEvent), nameof(RandomEvent.End))]
    internal static class EventEndPatch
    {
        public static bool active = true;
        [HarmonyPrefix]
        internal static void OnEventEnd()
        {
            if (active)
            {
                Singleton<FundamentalsGameController>.Instance.OnEventOver();
                active = false;
            }
        }

    }
}


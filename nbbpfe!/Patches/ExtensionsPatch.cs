using HarmonyLib;
using MTM101BaldAPI;
using nbppfe.BasicClasses.Extensions;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(Window), "Start")]
    internal static class ExamplePrivateFuncPatch
    {
        [HarmonyPostfix]
        internal static void AddExtension(Window __instance) =>
            __instance.gameObject.GetOrAddComponent<WindowExtension>();
        
    }
}

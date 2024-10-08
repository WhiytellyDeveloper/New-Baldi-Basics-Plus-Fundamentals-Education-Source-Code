using HarmonyLib;
using MTM101BaldAPI;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalsManager.Loaders;
using UnityEngine;

namespace nbppfe.Patches
{
    [HarmonyPatch(typeof(CoreGameManager), "Start")]
    internal static class InitializePatch
    {
        [HarmonyPrefix]
        internal static void OnCollect(CoreGameManager __instance) =>
            __instance.gameObject.GetOrAddComponent<FundamentalsGameController>();
        
    }

    [HarmonyPatch(typeof(GameInitializer), "Initialize")]
    internal static class PitLobbyPatch
    {
        public static bool initialized = false;

        [HarmonyPrefix]
        internal static void OverrideFloor()
        {
            if (!initialized && Singleton<CoreGameManager>.Instance.sceneObject.levelTitle == "F1")
            {
                initialized = true;
                Singleton<CoreGameManager>.Instance.sceneObject = SceneLoader.lobbyScene;
                GameObject.FindObjectOfType<ElevatorScreen>().UpdateFloorDisplay();
            }
            
        }
    }

    [HarmonyPatch(typeof(CoreGameManager), nameof(CoreGameManager.Quit))]
    internal static class ImportantPatch
    {
        [HarmonyPrefix]
        internal static void Placeholder() {
            PitLobbyPatch.initialized = false;
            Singleton<FundamentalsGameController>.Instance.OnExit();
        }
    }

    [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.EndGame))]
    internal static class ImportantPatch2
    {
        [HarmonyPrefix]
        internal static void Placeholder() { 
            Singleton<FundamentalsGameController>.Instance.OnReloadLevel();
        }
    }

    [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.CollectNotebooks))]
    internal static class NotebookPatch
    {
        [HarmonyPrefix]
        internal static void OnCollect(int count) {
            Singleton<FundamentalsGameController>.Instance.noteboooks =+ count;
        }
    }

    [HarmonyPatch(typeof(MainGameManager), nameof(MainGameManager.BeginPlay))]
    internal static class NotebooksPatch
    {
        [HarmonyPrefix]
        internal static void OnCollect() {
            Singleton<FundamentalsGameController>.Instance.AddNotebooks();
        }
    }
}


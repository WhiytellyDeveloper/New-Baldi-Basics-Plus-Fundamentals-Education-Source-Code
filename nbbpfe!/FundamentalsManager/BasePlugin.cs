using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Collections;
using UnityEngine;

namespace nbbpfe.FundamentalsManager
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", MTM101BaldiDevAPI.VersionNumber)]
    [BepInProcess("BALDI.exe")]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; private set; }

        private void Awake()
        {
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            Instance = this;
            harmony.PatchAllConditionals();

            LoadingEvents.RegisterOnAssetsLoaded(Info, PreLoad(), false);
            //LoadingEvents.RegisterOnAssetsLoaded(Info, PostLoad(), true);

            ModdedSaveGame.AddSaveHandler(Info);
        }

        private IEnumerator PreLoad()
        {
            yield return 1;
            yield return "Lodaing...";
        }

        /*
        IEnumerator PostLoad()
        {
            yield return 1;
            yield return "Postload message";
        }
        */
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.newbaldibasicspluseducationalfundamentals";
        public const string PLUGIN_NAME = "New Baldi's Basics Plus Educational Fundamentals";
        public const string PLUGIN_VERSION = "1.0";
    }
}

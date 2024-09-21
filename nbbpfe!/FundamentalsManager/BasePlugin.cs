using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using nbbpfe.FundamentalsManager.Loaders;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;
using static nbbpfe.FundamentalsManager.FundamentalLoaderManager;
using System.Linq;

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
            ModdedSaveGame.AddSaveHandler(Info);

            GeneratorManagement.Register(this, GenerationModType.Base, (floorName, floorNum, ld) =>
            {
                RoomGroup[] groups = [ld.roomGroup.Where(x => x.name.Contains("Class")).First(), ld.roomGroup.Where(x => x.name.Contains("Faculty")).First(), ld.roomGroup.Where(x => x.name.Contains("Office")).First()];
                foreach (FloorData floorData in FundamentalLoaderManager.floors)
                {
                    if (floorData.Floor == floorName)
                    {
                        ld.hallWallTexs = ld.hallWallTexs.AddRangeToArray(floorData.wallTextures[RoomTextures.Hall].ToArray());
                        ld.hallFloorTexs = ld.hallFloorTexs.AddRangeToArray(floorData.floorTextures[RoomTextures.Hall].ToArray());
                        ld.hallCeilingTexs = ld.hallCeilingTexs.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Hall].ToArray());

                        groups[0].wallTexture = groups[0].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Class].ToArray());
                        groups[0].floorTexture = groups[0].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Class].ToArray());
                        groups[0].ceilingTexture = groups[0].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Class].ToArray());

                        groups[1].wallTexture = groups[1].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Faculty].ToArray());
                        groups[1].floorTexture = groups[1].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Faculty].ToArray());
                        groups[1].ceilingTexture = groups[1].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Faculty].ToArray());

                        groups[2].wallTexture = groups[2].wallTexture.AddRangeToArray(floorData.wallTextures[RoomTextures.Faculty].ToArray());
                        groups[2].floorTexture = groups[2].floorTexture.AddRangeToArray(floorData.floorTextures[RoomTextures.Faculty].ToArray());
                        groups[2].ceilingTexture = groups[2].ceilingTexture.AddRangeToArray(floorData.ceilingTextures[RoomTextures.Faculty].ToArray());
                    }
                }
            });
        }
        private IEnumerator PreLoad()
        {
            yield return 1;
            yield return "Lodaing...";

            /*
            RoomTextureData roomTextureData = new RoomTextureData();
            string json = JsonConvert.SerializeObject(roomTextureData, Newtonsoft.Json.Formatting.Indented);
            string path = Path.Combine(Application.streamingAssetsPath, "Textures.data");
            File.WriteAllText(path, json);
            */

            Paths.Initialize();
            FundamentalLoaderManager.Initialize();
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.newbaldibasicspluseducationalfundamentals";
        public const string PLUGIN_NAME = "New Baldi's Basics Plus Educational Fundamentals";
        public const string PLUGIN_VERSION = "1.0";
    }
}

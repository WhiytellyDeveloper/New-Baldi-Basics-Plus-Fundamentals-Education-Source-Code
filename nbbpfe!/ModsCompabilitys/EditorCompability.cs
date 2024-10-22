using BaldiLevelEditor;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using nbppfe.FundamentalsManager;
using PlusLevelFormat;
using PlusLevelLoader;
using System.Collections.Generic;
using UnityEngine;
namespace nbppfe.ModsCompabilitys
{
    [HarmonyPatch]
    [ConditionalPatchMod("mtm101.rulerp.baldiplus.leveleditor")]
    public static class EditorCompability
    {
        [HarmonyPatch(typeof(BasePlugin), nameof(BasePlugin.PostLoading))]
        [HarmonyPostfix]
        private static void MakeEditorSeeAssets()
        {
            AssetsLoader.CreateSprites(35, Paths.GetPath(PathsEnum.Editor));
            AddRotatingObject("Mannequin", AssetsLoader.Get<GameObject>("Mannequin"), Vector3.up * 3.54f);
            AddRotatingObject("Mirror", AssetsLoader.Get<GameObject>("Mirror"), Vector3.up * 5f);
            AddRotatingObject("WelcomebackCarpet", AssetsLoader.Get<GameObject>("WelcomebackCarpet"), Vector3.zero);
            AddRotatingObject("PlaygroundCarpet", AssetsLoader.Get<GameObject>("PlaygroundCarpet"), Vector3.zero);
            AddRotatingObject("ParkBench", AssetsLoader.Get<GameObject>("ParkBench"), Vector3.up * 2.14f);
            AddRotatingObject("Flower", AssetsLoader.Get<GameObject>("Flower"), Vector3.up * 1.29f);
            AddRotatingObject("Cage", AssetsLoader.Get<GameObject>("Cage"), Vector3.up);

            AddRoom("CheapStore");
        }

        [HarmonyPatch(typeof(PlusLevelEditor), "Initialize")]
        [HarmonyPostfix]
        static void InitializeStuff(PlusLevelEditor __instance)
        {
            __instance.toolCats.Find((x) => x.name == "halls").tools.AddRange(rooms);
            __instance.toolCats.Find(x => x.name == "objects").tools.AddRange(objects);
        }

        [HarmonyPatch(typeof(EditorLevel), "InitializeDefaultTextures")]
        [HarmonyPostfix]
        private static void AddRoomTexs(EditorLevel __instance)
        {
            __instance.defaultTextures.Add("CheapStore", new("CheapFloor", "CheapWall", "CheapCeiling"));
        }

        static void AddRotatingObject(string name, GameObject obj, Vector3 offset)
        {
            objects.Add(new(name));
            EditorObjectType objConverted = EditorObjectType.CreateFromGameObject<EditorPrefab, PrefabLocation>(name, obj, offset, false);
            BaldiLevelEditorPlugin.editorObjects.Add(objConverted);
        }

        public static void AddRoom(string room) =>
            rooms.Add(new(room));

        public static List<CustomObjectEditor> objects = [];
        public static List<CustomRoomEditor> rooms = [];
    }

    public class CustomObjectEditor : RotateAndPlacePrefab
    {
        string obj;
        public override Sprite editorSprite
        {
            get { Debug.Log("Editor_" + obj); return AssetsLoader.Get<Sprite>("Editor_" + obj); }
        }

        public CustomObjectEditor(string obj) : base(obj) =>
            this.obj = obj;
    }

    public class CustomRoomEditor : FloorTool
    {
        string obj;
        public override Sprite editorSprite
        {
            get { Debug.Log("Editor_" + obj); return AssetsLoader.Get<Sprite>("Editor_" + obj); }
        }

        public CustomRoomEditor(string obj) : base(obj) =>
            this.obj = obj;

    }
}

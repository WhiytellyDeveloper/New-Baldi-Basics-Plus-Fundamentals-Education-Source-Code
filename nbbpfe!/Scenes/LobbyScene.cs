using BepInEx.Bootstrap;
using nbppfe.BasicClasses.Functions;
using nbppfe.BasicClasses;
using nbppfe.FundamentalsManager;
using PlusLevelLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MTM101BaldAPI;
using PlusLevelFormat;
using System.Linq;
using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager.Loaders;

namespace nbppfe.Scenes
{
    internal class LobbyScene : PlusScene
    {
        public override void LoadEverything()
        {
            base.LoadEverything();
            try
            {
                using (var reader = new BinaryReader(File.OpenRead(Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby/PitLobby.cbld"))))
                {
                    var manager = new GameObject("PitLobbyManager");
                    manager.ConvertToPrefab(true);
                    var lobbyManager = manager.AddComponent<PitLobbyGameManager>();
                    lobbyManager.ReflectionSetVariable("elevatorScreenPre", Resources.FindObjectsOfTypeAll<ElevatorScreen>().First());

                    var mainLevel = Resources.FindObjectsOfTypeAll<SceneObject>().FirstOrDefault(x => x.name == "MainLevel_1");
                    var lobbyScene = CustomLevelLoader.LoadLevel(reader.ReadLevel());
                    lobbyScene.name = "main_lobby";
                    lobbyScene.usesMap = false;
                    lobbyScene.levelNo = -1;
                    lobbyScene.nextLevel = SceneLoader.F0;
                    lobbyScene.levelTitle = "LBY";
                    lobbyScene.skybox = mainLevel.skybox;
                    lobbyScene.manager = lobbyManager;

                    // Room 0 setup
                    var room0 = lobbyScene.levelAsset.rooms[0];
                    room0.wallTex = AssetsLoader.Get<Texture2D>("F1!F2!END!BrickWillyWhite");
                    room0.florTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!Calpert_Gray");
                    room0.ceilTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!NyanCeiling");

                    // Room 1 setup with custom skybox function
                    var room1 = lobbyScene.levelAsset.rooms[1];
                    room1.wallTex = AssetsLoader.CreateTexture("alternateFence2", Paths.GetPath(PathsEnum.Misc));
                    var room1Functions = (List<RoomFunction>)room1.roomFunctionContainer.ReflectionGetVariable("functions");
                    var skybox = room1.roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>().skybox;
                    room1Functions.Remove(room1.roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>());

                    var lobbySkybox = room1.roomFunctionContainer.gameObject.AddComponent<LobbySkyboxRoomFunction>();
                    lobbySkybox.skybox = skybox;
                    room1.roomFunctionContainer.AddFunction(lobbySkybox);
                    UnityEngine.Object.Destroy(room1.roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>());

                    var room2 = lobbyScene.levelAsset.rooms[2];
                    room2.category = RoomCategory.Null;

                    // Room 3 setup
                    var room3 = lobbyScene.levelAsset.rooms[3];
                    room3.wallTex = AssetsLoader.CreateTexture("BookHolder", Paths.GetPath(PathsEnum.Misc));
                    room3.florTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!TheifCarpet1");
                    room3.ceilTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!FancyCeiling");

                    var room3Functions = (List<RoomFunction>)room3.roomFunctionContainer.ReflectionGetVariable("functions");
                    if (Chainloader.PluginInfos.ContainsKey("pixelguy.pixelmodding.baldiplus.bbextracontent"))
                        room3Functions.RemoveAt(2);               

                    // Room 4 setup
                    var room4 = lobbyScene.levelAsset.rooms[4];
                    room4.wallTex = AssetsLoader.Get<Texture2D>("F1!F2!END!OfficeBrickWillyWhite");
                    room4.florTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!LiteryNotebookFloorMixed");
                    room4.ceilTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!BasicRealCeiling");

                    // Poster setup
                    var posters = lobbyScene.levelAsset.posters;
                    posters.Add(new PosterData { position = new(4, 4), direction = Direction.South, poster = Resources.FindObjectsOfTypeAll<PosterObject>().First(x => x.name == "HNT_Rules") });
                    posters.Add(new PosterData { position = new(11, 7), direction = Direction.East, poster = Resources.FindObjectsOfTypeAll<PosterObject>().First(x => x.name == "HNT_Boots") });
                    posters.Add(new PosterData { position = new(2, 22), direction = Direction.North, poster = Resources.FindObjectsOfTypeAll<PosterObject>().First(x => x.name == "CLS_BaldiBuried") });
                    posters.Add(new PosterData { position = new(12, 9), direction = Direction.East, poster = ObjectCreators.CreatePosterObject(AssetsLoader.CreateTexture("NoIdeaPosterText", Paths.GetPath(PathsEnum.Posters)), []) });
                    posters.Add(new PosterData { position = new(15, 21), direction = Direction.East, poster = Resources.FindObjectsOfTypeAll<PosterObject>().First(x => x.name == "BLT_Hawaii") });
                    posters.Add(new PosterData { position = new(10, 5), direction = Direction.West, poster = ObjectCreators.CreatePosterObject(AssetsLoader.CreateTexture("advanceed", Paths.GetPath(PathsEnum.Posters)), []) });
                    posters.Add(new PosterData { position = new(7, 15), direction = Direction.South, poster = Resources.FindObjectsOfTypeAll<PosterObject>().First(x => x.name == "CLS_BaldiSays_5") });
                    posters.Add(new PosterData { position = new(2, 4), direction = Direction.East, poster = ObjectCreators.CreatePosterObject(AssetsLoader.CreateTexture("TimesPoster", Paths.GetPath(PathsEnum.Posters)), []) });

                    lobbyScene.MarkAsNeverUnload();
                    SceneLoader.lobbyScene = lobbyScene;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao criar o lobby scene: {ex.Message}");
            }

        }
    }
}

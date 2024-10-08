using MTM101BaldAPI;
using nbbpfe.FundamentalsManager;
using nbppfe.BasicClasses;
using PlusLevelFormat;
using PlusLevelLoader;
using System.IO;
using System;
using UnityEngine;
using MTM101BaldAPI.Reflection;
using System.Linq;
using System.Collections.Generic;
using nbppfe.BasicClasses.Functions;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class SceneLoader
    {
        public static SceneObject lobbyScene;

        public static void Load()
        {
            try
            {
                using (BinaryReader reader = new(File.OpenRead(Paths.GetPath(PathsEnum.PreMadeRooms, "PitLobby/PitLobby.cbld"))))
                {
                    //CreateTipNotebook(0, "CharcterBook");

                    var manager = new GameObject("PitLobbyManager");
                    manager.ConvertToPrefab(true);
                    var lobbyManager = manager.AddComponent<PitLobbyGameManager>();
                    lobbyManager.ReflectionSetVariable("elevatorScreenPre", Resources.FindObjectsOfTypeAll<ElevatorScreen>().First());

                    lobbyScene = CustomLevelLoader.LoadLevel(LevelExtensions.ReadLevel(reader));
                    lobbyScene.name = "main_lobby";
                    lobbyScene.usesMap = false;
                    lobbyScene.levelNo = -1;
                    lobbyScene.nextLevel = Resources.FindObjectsOfTypeAll<SceneObject>().FirstOrDefault(x => x.name == "MainLevel_1");
                    lobbyScene.levelTitle = "LBY";
                    lobbyScene.skybox = Resources.FindObjectsOfTypeAll<SceneObject>().FirstOrDefault(x => x.name == "MainLevel_1").skybox;
                    lobbyScene.manager = lobbyManager;

                    lobbyScene.levelAsset.rooms[0].wallTex = AssetsLoader.Get<Texture2D>("F1!F2!END!BrickWillyWhite");
                    lobbyScene.levelAsset.rooms[0].florTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!Calpert_Gray");
                    lobbyScene.levelAsset.rooms[0].ceilTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!NyanCeiling");

                    lobbyScene.levelAsset.rooms[1].wallTex = AssetsLoader.CreateTexture("alternateFence2", Paths.GetPath(PathsEnum.Misc));
                    var list = (List<RoomFunction>)lobbyScene.levelAsset.rooms[1].roomFunctionContainer.ReflectionGetVariable("functions");
                    var skybox = lobbyScene.levelAsset.rooms[1].roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>().skybox;
                    list.Remove(lobbyScene.levelAsset.rooms[1].roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>());
                    var function = lobbyScene.levelAsset.rooms[1].roomFunctionContainer.gameObject.AddComponent<LobbySkyboxRoomFunction>();
                    function.skybox = skybox;
                    function.gameObject.ConvertToPrefab(true);
                    lobbyScene.levelAsset.rooms[1].roomFunctionContainer.AddFunction(function);
                    GameObject.Destroy(lobbyScene.levelAsset.rooms[1].roomFunctionContainer.GetComponentInChildren<SkyboxRoomFunction>());


                    lobbyScene.levelAsset.rooms[3].wallTex = AssetsLoader.CreateTexture("BookHolder", Paths.GetPath(PathsEnum.Misc));
                    lobbyScene.levelAsset.rooms[3].florTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!TheifCarpet1");
                    lobbyScene.levelAsset.rooms[3].ceilTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!FancyCeiling");
                    var list2 = (List<RoomFunction>)lobbyScene.levelAsset.rooms[3].roomFunctionContainer.ReflectionGetVariable("functions");

                    if (list.Count > 1)
                        list2.RemoveAt(2);

                    lobbyScene.levelAsset.rooms[4].wallTex = AssetsLoader.Get<Texture2D>("F1!F2!END!OfficeBrickWillyWhite");
                    lobbyScene.levelAsset.rooms[4].florTex = AssetsLoader.Get<Texture2D>("F2!F3!F4!END!LiteryNotebookFloorMixed");
                    lobbyScene.levelAsset.rooms[4].ceilTex = AssetsLoader.Get<Texture2D>("F1!F2!F3!F4!END!BasicRealCeiling");

                    Texture2D posterTexture = AssetsLoader.CreateTexture("NoIdeaPosterText", Paths.GetPath(PathsEnum.Posters));
                    Texture2D posterTexture2 = AssetsLoader.CreateTexture("TimesPoster", Paths.GetPath(PathsEnum.Posters));
                    Texture2D posterTexture3 = AssetsLoader.CreateTexture("advanceed", Paths.GetPath(PathsEnum.Posters));

                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(4, 4), direction = Direction.South, poster = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name == "HNT_Rules").First() });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(11, 7), direction = Direction.East, poster = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name == "HNT_Boots").First() });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(2, 22), direction = Direction.North, poster = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name == "CLS_BaldiBuried").First() });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(12, 9), direction = Direction.East, poster = ObjectCreators.CreatePosterObject(posterTexture, []) });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(15, 21), direction = Direction.East, poster = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name == "BLT_Hawaii").First() });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(10, 5), direction = Direction.West, poster = ObjectCreators.CreatePosterObject(posterTexture3, []) }); //Advanced
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(7, 15), direction = Direction.South, poster = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name == "CLS_BaldiSays_5").First() });
                    lobbyScene.levelAsset.posters.Add(new PosterData { position = new IntVector2(2, 4), direction = Direction.East, poster = ObjectCreators.CreatePosterObject(posterTexture2, []) });

                    lobbyScene.MarkAsNeverUnload();

                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao criar o lobby scene: {ex.Message}");
            }
        }

        private static void CreateTipNotebook(int id, string name)
        {
            var book = Resources.FindObjectsOfTypeAll<Notebook>().First();
            book.name = $"TipBook_{id}";
            book.GetComponentInChildren<SpriteRenderer>().sprite = AssetsLoader.CreateSprite(book.name, Paths.GetPath(PathsEnum.PreMadeRooms, "PitLobby"), 100);
            GameObject.Destroy(book.GetComponent<Notebook>());
            var tipBook = book.gameObject.AddComponent<TipBook>();
            tipBook.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add<TipBook>($"TipBook_{id}", tipBook);
        }
    }
}

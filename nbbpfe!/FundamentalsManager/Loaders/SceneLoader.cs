using MTM101BaldAPI;
using PlusLevelFormat;
using PlusLevelLoader;
using System.IO;
using System;
using UnityEngine;
using MTM101BaldAPI.Reflection;
using System.Linq;
using System.Collections.Generic;
using nbppfe.FundamentalsManager;
using nbppfe.BasicClasses;
using nbppfe.BasicClasses.CustomObjects;
using nbppfe.BasicClasses.Functions;
using BepInEx.Bootstrap;
using nbppfe.Scenes;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class SceneLoader
    {
        public static SceneObject lobbyScene;
        public static SceneObject F0;

        public static void Load()
        {
            //new Floor0Scene().LoadEverything();
            new LobbyScene().LoadEverything();
        }

        private static void CreateTipNotebook(int id, string name)
        {
            var book = Resources.FindObjectsOfTypeAll<Notebook>().First();
            book.name = $"TipBook_{id}";
            book.GetComponentInChildren<SpriteRenderer>().sprite = AssetsLoader.CreateSprite(book.name, Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 100);
            UnityEngine.Object.Destroy(book.GetComponent<Notebook>());
            var tipBook = book.gameObject.AddComponent<TipBook>();
            tipBook.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add($"TipBook_{id}", tipBook);
        }
    }
}

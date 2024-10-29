using MTM101BaldAPI;
using nbppfe.BasicClasses.CustomObjects;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using PixelInternalAPI.Extensions;
using PlusLevelLoader;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static class CustomObjectsLoader
    {
        public static void Load()
        {
            Sprite spriteMannequin = AssetsLoader.CreateSprite("Mannequin", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 40);
            var mannequin = ObjectCreationExtensions.CreateSpriteBillboard(spriteMannequin);
            mannequin.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("Mannequin", mannequin.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("Mannequin", mannequin.gameObject);

            Sprite spriteMirror = AssetsLoader.CreateSprite("Mirror", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 30);
            var mirror = ObjectCreationExtensions.CreateSpriteBillboard(spriteMirror, false);
            mirror.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("Mirror", mirror.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("Mirror", mirror.gameObject);

            Sprite spriteCarpetWelcome = AssetsLoader.CreateSprite("WelcomebackCarpet", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 40);
            var welcomeCarpet = ObjectCreationExtensions.CreateSpriteBillboard(spriteCarpetWelcome, false).AddSpriteHolder(out var renderer, 0.01f).renderers[0].GetComponent<SpriteRenderer>();
            var welcCapt = welcomeCarpet.transform.parent;
            welcomeCarpet.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            welcCapt.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("WelcomebackCarpet", welcCapt.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("WelcomebackCarpet", welcCapt.gameObject);

            Sprite spriteCarpetPlayground = AssetsLoader.CreateSprite("OutsideCarpet", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 40);
            var playgroundCarpet = ObjectCreationExtensions.CreateSpriteBillboard(spriteCarpetPlayground, false).AddSpriteHolder(out var renderer2, 0.01f).renderers[0].GetComponent<SpriteRenderer>();
            var playgroundCapt = playgroundCarpet.transform.parent;
            playgroundCarpet.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            playgroundCapt.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("PlaygroundCarpet", playgroundCapt.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("PlaygroundCarpet", playgroundCapt.gameObject);

            Sprite spriteParkBench = AssetsLoader.CreateSprite("ParkBench", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 15);
            var parkBench = ObjectCreationExtensions.CreateSpriteBillboard(spriteParkBench, false);
            parkBench.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("ParkBench", parkBench.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("ParkBench", parkBench.gameObject);

            Sprite spriteFlower = AssetsLoader.CreateSprite("EVERYTHINGWILLNOTBEOKAY", Paths.GetPath(PathsEnum.PreMadeFloors, "PitLobby"), 80);
            var flower = ObjectCreationExtensions.CreateSpriteBillboard(spriteFlower);
            flower.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("Flower", flower.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("Flower", flower.gameObject);

            Texture2D cageTexture = AssetsLoader.CreateTexture("DiamongPlateFloor", Paths.GetPath(PathsEnum.Misc));
            var cageBack = ObjectCreationExtension.CreateCube(cageTexture, false);
            cageBack.transform.localPosition -= new Vector3(0, 0.75f, 0);
            cageBack.transform.localScale = new Vector3(8, -0.5f, 8);

            var cageUp = ObjectCreationExtension.CreateCube(cageTexture, false);
            cageUp.transform.localPosition += new Vector3(0, 8f, 0);
            cageUp.transform.localScale = new Vector3(8, -0.5f, 8);

            List<MeshRenderer> renderes = [];

            CreateBar(new Vector3(3.2f, 3f, -3.2f));
            CreateBar(new Vector3(0f, 3f, -3.2f));
            CreateBar(new Vector3(-3.2f, 3f, -3.2f));
            CreateBar(new Vector3(3.2f, 3f, 0f));
            CreateBar(new Vector3(-3.2f, 3f, 0f));
            CreateBar(new Vector3(3.2f, 3f, 3.2f));
            CreateBar(new Vector3(0f, 3f, 3.2f));
            CreateBar(new Vector3(-3.2f, 3f, 3.2f));

            void CreateBar(Vector3 position)
            {
                var cageBarTest = ObjectCreationExtension.CreateCube(cageTexture, false);
                cageBarTest.transform.localPosition = position;
                cageBarTest.transform.localScale = new Vector3(1, 10f, 1);
                cageBarTest.name = "Bar";
                renderes.Add(cageBarTest.GetComponent<MeshRenderer>());
            }

            var cageParent = new GameObject("Cage").AddComponent<EmillyCage>();
            cageBack.transform.SetParent(cageParent.transform);
            cageUp.transform.SetParent(cageParent.transform);

            foreach (var bar in renderes)
                bar.transform.SetParent(cageParent.transform);
            cageParent.bars = renderes;

            renderes.Add(cageUp.GetComponent<MeshRenderer>());
            renderes.Add(cageBack.GetComponent<MeshRenderer>());
            cageParent.gameObject.AddComponent<RendererContainer>().renderers = renderes.ToArray();
            cageParent.audMan = cageParent.gameObject.CreatePropagatedAudioManager();
            cageParent.close = AssetsLoader.CreateSound("CageClose", Paths.GetPath(PathsEnum.CustomObjects, "Cage"), "*Cage Closing*", SoundType.Effect, Color.white, 1);
            cageParent.open = AssetsLoader.CreateSound("CageOpen", Paths.GetPath(PathsEnum.CustomObjects, "Cage"), "*Cage Opening*", SoundType.Effect, Color.white, 1);
            cageParent.gameObject.ConvertToPrefab(true);
            AssetsLoader.assetMan.Add("Cage", cageParent.gameObject);
            PlusLevelLoaderPlugin.Instance.prefabAliases.Add("Cage", cageParent.gameObject);
        }
    }
}

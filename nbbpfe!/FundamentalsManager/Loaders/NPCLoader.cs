﻿using MTM101BaldAPI.ObjectCreation;
using nbbpfe.FundamentalsManager;
using nbppfe.CustomContent.NPCs;
using nbppfe.CustomData;
using System.IO;
using System.Linq;
using UnityEngine;
using nbppfe.Extensions;
using nbppfe.Enums;
using System.Collections.Generic;
using PixelInternalAPI.Extensions;
using Newtonsoft.Json;
using nbppfe.PrefabSystem;
using static nbbpfe.FundamentalsManager.FundamentalLoaderManager;
using nbbpfe.FundamentalsManager.Loaders;
using MTM101BaldAPI;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static partial class NPCLoader
    {
        public static Dictionary<CustomNPCsEnum, Sprite[]> spritesFormSpritesheet = [];
        public static Dictionary<CustomNPCsEnum, SoundObject[]> soundsObjects = [];

        public static void SetupNPCs()
        {
            NPC kawa = new NPCBuilder<Kawa>(BasePlugin.Instance.Info)
            .SetupAll("Kawa", CustomNPCsEnum.Kawa, "#C9D3EF", "", false, [RoomCategory.Faculty, RoomCategory.Special])
            .SetMinMaxAudioDistance(50, 100)
            .IgnoreBelts()
            .Build(-1.046f, AssetsLoader.SetHexaColor("#C9D3EF"), "Kawa_0")
            .MakeItWeightedNPC(["F1", "F2", "END"], [46, 30, 70]);

            /* Later...
            NPC cardboardCheese = new NPCBuilder<CardboardCheese>(BasePlugin.Instance.Info)
            .SetName(NPCLoaderExtenssion.LoadFile("CardboardCheese").name)
            .SetEnum(CustomNPCsEnum.CardboardCheese.ToString())
            .SetMinMaxAudioDistance(150, 300)
            .AddSpawnableRoomCategories(RoomCategory.Hall)
            .AddLooker()
            .SetMaxSightDistance(1500)
            .SetupPoster("CardboardCheese")
            .SetupSprites("CardboardCheese", false, "", true) //NormalSprites
            .SetupSprites("CardboardCheese", true, "_Slot") //Spritesheet
            .SetupSounds("CardboardCheese", "#D99245", true)
            .SetStationary()
            .Build(-0.733f, AssetsLoader.SetHexaColor("#D99245"), "CartboardCheeseV2")
            .MakeItForcedNPC(["F1", "END"]);
            */

            /* Later... 2 lol
            NPC shadowFollower = new NPCBuilder<Follower>(BasePlugin.Instance.Info)
            .SetName(NPCLoaderExtenssion.LoadFile("Follower").name)
            .SetEnum(CustomNPCsEnum.ShadowFollower.ToString())
            .AddLooker().SetMaxSightDistance(100)
            .SetMinMaxAudioDistance(75, 150)
            .AddSpawnableRoomCategories([RoomCategory.Hall])
            .SetupPoster("Follower")
            .SetupSprites("Follower", false)
            .SetupSounds("Follower", "#31323F", false)
            .IgnoreBelts()
            .Build(0f, AssetsLoader.SetHexaColor("#31323F"), "Follower")
            .MakeItForcedNPC(["F1", "END"]);
            */


            /* Later.. 3 
            NPC digitalArtistic = new NPCBuilder<DigitalArtist>(BasePlugin.Instance.Info)
            .SetupAll("DigitalArtistic", CustomNPCsEnum.DigitalArtist, "#1B131B", "", true, [RoomCategory.Hall])
            .AddLooker().SetMaxSightDistance(135)
            .SetMinMaxAudioDistance(100, 145)
            .Build(-1.21f, AssetsLoader.SetHexaColor("#1B131B"), "DigitalArtistic_0")
            .MakeItForcedNPC(["F1", "END"]);
            */

            NPC emmilyGutter = new NPCBuilder<EmillyGutter>(BasePlugin.Instance.Info)
            .SetupAll(npcName: "EmillyGutter", npcEnum: CustomNPCsEnum.EmillyGutter, hexaCode: "#00FF49", spriteSheetPrefix: "", debug: false, categorys: [RoomCategory.Hall])
            .AddLooker().SetMaxSightDistance(80)
            .IgnoreBelts()
            .AddPotentialRoomAsset(AssetsLoader.Get<RoomAsset>("EmellyGutterFacutlyRoom1"), 100)
            .SetMinMaxAudioDistance(45, 100)
            .Build(-1.705f, AssetsLoader.SetHexaColor("#A36508"), "EmillyGutter_0")
            .MakeItWeightedNPC(["F2", "F3", "F4", "END"], [75, 45, 20, 54]);

            NPC Slimely = new NPCBuilder<Slimely>(BasePlugin.Instance.Info)
            .SetupAll(npcName: "Slimely", npcEnum: CustomNPCsEnum.Slimely, hexaCode: "#A36508", spriteSheetPrefix: "", debug: true, categorys: [RoomCategory.Hall])
            .IgnoreBelts()
            .SetMinMaxAudioDistance(100, 145)
            .Build(-2.18f, AssetsLoader.SetHexaColor("#A36508"), "Slimely_0")
            .MakeItForcedNPC(["F1", "END"]);

        }
    }

    public static class NPCLoaderExtenssion
    {
        public static FileNPCData LoadFile(string npc)
        {
            string path = Paths.GetPath(PathsEnum.NPCs, [npc, "NpcData.data"]);
            string json = File.ReadAllText(path);
            FileNPCData npcData = JsonConvert.DeserializeObject<FileNPCData>(json);
            return npcData;
        }

        public static NPCBuilder<T> SetupPoster<T>(this NPCBuilder<T> builder, string npcName) where T : NPC
        {
            FileNPCData data = LoadFile(npcName);
            string[] sprites = Directory.GetFiles(Paths.GetPath(PathsEnum.NPCs, npcName), "*.png", SearchOption.AllDirectories);
            Texture2D posterTexture = Resources.FindObjectsOfTypeAll<PosterObject>().Where(x => x.name.Contains("Chk_BaldiSays")).First().baseTexture; //If don't have texture, will be obvius lol

            foreach(string sprite in sprites)
            {
                if (sprite.Contains("pri_"))
                    posterTexture = AssetsLoader.CreateTexture(Path.GetFileNameWithoutExtension(sprite), Paths.GetPath(PathsEnum.NPCs, npcName));
            }

            builder.SetPoster(posterTexture, data.pri_namekey, data.pri_descriptionkey);
            return builder;
        }

        public static NPCBuilder<T> SetupSprites<T>(this NPCBuilder<T> builder, string npcName, CustomNPCsEnum npcEnum, bool spriteSheet, string spriteSheetPostfix = "", bool debug = false) where T : NPC
        {
            FileNPCData data = LoadFile(npcName);
            string[] sprites = Directory.GetFiles(Paths.GetPath(PathsEnum.NPCs, npcName), "*.png", SearchOption.AllDirectories);

            void SaveSpriteAsPng(Sprite sprite, string path)
            {
                Texture2D texture = sprite.texture;
                Rect rect = sprite.textureRect;
                Texture2D newTexture = new Texture2D((int)rect.width, (int)rect.height);
                Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
                newTexture.SetPixels(pixels);
                newTexture.Apply();
                byte[] pngData = newTexture.EncodeToPNG();
                System.IO.File.WriteAllBytes(path, pngData);
                Object.DestroyImmediate(newTexture);
            }

            foreach (string sprite in sprites)
            {
                if (spriteSheet)
                {
                    foreach (KeyValuePair<string, Dictionary<int, Dictionary<int, int>>> sps in data.spritesSheets)
                    {
                        if (sps.Key == Path.GetFileNameWithoutExtension(sprite))
                        {
                            foreach (KeyValuePair<int, Dictionary<int, int>> spriteSheetEntry in sps.Value)
                            {
                                int pixelPerUnit = spriteSheetEntry.Key;
                                Dictionary<int, int> spriteData = spriteSheetEntry.Value;
                                foreach (KeyValuePair<int, int> spriteInfo in spriteData)
                                {
                                    int horizontal = spriteInfo.Key;
                                    int vertical = spriteInfo.Value;
                                    Sprite[] newSprites = TextureExtensions.LoadSpriteSheet(horizontal, vertical, pixelPerUnit, Paths.GetPath(PathsEnum.NPCs, [npcName, sprite]));

                                    string debugFolderPath = Path.Combine(Paths.GetPath(PathsEnum.NPCs, npcName), "Debug");
                                    if (!Directory.Exists(debugFolderPath) && debug)
                                        Directory.CreateDirectory(debugFolderPath);
                                    
                                    for (int i = 0; i < newSprites.Length; i++)
                                    {
                                        if (debug)
                                            Debug.Log($"{npcName}{spriteSheetPostfix}_{i}");
                                        newSprites[i].name = $"{npcName}{spriteSheetPostfix}_{i}";
                                        AssetsLoader.assetMan.Add($"{npcName}{spriteSheetPostfix}_{i}", newSprites[i]);
                                        if (debug)
                                        {
                                            string spritePath = Path.Combine(debugFolderPath, $"{newSprites[i].name}.png");
                                            SaveSpriteAsPng(newSprites[i], spritePath);
                                        }
                                    }
                                    NPCLoader.spritesFormSpritesheet.Add(npcEnum, newSprites);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, int> sps in data.sprites)
                    {
                        if (sps.Key == Path.GetFileNameWithoutExtension(sprite))
                        {
                            Sprite newSprite = AssetsLoader.CreateSprite(Path.GetFileNameWithoutExtension(sprite), Paths.GetPath(PathsEnum.NPCs, npcName), sps.Value);
                            if (debug)
                                Debug.Log($"{Path.GetFileNameWithoutExtension(sprite)}");
                            AssetsLoader.assetMan.Add($"{Path.GetFileNameWithoutExtension(sprite)}", newSprite);
                        }
                    }
                }
            }

            return builder;
        }

        public static NPCBuilder<T> SetupSounds<T>(this NPCBuilder<T> builder, string npcName, CustomNPCsEnum npcEnum, string hexaSound, bool debug = false) where T : NPC
        {
            FileNPCData data = LoadFile(npcName);
            string[] soundsFiles = Directory.GetFiles(Paths.GetPath(PathsEnum.NPCs, npcName), "*.wav", SearchOption.AllDirectories);
            List<SoundObject> sounds = [];

            foreach (string sound in soundsFiles)
            {
                string soundName = Path.GetFileNameWithoutExtension(sound);
                if (data.audioClips.TryGetValue(soundName, out string soundTypeIdentifier))
                {
                    SoundType type = SoundType.Effect;

                    if (soundTypeIdentifier.Contains("Sfx"))
                        type = SoundType.Effect;
                    else if (soundTypeIdentifier.Contains("Vfx"))
                        type = SoundType.Voice;
                    else if (soundTypeIdentifier.Contains("Mfx"))
                        type = SoundType.Music;

                    if (debug)
                        Debug.Log($"Sound: {soundName}, Type: {type}");

                    SoundObject soundObj = AssetsLoader.CreateSound(soundName, Paths.GetPath(PathsEnum.NPCs, npcName), soundTypeIdentifier, type, AssetsLoader.SetHexaColor(hexaSound), 1);
                    sounds.Add(soundObj);
                    AssetsLoader.assetMan.Add<SoundObject>(soundName, soundObj);
                }
            }
            NPCLoader.soundsObjects.Add(npcEnum, sounds.ToArray());
            return builder;
        }

        public static NPCBuilder<T> SetupAll<T>(this NPCBuilder<T> builder, string npcName, CustomNPCsEnum npcEnum, string hexaCode, string spriteSheetPrefix = "", bool debug = false, params RoomCategory[] categorys) where T : NPC
        {
            builder.SetMetaName(NPCLoaderExtenssion.LoadFile(npcName).pri_namekey);
            builder.SetName(NPCLoaderExtenssion.LoadFile(npcName).name);
            builder.SetEnum(npcEnum.ToString());
            builder.SetupPoster(npcName);
            builder.AddSpawnableRoomCategories(categorys);
            builder.SetupSprites(npcName, npcEnum, false, "", debug);
            builder.SetupSprites(npcName, npcEnum, true, spriteSheetPrefix, debug);
            builder.SetupSounds(npcName, npcEnum, hexaCode, debug);
            return builder;
        }

        public static NPC Build<T>(this NPCBuilder<T> builder, float height, Color audManColor, string startSprite) where T : NPC
        {
            builder.SetForcedSubtitleColor(audManColor);
            builder.AddTrigger();
            NPC npc = builder.Build();
            npc.Navigator.npc = npc;
            npc.Navigator.ec = npc.ec;
            npc.spriteRenderer[0].transform.localPosition = new Vector3(0, height, 0);
            npc.spriteRenderer[0].sprite =  AssetsLoader.assetMan.Get<Sprite>(startSprite);
            npc.spriteRenderer[0].ResizeCollider(npc.baseTrigger[0]);
            if (npc.GetComponent<INPCPrefab>() != null)
                npc.GetComponent<INPCPrefab>().Setup();

            return npc;
        }

        public static NPC MakeItWeightedNPC(this NPC npc, string[] floors, int[] weights)
        {
            int weight = 0;
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                {
                    floor.NPCs.Add(new WeightedNPC { selection = npc, weight = weights[weight] });
                    weight++;
                }
            }
            return npc;
        }

        public static NPC MakeItForcedNPC(this NPC npc, string[] floors)
        {
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                    floor.forcedNPCs.Add(npc);

            }
            return npc;
        }

    }
}

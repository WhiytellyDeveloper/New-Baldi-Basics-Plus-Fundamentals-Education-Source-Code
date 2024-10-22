using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using nbppfe.CustomContent.CustomEvents;
using nbppfe.CustomData;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.PrefabSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static nbppfe.FundamentalsManager.FundamentalLoaderManager;

namespace nbppfe.FundamentalsManager.Loaders
{
    public static class EventsLoader
    {
        public static void Setup()
        {
            CreateEndVoicelines();

            LoadEvent<RandomiclyTimeEvent>("RadomiclyTime", CustomRandomEventEnum.RadomiclyTime)
            .MakeItWeightedEvent(["F2", "F3", "F4", "END"], [28, 15, 40, 34]);

            LoadEvent<RaveEvent>("Rave", CustomRandomEventEnum.Rave)
            .MakeItWeightedEvent(["F2", "F3", "F4", "END"], [42, 35, 52, 73]);
        }

        public static RandomEvent LoadEvent<T>(string path, CustomRandomEventEnum eventEnum, Character npcRequired = Character.Null) where T : RandomEvent
        {
            string json = File.ReadAllText(Paths.GetPath(PathsEnum.Events, [path, "EventData.data"]));
            FileEventData eventData = JsonUtility.FromJson<FileEventData>(json);

            RandomEventBuilder<T> eventBuilder = new RandomEventBuilder<T>(BasePlugin.Instance.Info);

            string[] sounds = Directory.GetFiles(Path.Combine(AssetLoader.GetModPath(BasePlugin.Instance), "Events", path), "*.wav", SearchOption.AllDirectories);

            foreach (string file in sounds)
            {
                string newFile = Path.GetFileNameWithoutExtension(file);
                Debug.Log(newFile);
                if (newFile.Contains(eventData.voicelineName))
                    eventBuilder.SetSound(AssetsLoader.CreateSound(eventData.voicelineName, Paths.GetPath(PathsEnum.Events, path), eventData.voicelineEventFirstKey, SoundType.Effect, AssetsLoader.SetHexaColor("#0EE716"), 1, eventData.voicelinesKeys));
            }

            eventBuilder.SetName(eventData.name);
            eventBuilder.SetEnum(eventEnum.ToString());

            if (npcRequired != Character.Null)
                eventBuilder.AddRequiredCharacter(npcRequired);
            eventBuilder.SetMinMaxTime(eventData.minTime, eventData.maxTime);

            RandomEvent randomEvent = eventBuilder.Build();

            if (randomEvent.GetComponent<IEventPrefab>() != null)
                randomEvent.GetComponent<IEventPrefab>().Setup();

            return randomEvent;
        }

        public static RandomEvent MakeItWeightedEvent(this RandomEvent randomEvent, string[] floors, int[] weights)
        {
            int weight = 0;
            foreach (FloorData floor in FundamentalLoaderManager.floors)
            {
                if (floors.Contains(floor.Floor))
                {
                    floor.events.Add(randomEvent.ToWeighted<WeightedRandomEvent, RandomEvent>(weights[weight]));
                    weight++;
                }
            }
            return randomEvent;
        }

        public static void CreateEndVoicelines()
        {
            /*
            Sprite[] endExclamationEvent = TextureExtensions.LoadSpriteSheet(4, 2, 1, Paths.GetPath(PathsEnum.Events, ["EventOver", "BlueExclamationPoint_Sheet.png"]));

            for (int i = 0; i < endExclamationEvent.Length; i++)
            {
                if (i != 5 || i != 6)
                {
                    endExclamationEvent[i].name = $"BlueExclamationPoint_Sheet_{i}";
                    Debug.Log(endExclamationEvent[i].name);
                }
            }

            Later... lol
            */

            //Resources.FindObjectsOfTypeAll<Texture2D>().Where(x => x.name == "ExclamationPoint_Sheet").FirstOrDefault().MakeReadableTexture().OverlayTexture(AssetsLoader.CreateTexture("BlueExclamationPoint_Sheet", Paths.GetPath(PathsEnum.Events, "EventOver")));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_1", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent1(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent1(2)", time = 1.498f },
                  new() { key = "Vfx_BAL_EndEvent1(3)", time = 3.394f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_2", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent2(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent2(2)", time = 2.645f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_3", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent3(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent3(2)", time = 1.165f },
                  new() { key = "Vfx_BAL_EndEvent3(3)", time = 2.904f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_4", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent4(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent4(2)", time = 3.089f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_5", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent5(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent5(2)", time = 2.416f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_6", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent6(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [ 
                  new() { key = "Vfx_BAL_EndEvent6(2)", time = 2.174f },
                  new() { key = "Vfx_BAL_EndEvent6(3)", time = 6.663f }
            ]));

            eventEndVoicelines.Add(AssetsLoader.CreateSound("EventOver_7", Paths.GetPath(PathsEnum.Events, "EventOver"), "Vfx_BAL_EndEvent7(1)", SoundType.Voice, AssetsLoader.SetHexaColor("#0EE716"), 1, [
                  new() { key = "Vfx_BAL_EndEvent7(2)", time = 3.978f }
            ]));
        }



        public static List<SoundObject> eventEndVoicelines = [];
    }
}

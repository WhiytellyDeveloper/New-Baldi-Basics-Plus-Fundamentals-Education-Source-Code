using MTM101BaldAPI.Components;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace nbppfe.CustomContent.CustomEvents
{
    public class RaveEvent : RandomEvent, IEventPrefab
    {
        public void Setup()
        {
            blackout = AssetsLoader.CreateSound("RaveBlackout", Paths.GetPath(PathsEnum.Events, "Rave"), "", SoundType.Effect, Color.white, 1);
            midi = AssetsLoader.LoadMidi(Paths.GetPath(PathsEnum.Events, ["Rave", "RaveEvent.midi"]), "RaveEvent");
        }

        public override void Begin()
        {
            base.Begin();
            StartCoroutine(StartAnimation());
        }
        public IEnumerator StartAnimation()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(blackout);

            foreach (var cell in ec.AllCells())
            {
                cellColors.Add(cell, new KeyValuePair<Color, bool>(cell.lightColor, cell.hasLight));
                Singleton<CoreGameManager>.Instance.UpdateLighting(Color.black, cell.position);
            }
            yield return new WaitForSeconds(blackout.subDuration);

            Singleton<MusicManager>.Instance.PlayMidi(midi, true);

            foreach (NPC npc in ec.Npcs)
            {
                var baldi = npc.GetComponent<Baldi>();
                if (baldi)
                {
                    baldi.GetExtraAnger(28);
                    baldi.GetNPCContainer().AddLookerMod(blindMod);
                }
            }

            StartCoroutine(RaveLoop());
            yield break;
        }


        public IEnumerator RaveLoop()
        {
            foreach (var cell in ec.AllCells())
                Singleton<CoreGameManager>.Instance.UpdateLighting(Random.ColorHSV(), cell.position);
            yield return new WaitForSeconds(0.16f);
            StartCoroutine(RaveLoop());
        }

        public override void End()
        {
            base.End();
            StopAllCoroutines();
            foreach (NPC npc in ec.Npcs)
            {
                var baldi = npc.GetComponent<Baldi>();
                if (baldi)
                {
                    baldi.GetExtraAnger(-28);
                    baldi.GetNPCContainer().RemoveLookerMod(blindMod);
                }
            }
            Singleton<MusicManager>.Instance.StopMidi();
            foreach (var cell in ec.AllCells())
            {
                if (cellColors[cell].Value)
                    Singleton<CoreGameManager>.Instance.UpdateLighting(cellColors[cell].Key, cell.position);
                else
                    Singleton<CoreGameManager>.Instance.UpdateLighting(Color.white, cell.position);
            }
        }

        public ValueModifier blindMod = new(0f, 0f);
        public SoundObject blackout;
        protected readonly Dictionary<Cell, KeyValuePair<Color, bool>> cellColors = new Dictionary<Cell, KeyValuePair<Color, bool>>();
        public bool updateLight;
        public List<Cell> positions = new List<Cell>();
        public string midi;
        public float baldiDistance;
    }
}

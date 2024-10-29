using HarmonyLib;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Compass : NPCItem, IItemPrefab
    {
        public void Setup()
        {
            audMan = gameObject.CreateAudioManager(1000, 1000);
            silenceSound = AssetsLoader.CreateSound("CompassLocal", Paths.GetPath(PathsEnum.Items, "Compass"), "Sfx_CompassPoint", SoundType.Effect, Color.white, 1);
            trackingSound = FundamentalLoaderManager.GenericTrackerSound;
        }

        public override bool OnUse(PlayerManager pm, NPC npc)
        {
            this.npc = npc;
            audMan.transform.position = npc.transform.position;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(trackingSound);
            Singleton<SubtitleManager>.Instance.CreateSub(silenceSound, audMan, audMan.sourceId, audMan.audioDevice.maxDistance, true, Color.white);

            foreach (SubtitleController subtitle in FindObjectsOfType<SubtitleController>())
            {
                if (subtitle.text.text == Singleton<LocalizationManager>.Instance.GetLocalizedText(silenceSound.soundKey))
                {
                    subtitle.text.text = $"*{Singleton<LocalizationManager>.Instance.GetLocalizedText(npc.GetMeta().nameLocalizationKey)}* {Singleton<LocalizationManager>.Instance.GetLocalizedText(silenceSound.soundKey)}";

                    if (npc.GetComponent<AudioManager>())
                    {
                        if ((Color)npc.GetComponent<AudioManager>().ReflectionGetVariable("subtitleColor") != (Color)Character.Beans.ToNPC().GetComponent<AudioManager>().ReflectionGetVariable("subtitleColor") && npc.Character != Character.Beans.ToNPC().Character)
                            subtitle.text.color = (Color)npc.GetComponent<AudioManager>().ReflectionGetVariable("subtitleColor");
                    }
                }
            }
            return true;
        }

        private void Update()
        {
            audMan.transform.position = npc.transform.position;
            cooldown.UpdateCooldown(npc.ec.EnvironmentTimeScale);
        }

        public void OnEndCooldown() =>
            Destroy(gameObject);

        public SoundObject silenceSound, trackingSound;
        public AudioManager audMan;
        public NPC npc;
        public Cooldown cooldown = new Cooldown(30, 0);
    }
}

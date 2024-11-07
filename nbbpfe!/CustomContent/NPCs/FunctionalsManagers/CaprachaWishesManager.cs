using nbppfe.Enums;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs.FunctionalsManagers
{
    public class CaprachaWishesManager : MonoBehaviour
    {
        public void Initialize(Capracha capracha)
        {
            this.capracha = capracha;

            wishesTexts.Add(CaprachaWishes.MultipliesPlayerSpeed, "Men_CaprachaWish_PlayerSpeed");
            wishesTexts.Add(CaprachaWishes.SlowdownNPCsSpeed, "Men_CaprachaWish_NPCsSlowdown");
            wishesTexts.Add(CaprachaWishes.Earn75Points, "Men_CaprachaWish_EarnPoints");
            wishesTexts.Add(CaprachaWishes.DontMakeSound, "Men_CaprachaWish_Silence");
            wishesTexts.Add(CaprachaWishes.GoodItem, "Men_CaprachaWish_GoodItem");
            wishesTexts.Add(CaprachaWishes.FacultyNametag, "Men_CaprachaWish_FacultyNametag");
            wishesTexts.Add(CaprachaWishes.InfiniteStamina, "Men_CaprachaWish_IniniteStamina");
            wishesTexts.Add(CaprachaWishes.BlindEveryone, "Men_CaprachaWish_BlindNPCs");
        }

        private void Update() 
        {
            UpdateTexts();

            if (Singleton<InputManager>.Instance.GetDigitalInput("MapZoomPos", true)) ChooseOption(0);
            else if (Singleton<InputManager>.Instance.GetDigitalInput("MapZoomNeg", true)) ChooseOption(1);
            else if (Singleton<InputManager>.Instance.GetDigitalInput("LookBack", true)) Exit(0);

        }

        public void ChooseOption(int option)
        {
            if (screenActive && !inEffect)
            {
                foreach (var text in texts)
                    text.color = Color.white;

                texts[option].color = Color.green;
                inEffect = true;
                CloseScreen(0);
            }
        }

        public void OpenScreen(int player)
        {
            renderer.gameObject.SetActive(true);
            foreach (var text in texts)
                text.color = Color.white;
            ChooseRandomWish();
            UpdateTexts();
            screenActive = true;
            Singleton<CoreGameManager>.Instance.disablePause = true;
            Singleton<GlobalCam>.Instance.FadeIn(UiTransition.Dither, 0.00001f);
            Singleton<BaseGameManager>.Instance.Ec.PauseEnvironment(true);
            Singleton<BaseGameManager>.Instance.Ec.PauseEvents(true);
            Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.Enable(false);
            Singleton<CoreGameManager>.Instance.GetPlayer(player).DisableClick(true);
        }

        public void CloseScreen(int player)
        {
            Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.00001f);
            renderer.gameObject.SetActive(false);
            UpdateTexts();
            screenActive = false;
            Singleton<CoreGameManager>.Instance.disablePause = false;
            Singleton<BaseGameManager>.Instance.Ec.PauseEnvironment(false);
            Singleton<BaseGameManager>.Instance.Ec.PauseEvents(false);
            Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.Enable(true);
            Singleton<CoreGameManager>.Instance.GetPlayer(player).DisableClick(false);
        }

        public void Exit(int player)
        {
            texts[2].color = Color.green;
            CloseScreen(player);
        }

        public void ChooseRandomWish()
        {
            List<CaprachaWishes> wishesList = wishesTexts.Keys.ToList();
            wishesList.Shuffle();
            wishes[0] = wishesList[0];
            wishes[1] = wishesList[1];
        }

        private void UpdateTexts()
        {

            if (wishes[0] != CaprachaWishes.Null && wishes[1] != CaprachaWishes.Null)
            {
                texts[0].text = $"({Singleton<InputManager>.Instance.GetInputButtonName("MapZoomPos")}) {Singleton<LocalizationManager>.Instance.GetLocalizedText(wishesTexts[wishes[0]])}";
                texts[1].text = $"({Singleton<InputManager>.Instance.GetInputButtonName("MapZoomNeg")}) {Singleton<LocalizationManager>.Instance.GetLocalizedText(wishesTexts[wishes[1]])}";
                texts[2].text = $"({Singleton<InputManager>.Instance.GetInputButtonName("LookBack")}) {Singleton<LocalizationManager>.Instance.GetLocalizedText("Men_CaprachaWish_Exit")}";
            }
        }

        public Capracha capracha;
        public Transform renderer;
        public Dictionary<CaprachaWishes, string> wishesTexts = [];
        public CaprachaWishes[] wishes = [CaprachaWishes.Null, CaprachaWishes.Null];
        public TextMeshProUGUI[] texts = [null, null, null];
        public bool screenActive;
        public bool inEffect;
    }
}

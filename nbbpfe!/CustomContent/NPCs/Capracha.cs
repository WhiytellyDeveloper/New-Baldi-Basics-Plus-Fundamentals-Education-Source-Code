using nbppfe.Enums;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using MTM101BaldAPI.UI;
using UnityEngine;
using TMPro;
using nbppfe.CustomContent.NPCs.FunctionalsManagers;
using PixelInternalAPI.Classes;

namespace nbppfe.CustomContent.NPCs
{
    public class Capracha : NPC, INPCPrefab, IClickable<int>
    {
        public void Setup()
        {
            canvas = nbppfe.Extensions.UIExtensions.CreateCanvas(true);
            manager = canvas.gameObject.AddComponent<CaprachaWishesManager>();
            var renderesParent = new GameObject("Rendrers");
            renderesParent.transform.SetParent(canvas.transform);
            manager.renderer = renderesParent.transform;
            var renderer = ObjectCreationExtensions.CreateImage(canvas, TextureExtensions.CreateSolidTexture(480, 360, new(0, 0, 0, 0.8f)), true);
            renderer.transform.SetParent(renderesParent.transform);

            Vector3 textPos1 = new(0, 160, -9.69f);
            Vector3 textPos2 = new(0, 30, -9.69f);
            Vector3 textPos3 = new(0, -10, -9.69f);
            Vector3 textPos4 = new(0, -50, -9.69f);

            var text1 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans36, "Choose a wish!", renderesParent.transform, textPos1, false);
            text1.alignment = TextAlignmentOptions.Center;
            text1.GetComponent<RectTransform>().sizeDelta = new Vector2(480f, 50f);

            var text2 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "Placeholder With Text 1", renderesParent.transform, textPos2, false);
            text2.alignment = TextAlignmentOptions.Center;
            text2.GetComponent<RectTransform>().sizeDelta = new Vector2(900f, 50f);
            manager.texts[0] = text2;

            var text3 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "Placeholder With Text 2", renderesParent.transform, textPos3, false);
            text3.alignment = TextAlignmentOptions.Center;
            text3.GetComponent<RectTransform>().sizeDelta = new Vector2(900f, 50f);
            manager.texts[1] = text3;

            var text4 = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "Placeholder With Text 3", renderesParent.transform, textPos4, false);
            text4.alignment = TextAlignmentOptions.Center;
            text4.GetComponent<RectTransform>().sizeDelta = new Vector2(900f, 50f);
            manager.texts[2] = text4;

            renderesParent.gameObject.SetActive(false);
            canvas.transform.SetParent(transform);

            gameObject.layer = LayerStorage.iClickableLayer;
        }

        public void PostLoading() 
        {
            manager = canvas.GetComponent<CaprachaWishesManager>();
            manager.Initialize(this);
        }

//-----------------------------------------------------------------------------------------------------------------------------------------------------------|

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new Capracha_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();

            if (Input.GetKeyDown(KeyCode.P))
            {
                manager.OpenScreen(0);
                manager.inEffect = false;
            }
            
        }

        public void Clicked(int player) {
            manager.OpenScreen(0);
            manager.inEffect = false;
        }

        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return false; }
        public bool ClickableRequiresNormalHeight() { return true; }

        public CaprachaWishes choosedWish;
        public Canvas canvas;
        private CaprachaWishesManager manager;

    }

    public class Capracha_StateBase : NpcState {
        protected Capracha cpr;
        public Capracha_StateBase(Capracha caparacha) : base(caparacha) { npc = caparacha; this.cpr = caparacha; }
    }

    public class Capracha_Wandering : Capracha_StateBase
    {
        public Capracha_Wandering(Capracha caparacha) : base(caparacha) { }

        public override void Initialize()
        {
            base.Initialize();
            npc.Navigator.SetSpeed(15);
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }
    }
}

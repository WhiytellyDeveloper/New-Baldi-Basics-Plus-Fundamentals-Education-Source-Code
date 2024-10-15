using MTM101BaldAPI;
using PixelInternalAPI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.BasicClasses.CustomObjects
{
    public class TipBook : MonoBehaviour, IClickable<int>
    {
        private void Start()
        {
        }

        public void Clicked(int player)
        {
        }
        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return bookOpen; }
        public bool ClickableRequiresNormalHeight() { return true; }

        public bool bookOpen;
        public Canvas canvas;
        public Image bookImage;
        public Sprite closed, open;
    }
}

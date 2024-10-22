using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;
using PixelInternalAPI.Extensions;
using TMPro;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_BoxPortal : Item, IItemPrefab, IClickable<int>
    {
        public void Setup()
        {
            portal = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("NewPortal", Paths.GetPath(PathsEnum.Items, "BoxPortal"), 13), false).AddSpriteHolder(-5 + 0.01f, LayerStorage.iClickableLayer);
            var holder = portal.transform.parent;
            gameObject.AddBoxCollider(Vector3.zero, new Vector3(8, 10, 8), true);
            holder.gameObject.ConvertToPrefab(true);
            holder.transform.SetParent(transform);
            portal.transform.SetParent(holder.transform);
            portal.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            text = new GameObject("Text", typeof(BillboardRotator), typeof(PickupBob)).AddComponent<TextMeshPro>();
            text.text = "60";
            text.alignment = TextAlignmentOptions.Center;
            text.transform.SetParent(transform);
            text.transform.localPosition = Vector3.zero;
            text.gameObject.layer = LayerStorage.billboardLayer;

            teleport = AssetsLoader.CreateSound("PortalTeleport", Paths.GetPath(PathsEnum.Items, "BoxPortal"), "", SoundType.Effect, Color.white, 1);
            placePortal = AssetsLoader.CreateSound("PlacePortal", Paths.GetPath(PathsEnum.Items, "BoxPortal"), "", SoundType.Effect, Color.white, 1);
            clockSound = (SoundObject)Items.AlarmClock.ToItem().item.GetComponent<ITM_AlarmClock>().ReflectionGetVariable("audWind");
        }

        //----------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(placePortal);
            time.endAction = OnEnd;
            time.cooldown = timeSteps[0];
            Vector3 centilizedPos = pm.ec.CellFromPosition(pm.transform.position).FloorWorldPosition;
            transform.position = new Vector3(centilizedPos.x, 5, centilizedPos.z);
            return true;
        }

        private void Update()
        {
            time.UpdateCooldown(pm.ec.EnvironmentTimeScale);
            text.text = ((int)time.cooldown).ToString();
            float timer = Mathf.Clamp(time.cooldown, timeSteps[choosedTimeId], 60);
            float speedFactor = timeSteps[choosedTimeId] / timer;
            float currentRotationSpeed = Mathf.Lerp(rotationSpeed, 200, speedFactor);
            portal.transform.parent.Rotate(0, currentRotationSpeed * Time.deltaTime, 0);
        }

        public void OnEnd()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(teleport);
            pm.plm.Entity.Teleport(transform.position);
            Destroy(gameObject);
        }

        public void Clicked(int player)
        {
            choosedTimeId++;

            if (choosedTimeId >= timeSteps.Length)
                choosedTimeId = 0;

            time.cooldown = timeSteps[choosedTimeId] + 1;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(clockSound);

        }

        public void ClickableSighted(int player) { text.fontStyle = FontStyles.Underline; }
        public void ClickableUnsighted(int player) { text.fontStyle = FontStyles.Normal; }
        public bool ClickableHidden() { return false; }
        public bool ClickableRequiresNormalHeight() { return false; }

        public SpriteRenderer portal;
        public TextMeshPro text;
        public int choosedTimeId;
        public int[] timeSteps = [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 99];
        private Cooldown time = new Cooldown(60, 0);
        private float rotationSpeed = 1;
        public SoundObject teleport, clockSound, placePortal;
    }
}

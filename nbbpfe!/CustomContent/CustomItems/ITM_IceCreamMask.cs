using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_IceCreamMask : Item, IItemPrefab
    {
        public void Setup()
        {
            var canvas = ObjectCreationExtensions.CreateCanvas();
            canvas.name = "IceCreamMaskCanvas";
            canvas.transform.SetParent(transform);
            image = ObjectCreationExtensions.CreateImage(canvas, false);
            image.transform.localPosition = new(-285, -90);
            image.transform.localScale = new(0.6f, 0.6f);

            far = AssetsLoader.CreateSprite("IceCreamMask_IconLarge", Paths.GetPath(PathsEnum.Items, "IceCreamMask"), 1);
            close = AssetsLoader.CreateSprite("IceCreamMask_Close", Paths.GetPath(PathsEnum.Items, "IceCreamMask"), 1);
            veryClose = AssetsLoader.CreateSprite("IceCreamMask_VeryClose", Paths.GetPath(PathsEnum.Items, "IceCreamMask"), 1);
            notHere = AssetsLoader.CreateSprite("IceCreamMask_Sleeping", Paths.GetPath(PathsEnum.Items, "IceCreamMask"), 1);

            image.sprite = far;
            switchSound = AssetsLoader.CreateSound("switch", Paths.GetPath(PathsEnum.Items, "IceCreamMask"), "", SoundType.Effect, Color.white, 1);
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = RiseUp;
            if (pm.ec.GetBaldi() == null && Singleton<CoreGameManager>.Instance.currentMode != Mode.Free)
            {
                Destroy(gameObject);
                return false;
            }

            var canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).canvasCam;
            image = canvas.GetComponentInChildren<Image>();
            StartCoroutine(Anim());
            return true;
        }

        private void Update()
        {
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
            if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
            {
                StopAllCoroutines();
                StartCoroutine(Rise());
                image.sprite = notHere;
            }
            else
            {
                Transform baldi = pm.ec.GetBaldi().transform;
                float distanceToBaldi = Vector3.Distance(pm.transform.position, baldi.position);

                if (distanceToBaldi <= 100)
                    Reload(veryClose);
                else if (distanceToBaldi <= 230)
                    Reload(close);
                else if (distanceToBaldi <= 300)
                    Reload(far);
            }
        }

        private IEnumerator Anim()
        {
            while (true)
            {
                while (image.transform.localPosition.y < targetHeightMax)
                {
                    image.transform.localPosition += Vector3.up * riseSpeed * Time.deltaTime;
                    animHeight = image.transform.localPosition.y;
                    yield return null;
                }

                while (image.transform.localPosition.y > targetHeightMin)
                {
                    image.transform.localPosition -= Vector3.up * riseSpeed * Time.deltaTime;
                    animHeight = image.transform.localPosition.y;
                    yield return null;
                }

                image.transform.localPosition = new(image.transform.localPosition.x, animHeight, image.transform.localPosition.z);
            }
        }

        public void Reload(Sprite newSprite)
        {
            if (newSprite != image.sprite)
            {
                image.sprite = newSprite;

                if (newSprite != oldsprite)
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(switchSound);

                oldsprite = newSprite;
            }
        }


        private IEnumerator Rise()
        {
            while (image.transform.localPosition.y > riseTargetHeight)
            {
                image.transform.localPosition -= Vector3.up * 2 * riseSpeed * Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = new(image.transform.localPosition.x, riseTargetHeight, image.transform.localPosition.z);
            Destroy(gameObject);
        }

        public void RiseUp() =>
            StartCoroutine(Rise());

        public Image image;
        public Sprite far, close, veryClose, notHere, oldsprite;
        private float targetHeightMin = -90f, targetHeightMax = -85f, riseTargetHeight = -150f, animHeight, riseSpeed = 7f;
        public SoundObject switchSound;
        public Cooldown cooldown = new(48, 0);
    }
}

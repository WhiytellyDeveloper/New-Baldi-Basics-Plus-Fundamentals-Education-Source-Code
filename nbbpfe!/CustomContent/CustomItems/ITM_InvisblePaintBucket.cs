using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_InvisblePaintBucket : Item, IItemPrefab
    {
        public void Setup()
        {
            canvasPrefab = ObjectCreationExtensions.CreateCanvas();
            canvasPrefab.gameObject.ConvertToPrefab(true);
            image = ObjectCreationExtensions.CreateImage(canvasPrefab, false);
            sprites[0] = AssetsLoader.CreateSprite("PaintBucket_Drop", Paths.GetPath(PathsEnum.Items, "InvisiblePaintBucket"), 1);
            sprites[1] = AssetsLoader.CreateSprite("PaintBucket_Empty", Paths.GetPath(PathsEnum.Items, "InvisiblePaintBucket"), 1);
            sprites[2] = AssetsLoader.CreateSprite("PaintBucket_DropEnd", Paths.GetPath(PathsEnum.Items, "InvisiblePaintBucket"), 1);
            image.sprite = sprites[0];
            image.transform.localPosition = new Vector3(0, 420);
            image.transform.localScale = new Vector3(1.8f, 1.8f);

            splashSound = AssetsLoader.CreateSound("PaintSplash", Paths.GetPath(PathsEnum.Items, "InvisiblePaintBucket"), "Sfx_Splash", SoundType.Effect, Color.white, 1);
        }

        public override bool Use(PlayerManager pm)
        {
            Canvas canvas = Instantiate(canvasPrefab);
            canvas.name = "PaintBucketCanvas";
            canvas.transform.SetParent(transform);
            canvas.worldCamera = pm.GetPlayerCamera().canvasCam;
            image = canvas.GetComponentInChildren<Image>();

            this.pm = pm;
            cooldown.endAction = OnEndCooldown;

            StartCoroutine(BucketAnimation());
            return true;
        }

        private void Update() =>
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

        public void OnEndCooldown() =>
            StartCoroutine(OnCooldownEnd());

        public IEnumerator BucketAnimation()
        {
            while (image.transform.localPosition.y > -320)
            {
                image.transform.localPosition -= Vector3.up * 335 * Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = new Vector3(image.transform.localPosition.x, -320, image.transform.localPosition.z);
            Singleton<CoreGameManager>.Instance.GetHud(pm.playerNumber).ReflectionSetVariable("darkColor", new Color(1f, 1f, 1f, 0.7f));
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(splashSound);
            cooldown.Pause(false);
            pm.SetInvisible(true);
            image.sprite = sprites[1];
            while (image.transform.localPosition.y < 320)
            {
                image.transform.localPosition += Vector3.up * 635 * Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = new Vector3(image.transform.localPosition.x, 320, image.transform.localPosition.z);
        }

        public IEnumerator OnCooldownEnd()
        {
            image.sprite = sprites[2];
            while (image.transform.localPosition.y > -320)
            {
                image.transform.localPosition -= Vector3.up * 335 * Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = new Vector3(image.transform.localPosition.x, -320, image.transform.localPosition.z);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(splashSound);
            pm.SetInvisible(false);
            Destroy(gameObject);
        }

        public void OnDestroy() =>
            Singleton<CoreGameManager>.Instance.GetHud(pm.playerNumber).ReflectionSetVariable("darkColor", new Color(0.25f, 0.25f, 0.25f, 1f));
        

        public Cooldown cooldown = new(30, 0, null, null, true);
        public Canvas canvasPrefab;
        public Image image;
        public Sprite[] sprites = [null, null, null];
        public SoundObject splashSound;
    }
}

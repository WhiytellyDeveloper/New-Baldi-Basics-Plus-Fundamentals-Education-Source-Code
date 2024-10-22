using nbppfe.FundamentalsManager;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Homework : Item
    {
        private Image image;

        public void OnUse()
        {
            var canvasPrefab = ObjectCreationExtensions.CreateCanvas();
            canvasPrefab.transform.SetParent(transform);
            canvasPrefab.worldCamera = Camera.main;
            image = ObjectCreationExtensions.CreateImage(canvasPrefab, false);
            image.sprite = AssetsLoader.CreateSprite("HomeworkRenderer_A", Paths.GetPath(PathsEnum.Items, "HomeworkTierA"), 1);
            image.transform.localPosition = new Vector3(-350, 260);
            image.transform.localScale = new Vector3(1.5f, 1.5f);
            StartCoroutine(MoveImage());
        }

        private IEnumerator MoveImage()
        {
            Vector3 startPosition = new Vector3(-350, 260);
            Vector3 endPosition = new Vector3(350, -260);
            float duration = 2.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                image.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = endPosition;
            Destroy(gameObject);
        }
    }
}

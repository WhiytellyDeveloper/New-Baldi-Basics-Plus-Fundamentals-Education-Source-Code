using MTM101BaldAPI;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Tea : Item, DietItemVariation, IItemPrefab
    {
        public void Setup()
        {
            canvasPrefab = ObjectCreationExtensions.CreateCanvas();
            canvasPrefab.gameObject.ConvertToPrefab(true);
            image = ObjectCreationExtensions.CreateImage(canvasPrefab, false);
            image.sprite = diet ? AssetsLoader.CreateSprite("laaargeDietTea_overlay", Paths.GetPath(PathsEnum.Items, "DietTea"), 1) : AssetsLoader.CreateSprite("laaargeTea_overlay", Paths.GetPath(PathsEnum.Items, "Tea"), 1);
            image.transform.localPosition = new Vector3(0, -260);
            image.transform.localScale = new Vector3(2f, 2f);
        }

        public override bool Use(PlayerManager pm)
        {
            Canvas canvas = Instantiate(canvasPrefab);
            canvas.name = "TeaCanvas";
            canvas.transform.SetParent(transform);
            canvas.worldCamera = pm.GetPlayerCamera().canvasCam;
            image = canvas.GetComponentInChildren<Image>();

            if (diet)
                pm.plm.am.moveMods.Clear();        
            else
            {
                List<MovementModifier> modsToRemove = [];
                foreach (MovementModifier movMod in pm.plm.am.moveMods)
                {
                    if (movMod.movementMultiplier <= 1 && movMod != null)
                        modsToRemove.Add(movMod);
                }
                foreach (MovementModifier mod in modsToRemove)
                    pm.plm.am.moveMods.Remove(mod);
            }
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericDrinkingSound);
            StartCoroutine(Rise());
            return true;
        }

        public IEnumerator Rise()
        {
            while (image.transform.localPosition.y < 260)
            {
                image.transform.localPosition += Vector3.up * 200 * Time.deltaTime;
                yield return null;
            }
            image.transform.localPosition = new Vector3(image.transform.localPosition.x, 260, image.transform.localPosition.z);
            Destroy(gameObject);
        }

        public SoundObject drinking;
        public Canvas canvasPrefab;
        public Image image;

        public bool diet { get; set; }
    }
}

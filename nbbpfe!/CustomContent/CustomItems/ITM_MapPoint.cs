using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_MapPoint : Item, IItemPrefab
    {
        public void Setup()
        {
            mapIcon = Resources.FindObjectsOfTypeAll<NoLateIcon>().FirstOrDefault().SafeInstantiate();
            mapIcon.spriteRenderer.sprite = AssetsLoader.CreateSprite("Map_PinPoint", Paths.GetPath(PathsEnum.Items, "MapPoint"), 30);
            mapIcon.name = "MapPoint";
            mapIcon.timeText.text = "0";
            mapIcon.spriteRenderer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            mapIcon.gameObject.AddComponent<OwnerPoint>();
            Destroy(mapIcon.GetComponent<Animator>());
            mapIcon.gameObject.ConvertToPrefab(true);
            trackingSound = FundamentalLoaderManager.GenericTrackerSound;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(trackingSound);
            transform.position = pm.transform.position;
            pm.ec.map.AddIcon(mapIcon, transform, Color.white);
            return true;
        }

        private void Update()
        {
            var lsit = (List<MapIcon>)pm.ec.map.ReflectionGetVariable("icons");
            foreach(MapIcon icon in lsit.Where(x => x.name.Contains(mapIcon.name)))
            {
                if (icon.GetComponent<OwnerPoint>() != null)
                {
                    var selectedIcon = icon.GetComponent<OwnerPoint>();

                    if (selectedIcon.owner == Vector3.zero && !cathed)
                    {
                        selectedIcon.owner = transform.position;
                        cathed = true;
                    }

                    if (selectedIcon.owner == transform.position)
                        selectedIcon.GetComponent<NoLateIcon>().timeText.text = ((int)pm.transform.position.GetDistanceFrom(transform.position)).ToString();          
                }
            }
        }

        public NoLateIcon mapIcon;
        public bool cathed;
        public SoundObject trackingSound;
    }

    public class OwnerPoint : MonoBehaviour
    {
        public Vector3 owner = Vector3.zero;
    }
}

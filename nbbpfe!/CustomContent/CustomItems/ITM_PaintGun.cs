using MTM101BaldAPI;
using nbppfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_PaintGun : Item, IItemPrefab
    {
        public void Setup()
        {
            redPortal = AssetsLoader.CreateTexture("RedPortal", Paths.GetPath(PathsEnum.Items, "PaintGun"));
            bluePortal = AssetsLoader.CreateTexture("BluePortal", Paths.GetPath(PathsEnum.Items, "PaintGun"));

            audSplash = CustomItemsEnum.InvisiblePaintBucket.ToItem().item.GetComponent<ITM_InvisblePaintBucket>().splashSound;
            audTeleport = CustomItemsEnum.CommonTeleporter.ToItem().item.GetComponent<ITM_CommonTeleporter>().teleportSound;
            audShot = AssetsLoader.CreateSound("PaintGunShot", Paths.GetPath(PathsEnum.Items, "PaintGun"), "", SoundType.Effect, Color.white, 2);
        }

        public override bool Use(PlayerManager pm)
        {
            if (!Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach, pm.pc.ClickLayers, QueryTriggerInteraction.Ignore))
            {
                Destroy(gameObject);
                block = true;
            }

            if (hit.transform == null)
                Destroy(gameObject);

            if (!hit.transform.CompareTag("Wall"))
            {
                Destroy(gameObject);
                block = true;
            }

            Direction direction = Directions.DirFromVector3(hit.transform.forward, 5f);
            Cell cell = pm.ec.CellFromPosition(IntVector2.GetGridPosition(hit.transform.position + hit.transform.forward * -5f));

            if (pm.ec.ContainsCoordinates(IntVector2.GetGridPosition(hit.transform.position + hit.transform.forward * -5f)) && !cell.Null && !cell.WallHardCovered(direction))
            {
                foreach (var paintPortal in FindObjectsOfType<PaintPortal>())
                {
                    if (paintPortal.cell == cell)
                    {
                        Destroy(gameObject);
                        block = true;
                    }
                }

                if (!block)
                {
                    secondPortal = CheckForPaintGunManager().nextIsBlue;
                    CheckForPaintGunManager().nextIsBlue = !secondPortal;
                    portalSecond = CheckForPaintGunManager().nextPortal;

                    cell.PrepareForPoster();
                    cell.AddPoster(direction, secondPortal ? bluePortal : redPortal);
                    portal = hit.transform.gameObject.AddComponent<PaintPortal>();
                    portal.Initialize(audTeleport);
                    portal.cell = cell;

                    if (secondPortal)
                    {
                        portal.secondPortal = portalSecond;
                        portalSecond.secondPortal = portal;
                        Destroy(gameObject);
                    }

                    CheckForPaintGunManager().nextPortal = portal;
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audShot);
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audSplash);
                }
            }

            Destroy(gameObject);
            return secondPortal && !block;
        }

        public MiniPaintGunManager CheckForPaintGunManager()
        {
            var miniPaintGunManager = Singleton<CoreGameManager>.Instance.gameObject.GetOrAddComponent<MiniPaintGunManager>();
            return miniPaintGunManager;
        }

        private RaycastHit hit;
        public Texture2D redPortal, bluePortal;
        public PaintPortal portal, portalSecond;
        public bool secondPortal, block;
        public SoundObject audSplash, audTeleport, audShot;
    }

    public class PaintPortal : MonoBehaviour, IClickable<int>
    {
        public Cell cell;
        public PaintPortal secondPortal;
        public SoundObject telSound;

        public void Initialize(SoundObject sound) =>
            telSound = sound;     

        public void Clicked(int player)
        {
            if (secondPortal != null)
            {
                Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.Teleport(secondPortal.cell.CenterWorldPosition);
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(telSound);
            }
        }

        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return false; }
        public bool ClickableRequiresNormalHeight() { return true; }
    }

    public class MiniPaintGunManager : Singleton<MiniPaintGunManager>  {
        public bool nextIsBlue;
        public PaintPortal nextPortal;
    }
}

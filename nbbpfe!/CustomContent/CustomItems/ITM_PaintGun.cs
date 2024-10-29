using MTM101BaldAPI;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    //Not foinished
    public class ITM_PaintGun : Item, IItemPrefab
    {
        public void Setup()
        {
            redPortal = AssetsLoader.CreateTexture("RedPortal", Paths.GetPath(PathsEnum.Items, "PaintGun"));
            bluePortal = AssetsLoader.CreateTexture("BluePortal", Paths.GetPath(PathsEnum.Items, "PaintGun"));

            portalPrefab = new GameObject("PaintPortal").AddComponent<PaintPortal>();
            portalPrefab.gameObject.ConvertToPrefab(true);
            portalPrefab.entity = portalPrefab.gameObject.CreateEntity(0.8f, 0.8f);
        }

        public override bool Use(PlayerManager pm)
        {
            foreach (ITM_PaintGun paintGun in FindObjectsOfType<ITM_PaintGun>())
            {
                if (paintGun != this)
                {
                    secondPortal = true;
                    Destroy(gameObject);
                }
            }

            if (!Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach, pm.pc.ClickLayers, QueryTriggerInteraction.Ignore))
            {
                Destroy(base.gameObject);
                block = true;
            }
            if (!hit.transform.CompareTag("Wall"))
            {
                Destroy(base.gameObject);
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
                    cell.PrepareForPoster();
                    cell.AddPoster(direction, secondPortal ? bluePortal : redPortal);
                    var portal = Instantiate<PaintPortal>(portalPrefab);
                    portal.Initialize(pm.ec);
                    portal.transform.position = pm.ec.CellFromPosition(pm.transform.position).CenterWorldPosition;
                    portal.cell = cell;
                }
            }

            if (secondPortal)
                Destroy(gameObject);

            return secondPortal && !block;
        }

        private RaycastHit hit;
        public Texture2D redPortal, bluePortal;
        public PaintPortal portalPrefab;
        public bool secondPortal, block;
    }

    public class PaintPortal : MonoBehaviour, IEntityTrigger
    {
        public Cell cell = new();
        public PaintPortal secondPortal;
        public Entity entity;

        public void Initialize(EnvironmentController ec)
        {
            entity.Initialize(ec, transform.position);
        }

        public void EntityTriggerEnter(Collider other)
        {
            Debug.Log("PlayerEnter");
        }

        public void EntityTriggerStay(Collider other)
        {

        }

        public void EntityTriggerExit(Collider other)
        {
            Debug.Log("PlayerExit");
        }
    }
}

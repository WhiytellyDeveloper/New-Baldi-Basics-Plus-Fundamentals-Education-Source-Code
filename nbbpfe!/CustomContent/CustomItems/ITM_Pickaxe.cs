using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Pickaxe : Item, IItemPrefab
    {
        public void Setup()
        {
            audWallHit = AssetsLoader.CreateSound("WallHit", Paths.GetPath(PathsEnum.Items, "Pickaxe"), "", SoundType.Effect, Color.white, 1);
        }

        public override bool Use(PlayerManager pm)
        {
            if (!Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach, pm.pc.ClickLayers, QueryTriggerInteraction.Ignore))
            {
                Destroy(gameObject);
                return false;
            }
            if (!hit.transform.CompareTag("Wall"))
            {
                Destroy(gameObject);
                return false;
            }

            var tile = hit.transform.GetComponentInParent<Tile>();

            Direction direction = Directions.DirFromVector3(hit.transform.forward, 5f);
            var cell2 = pm.ec.CellFromPosition(tile.transform.position);

            try
            {
                if (pm.ec.ContainsCoordinates(cell2.position + direction.ToIntVector2()) && pm.ec.CellFromPosition(cell2.position + direction.ToIntVector2()).room.category == RoomCategory.Null)
                {
                    pm.ec.CreateCell(0, pm.ec.rooms[0].transform, cell2.position + direction.ToIntVector2(), cell2.room);
                    pm.ec.ConnectCells(cell2.position, direction);
                    Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audWallHit);

                    foreach (Cell cell in pm.ec.cells)
                        pm.ec.UpdateCell(cell.position);
                }
            }
            catch { }
            Destroy(gameObject);
            return true;
        }

        private RaycastHit hit;
        public SoundObject audWallHit;
    }
}

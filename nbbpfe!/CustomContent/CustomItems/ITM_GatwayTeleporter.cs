using UnityEngine;
using System.Collections.Generic;
using nbppfe.PrefabSystem;
using nbppfe.Extensions;
using MTM101BaldAPI.Reflection;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_GatwayTeleporter : Item, IItemPrefab
    {
        public void Setup() =>
            teleportSound = (SoundObject)Items.Teleporter.ToItem().item.GetComponent<ITM_Teleporter>().ReflectionGetVariable("audTeleport");
        

        public override bool Use(PlayerManager pm)
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(teleportSound);
            if (Singleton<BaseGameManager>.Instance.FoundNotebooks != Singleton<BaseGameManager>.Instance.NotebookTotal)
                return false;

            TeleportPlayer(pm);
            return true;
        }

        public void TeleportPlayer(PlayerManager player)
        {
            pm = player;
            List<IntVector2> list = new List<IntVector2>();
            foreach (Elevator elevator in pm.ec.elevators)
            {
                if (elevator.IsOpen)
                {
                    list.Add(elevator.Door.position);
                }
            }
            if (list.Count <= 0)
            {
                foreach (Elevator elevator2 in pm.ec.elevators)
                {
                    list.Add(elevator2.Door.position);
                }
            }
            bool flag = false;
            List<Cell> list2 = new List<Cell>();
            IntVector2 position = list[Random.Range(0, list.Count)];
            int num = 0;
            if (list.Count > 0)
            {
                while (!flag && num < 32)
                {
                    bool flag2;
                    pm.ec.FindPath(pm.ec.CellFromPosition(position), pm.ec.mainHall.TileAtIndex(Random.Range(0, pm.ec.mainHall.TileCount)), PathType.Nav, out list2, out flag2);
                    if (flag2 && list2.Count > 18)
                    {
                        flag = true;
                        list2 = new List<Cell>(list2);
                    }
                    num++;
                }
            }
            if (flag)
            {
                int distance = Random.Range(2, 5);
                player.Teleport(list2[distance].CenterWorldPosition);
                pm.ec.MakeNoise(list2[distance].CenterWorldPosition, 64);
            }
        }

        public SoundObject teleportSound;
    }
}

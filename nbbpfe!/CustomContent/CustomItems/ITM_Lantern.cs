using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalSystems;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Lantern : Item
    {
        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;
            foreach (var cell in pm.ec.AllCells())
                cellColors.Add(cell, new KeyValuePair<Color, bool>(cell.lightColor, cell.hasLight));        

            foreach (var cell in pm.ec.AllCells())
                Singleton<CoreGameManager>.Instance.UpdateLighting(Color.white, cell.position);

            return true;
        }

        private void Update()
        {
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        }

        public void OnCooldownEnd()
        {
            foreach (var cell in pm.ec.AllCells())
            {
                if (cellColors[cell].Value)
                    Singleton<CoreGameManager>.Instance.UpdateLighting(cellColors[cell].Key, cell.position);
                else
                    Singleton<CoreGameManager>.Instance.UpdateLighting(Color.white, cell.position);
            }
        }

        public Cooldown cooldown = new Cooldown(2, 0);
        protected readonly Dictionary<Cell, KeyValuePair<Color, bool>> cellColors = [];
    }
}

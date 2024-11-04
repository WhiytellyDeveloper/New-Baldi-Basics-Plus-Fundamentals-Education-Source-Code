using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_LaserPointer : Item
    {
        public override bool Use(PlayerManager pm)
        {
            if (!Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach * 10, pm.pc.ClickLayers, QueryTriggerInteraction.Ignore))
            {
                Destroy(gameObject);
                return false;
            }
            if (!hit.transform.CompareTag("Wall"))
            {
                Destroy(gameObject);
                return false;
            }

            pm.Teleport(hit.transform.position);

            return true;
        }


        private RaycastHit hit;
    }
}

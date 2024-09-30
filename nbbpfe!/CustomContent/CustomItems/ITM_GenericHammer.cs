using MTM101BaldAPI.Reflection;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_GenericHammer : Item
    {
        public override bool Use(PlayerManager pm)
        {
            Destroy(gameObject);
            if (Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out hit, pm.pc.reach, LayerMask.GetMask("Default", "Windows")))
            {
                Window component = hit.transform.GetComponent<Window>();
                if (component != null && !(bool)component.ReflectionGetVariable("broken"))
                {
                    component.Break(true);

                    if (!(bool)component.ReflectionGetVariable("broken"))
                        return false;

                    return true;
                }
            }
            return false;
        }

        private RaycastHit hit;
    }
}

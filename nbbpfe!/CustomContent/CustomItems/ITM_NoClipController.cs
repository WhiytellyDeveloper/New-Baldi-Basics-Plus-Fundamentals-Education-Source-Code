using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_NoClipController : Item, IItemPrefab
    {
        public void Setup()
        {
            beep = AssetsLoader.CreateSound("gps_beep", Paths.GetPath(PathsEnum.Items, "NoClipController"), "Sfx_Beep", SoundType.Effect, Color.white, 1);
            end = AssetsLoader.CreateSound("gps_end", Paths.GetPath(PathsEnum.Items, "NoClipController"), "Sfx_Ops", SoundType.Effect, Color.white, 1);
            error = Resources.FindObjectsOfTypeAll<SoundObject>().Where(x => x.name == "Activity_Incorrect").First();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            ITM_NoClipController[] controllers = FindObjectsOfType<ITM_NoClipController>();
            if (controllers.Length != 1)
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(error);
                Destroy(gameObject);
                return false;
            }

            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(beep);
            layerName = LayerMask.LayerToName(pm.gameObject.layer);
            startPoint = pm.transform.position;
            cooldown.endAction = OnCooldownEnd;
            return true;
        }

        private void LateUpdate()
        {
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

            if (layerName != "None" && !cooldown.cooldownIsEnd)
                pm.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void OnCooldownEnd()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(end);
            pm.plm.Entity.Teleport(startPoint);
            pm.gameObject.layer = LayerMask.NameToLayer(layerName);
            Destroy(gameObject);
        }

        public Vector3 startPoint;
        public Cooldown cooldown = new Cooldown(35, 0);
        public string layerName = "None";
        public SoundObject beep, end, error;
    }
}

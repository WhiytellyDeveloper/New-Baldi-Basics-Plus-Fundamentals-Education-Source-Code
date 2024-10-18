using MTM101BaldAPI.Reflection;
using nbbpfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Umbrella : Item, IItemPrefab
    {   
        public void Setup() {
            umbrellaOpen = AssetsLoader.CreateSound("UmbrellaOpen", Paths.GetPath(PathsEnum.Items, "Umbrella"), "Sfx_UmbrellaOpen", SoundType.Effect, Color.white, 1);
            playerGroundHit = AssetsLoader.CreateSound("UmbrellaImpact", Paths.GetPath(PathsEnum.Items, "Umbrella"), "Sfx_UmbrellaClose", SoundType.Effect, Color.white, 1);
        }

//------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            pm.SetInvisible(true);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(umbrellaOpen);
            pm.plm.Entity.SetTrigger(false);
            pm.plm.Entity.SetGrounded(false);
            height = pm.plm.Entity.BaseHeight;

            return true;
        }

        private void Update()
        {
            foreach (Collider npc in FindObjectsOfType<Collider>())
            {
                if (npc.isTrigger)
                    Physics.IgnoreCollision((Collider)pm.plm.Entity.ReflectionGetVariable("trigger"), npc, true);
            }

            if (up)
            {
                height += ascentSpeed * Time.deltaTime;

                if (height >= maxHeight)
                {
                    height = maxHeight;
                    up = false;
                }
            }
            else
            {
                height -= descentSpeed * Time.deltaTime;

                if (height <= targetHeight)
                {
                    height = targetHeight;
                    pm.plm.Entity.SetTrigger(true);
                    pm.plm.Entity.SetGrounded(true);
                    pm.SetInvisible(false);

                    if (pm.plm.Entity.BaseHeight == height)
                        Singleton<CoreGameManager>.Instance.audMan.PlaySingle(playerGroundHit);
                    foreach (Collider npc in FindObjectsOfType<Collider>())
                    {
                        if (npc.isTrigger)
                            Physics.IgnoreCollision((Collider)pm.plm.Entity.ReflectionGetVariable("trigger"), npc, false);
                    }
                    Destroy(gameObject);
                }
            }

            pm.plm.Entity.SetHeight(height);
        }

        public float height = 5f;
        public float ascentSpeed = 34.5f;
        public float descentSpeed = 1.7f;
        public float targetHeight = 5f;
        public float maxHeight = 25f;
        public bool up = true;
        public SoundObject umbrellaOpen, playerGroundHit;
    }
}

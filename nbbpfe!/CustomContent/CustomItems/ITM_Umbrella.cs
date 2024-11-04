using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Umbrella : Item, IItemPrefab
    {
        public void Setup()
        {
            umbrellaOpen = AssetsLoader.CreateSound("UmbrellaOpen", Paths.GetPath(PathsEnum.Items, "Umbrella"), "Sfx_UmbrellaOpen", SoundType.Effect, Color.white, 1);
            playerGroundHit = AssetsLoader.CreateSound("UmbrellaImpact", Paths.GetPath(PathsEnum.Items, "Umbrella"), "Sfx_UmbrellaClose", SoundType.Effect, Color.white, 1);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            ITM_JumpBoots[] boots = FindObjectsOfType<ITM_JumpBoots>();
            if (boots.Length != 0)
            {
                Destroy(gameObject);
                return false;
            }

            this.pm = pm;
            pm.SetInvisible(true);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(umbrellaOpen);
            pm.plm.Entity.SetTrigger(false);
            pm.plm.Entity.SetGrounded(false);
            pm.itm.Disable(true);
            height = pm.plm.Entity.BaseHeight;


            foreach (Collider npc in FindObjectsOfType<Collider>())
            {
                if (npc.isTrigger)
                    Physics.IgnoreCollision((Collider)pm.plm.Entity.ReflectionGetVariable("trigger"), npc, true);
            }

            return true;
        }

        private void Update()
        {
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
                    pm.itm.Disable(false);

                    if (pm.plm.Entity.BaseHeight == height)
                        Singleton<CoreGameManager>.Instance.audMan.PlaySingle(playerGroundHit);                  
                    Destroy(gameObject);
                }
            }

            pm.plm.Entity.SetHeight(height);
        }

        public void OnDestroy()
        {
            foreach (Collider npc in FindObjectsOfType<Collider>())
            {
                if (npc.isTrigger)
                    Physics.IgnoreCollision((Collider)pm.plm.Entity.ReflectionGetVariable("trigger"), npc, false);
            }
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

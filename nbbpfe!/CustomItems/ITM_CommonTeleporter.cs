using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbppfe.PrefabSystem;

namespace nbppfe.CustomItems
{
    public class ITM_CommonTeleporter : Item, IItemPrefab
    {
        public void Setup() =>      
            teleportSound = (SoundObject)ItemMetaStorage.Instance.FindByEnum(Items.Teleporter).value.item.GetComponent<ITM_Teleporter>().ReflectionGetVariable("audTeleport");
        
 //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            pm.plm.Entity.Teleport(pm.ec.RandomCell(true, false, true).FloorWorldPosition);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(teleportSound);
            pm.ec.MakeNoise(pm.transform.position, 100);
            Destroy(base.gameObject);
            return true;
        }

        public SoundObject teleportSound;
    }
}

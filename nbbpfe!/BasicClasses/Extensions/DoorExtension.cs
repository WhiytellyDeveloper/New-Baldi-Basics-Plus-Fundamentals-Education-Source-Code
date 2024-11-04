using nbppfe.Enums;
using nbppfe.Extensions;
using UnityEngine;

namespace nbppfe.BasicClasses.Extensions
{
    public class DoorExtension : MonoBehaviour, IItemAcceptor
    {
        private void Awake() =>
            door = GetComponent<StandardDoor>();

        public void InsertItem(PlayerManager pm, EnvironmentController ec)
        {           
            door.LockTimed(30);
        }

        public bool ItemFits(Items item)
        {
            return CustomItemsEnum.BlueLocker.ToItemEnum() == item && !door.locked;
        }

        public StandardDoor door;
    }
}

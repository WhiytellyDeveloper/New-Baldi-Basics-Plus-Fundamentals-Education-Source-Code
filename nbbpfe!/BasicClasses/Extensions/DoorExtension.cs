using UnityEngine;

namespace nbppfe.BasicClasses.Extensions
{
    public class DoorExtension : MonoBehaviour, IEntityTrigger
    {
        private void Awake() =>
            door = GetComponent<StandardDoor>();

        public void EntityTriggerEnter(Collider other)
        {
            door.OpenTimed(1, false);

            if (door.locked)
                door.Lock(false);
        }

        public void EntityTriggerStay(Collider other)
        {

        }

        public void EntityTriggerExit(Collider other)
        {

        }

        public StandardDoor door;
    }
}

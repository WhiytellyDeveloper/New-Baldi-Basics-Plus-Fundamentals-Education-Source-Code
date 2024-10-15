using UnityEngine;

namespace nbppfe.FundamentalSystems
{
    public class SlipArea : MonoBehaviour, IEntityTrigger
    {
        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") || other.CompareTag("Player") && enabled && !touched)
            {
                if (audMan != null && onEnterSound != null)
                    audMan.PlaySingle(onEnterSound);
                Debug.Log("Slip");
                touched = true;
                other.GetComponent<Entity>().Teleport(other.GetComponent<Entity>().CurrentRoom.ec.CellFromPosition(other.transform.position).FloorWorldPosition);
                other.GetComponent<Entity>().AddForce(force);
            }
        }

        public void EntityTriggerStay(Collider other)
        {

        }

        public void EntityTriggerExit(Collider other)
        {
        }

        public Force force;
        public AudioManager audMan;
        public SoundObject onEnterSound;
        public bool touched;
    }
}

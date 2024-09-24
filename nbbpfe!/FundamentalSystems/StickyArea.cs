using UnityEngine;

namespace nbppfe.FundamentalSystems
{
    public class StickyArea : MonoBehaviour, IEntityTrigger
    {
        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") || other.CompareTag("Player") && enabled)
            {
                if (audMan != null || onEnterSound != null)
                    audMan.PlaySingle(onEnterSound);
                other.GetComponent<ActivityModifier>().moveMods.Add(moveMod);
            }
        }

        public void EntityTriggerStay(Collider other)
        {

        }

        public void EntityTriggerExit(Collider other)
        {
            if (other.CompareTag("NPC") || other.CompareTag("Player") && enabled)
                other.GetComponent<ActivityModifier>().moveMods.Remove(moveMod); 
        }

        public MovementModifier moveMod;
        public AudioManager audMan;
        public SoundObject onEnterSound;
    }
}

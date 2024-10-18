using System;
using UnityEngine;

namespace nbppfe.FundamentalSystems
{
    public class ForceArea : MonoBehaviour, IEntityTrigger
    {
        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") && other.GetComponent<Entity>().Grounded && enabled || other.CompareTag("Player") && other.GetComponent<Entity>().Grounded && enabled)
            {
                col = other;
                preEnter.Invoke();
                if (audMan != null && onEnterSound != null)
                    audMan.PlaySingle(onEnterSound);
                other.GetComponent<Entity>().AddForce(force);
                someOneEnter = true;
                if (audMan == null)
                    Destroy(gameObject);
            }
        }

        public void EntityTriggerStay(Collider other)
        {

        }

        public void EntityTriggerExit(Collider other)
        {
        }

        private void Update()
        {
            if (audMan != null && destroyAfterUse && someOneEnter)
            {
                if (!audMan.AnyAudioIsPlaying)
                    Destroy(gameObject);
            }
        }

        public Force force;
        public AudioManager audMan;
        public SoundObject onEnterSound;
        private bool someOneEnter;
        public bool destroyAfterUse = true;
        public Action preEnter;
        public Collider col; 
    }
}

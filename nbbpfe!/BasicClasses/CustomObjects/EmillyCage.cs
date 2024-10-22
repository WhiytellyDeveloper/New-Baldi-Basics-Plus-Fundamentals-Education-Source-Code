using nbppfe.CustomContent.NPCs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.BasicClasses.CustomObjects
{
    public class EmillyCage : MonoBehaviour
    {

        public void SetOwner(EmillyGutter newOwner) =>
            owner = newOwner;

        public void Open() =>
            StartCoroutine(AnimateBars(Vector3.one, new Vector3(0, 7.5f, 0), 0.05f, open));

        public void Close() =>
            StartCoroutine(AnimateBars(new Vector3(1, 10, 1), new Vector3(0, 3f, 0), 0.05f, close));

        private IEnumerator AnimateBars(Vector3 targetScale, Vector3 targetPositionOffset, float duration, SoundObject sound)
        {
            audMan.PlaySingle(sound);
            foreach (MeshRenderer bar in bars)
            {
                if (bar.name == "Bar")
                {
                    Vector3 initialScale = bar.transform.localScale;
                    Vector3 initialPosition = bar.transform.localPosition;

                    float time = 0f;

                    while (time < duration)
                    {
                        bar.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
                        bar.transform.localPosition = Vector3.Lerp(initialPosition, new Vector3(bar.transform.localPosition.x, targetPositionOffset.y, bar.transform.localPosition.z), time / duration);

                        time += Time.deltaTime;
                        yield return null;
                    }

                    bar.transform.localScale = targetScale;
                    bar.transform.localPosition = new Vector3(bar.transform.localPosition.x, targetPositionOffset.y, bar.transform.localPosition.z);
                }
            }
        }

        public AudioManager audMan;
        public SoundObject open, close;
        public List<MeshRenderer> bars = [];
        public EmillyGutter owner;
    }
}

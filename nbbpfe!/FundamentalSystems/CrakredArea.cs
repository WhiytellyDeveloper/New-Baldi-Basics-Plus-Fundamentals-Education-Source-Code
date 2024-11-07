using nbppfe.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace nbppfe.FundamentalSystems
{
    public class CrakredArea : MonoBehaviour
    {
        public void Initialize(EnvironmentController ec, float time, float timeScale)
        {
            this.ec = ec;
            cooldown.cooldown = time;
            this.timeScale = timeScale;
            cooldown.endAction = Destroy;
            me = this;
            ec.BlockAllDirs(IntVector2.GetGridPosition(transform.position), true);
        }

        private void Update() =>
            cooldown.UpdateCooldown(timeScale);      

        private void Destroy() =>
            StartCoroutine(InDestroy());

        public IEnumerator InDestroy()
        {
            renderer.sprite = endSprite;
            yield return new WaitForSeconds(0.3f);
            ec.BlockAllDirs(IntVector2.GetGridPosition(transform.position), false);
            Destroy(gameObject);
        }

        public EnvironmentController ec;
        public SpriteRenderer renderer;
        public Sprite endSprite;
        public Cooldown cooldown = new(5, 0);
        public float timeScale;
        public CrakredArea me;
    }
}

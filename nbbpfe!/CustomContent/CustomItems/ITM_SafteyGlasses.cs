using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_SafteyGlasses : Item, IItemPrefab
    {
        public void Setup()
        {
            canvas = ObjectCreationExtensions.CreateCanvas();
            var renderer = ObjectCreationExtensions.CreateImage(canvas, TextureExtensions.CreateSolidTexture(480, 360, Color.white), true);
            renderer.color = new Color(1, 1, 1, 0.5f);
            canvas.transform.SetParent(transform);
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = Destroy;
            canvas.worldCamera = pm.GetPlayerCamera().canvasCam;
            return true;
        }

        private void Update()
        {
            foreach (Fog fog in (List<Fog>)pm.ec.ReflectionGetVariable("fogs"))
                pm.ec.RemoveFog(fog);

            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);
        }

        public void Destroy() =>
            Destroy(gameObject);

        public Cooldown cooldown = new Cooldown(25, 0);
        public Canvas canvas;
    }
}

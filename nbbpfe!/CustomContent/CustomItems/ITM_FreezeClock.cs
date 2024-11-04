using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    internal class ITM_FreezeClock : Item, IItemPrefab, IClickable<int>
    {
        public void Setup()
        {
            //-1,2
            clockSprites = TextureExtensions.LoadSpriteSheet(4, 1, 15, Paths.GetPath(PathsEnum.Items, ["FreezeClock", "IceClockSpriteSheet.png"]));

            var holder = ObjectCreationExtensions.CreateSpriteBillboard(clockSprites[0]).AddSpriteHolder(out var render, -1.2f);
            renderer = holder.renderers[0].GetComponent<SpriteRenderer>();
            holder.transform.SetParent(transform);
            gameObject.layer = LayerStorage.iClickableLayer;

            entity = gameObject.CreateEntity(1, 1);
            audMan = gameObject.CreatePropagatedAudioManager(30, 60);
            audWind = Items.AlarmClock.ToItem().item.GetComponent<ITM_AlarmClock>().ReflectionGetVariable("audWind") as SoundObject;
            audRing = Items.AlarmClock.ToItem().item.GetComponent<ITM_AlarmClock>().ReflectionGetVariable("audRing") as SoundObject;
        }

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            cooldown.endAction = OnCooldownEnd;

            time.Add(clockSprites[0], 15);
            time.Add(clockSprites[1], 30);
            time.Add(clockSprites[2], 45);
            time.Add(clockSprites[3], 60);

            transform.position = pm.transform.position;
            entity.Initialize(pm.ec, transform.position);
            StartCoroutine(AnimateClock());
            return true;
        }

        public IEnumerator AnimateClock()
        {
            while (true)
            {
                foreach (var sprite in clockSprites)
                {
                    renderer.sprite = sprite;

                    audMan.PlaySingle(audWind);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        public void Clicked(int player)
        {
            if (!clicked)
            {
                clicked = true;
                cooldown.cooldown = time[renderer.sprite];
                cooldown.Pause(false);
                audMan.PlaySingle(audRing);
                StopAllCoroutines();
                pm.ec.PauseEnvironment(true);
            }
        }

        private void Update()
        {
            if (clicked && !audMan.AnyAudioIsPlaying && renderer.enabled)
                renderer.enabled = false;

            cooldown.UpdateCooldown(1f); //lol
        }

        public void OnCooldownEnd()
        {
            pm.ec.PauseEnvironment(false);
            Destroy(gameObject);
        }

        public void ClickableSighted(int player) { }
        public void ClickableUnsighted(int player) { }
        public bool ClickableHidden() { return clicked; }
        public bool ClickableRequiresNormalHeight() { return true; }

        public Entity entity;
        public SpriteRenderer renderer;
        public Sprite[] clockSprites = new Sprite[4];
        public Dictionary<Sprite, int> time = [];
        public bool clicked;
        public AudioManager audMan;
        public SoundObject audWind, audRing;
        public Cooldown cooldown = new Cooldown(1, 0, null, null, true);
    }
}

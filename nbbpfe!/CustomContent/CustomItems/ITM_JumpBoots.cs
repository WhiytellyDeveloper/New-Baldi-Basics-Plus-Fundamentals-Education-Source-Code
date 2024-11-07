using MidiPlayerTK;
using MTM101BaldAPI.Reflection;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_JumpBoots : Item, IItemPrefab
    {
        public void Setup()
        {
            var falseRenderer = new GameObject("JumpBootsFakeObject");
            falseRenderer.transform.SetParent(transform);
            falseRenderer.transform.localPosition = Vector3.zero;

            entity = gameObject.CreateEntity(1.5f, rendererBase: falseRenderer.transform);
            ((CapsuleCollider)entity.ReflectionGetVariable("collider")).height = 10f;
            gameObject.layer = LayerStorage.ignoreRaycast;

            rendererBase = falseRenderer.transform;

            boing = AssetsLoader.CreateSound("JumpBootsBoing", Paths.GetPath(PathsEnum.Items, "JumpBoots"), "Sfx_Boing", SoundType.Effect, Color.white, 1);
            groundHit = AssetsLoader.CreateSound("GroundHit", Paths.GetPath(PathsEnum.Items, "JumpBoots"), "Sfx_Thump", SoundType.Effect, Color.white, 1);
        }

        public override bool Use(PlayerManager pm)
        {
            ITM_JumpBoots[] boots = FindObjectsOfType<ITM_JumpBoots>();
            ITM_Umbrella[] umbrellas = FindObjectsOfType<ITM_Umbrella>();
            if (boots.Length != 1)
            {
                Destroy(gameObject);
                return false;
            }

            if (umbrellas.Length != 0)
            {
                Destroy(gameObject);
                return false;
            }

            this.pm = pm;
            entity.Initialize(pm.ec, pm.transform.position);
            transform.rotation = pm.cameraBase.rotation;
            Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).UpdateTargets(rendererBase, targetIdx);

            foreach (NPC npc in pm.ec.Npcs)
            {
                if (npc.Navigator.Entity == null)
                    npc.Navigator.Entity.IgnoreEntity(pm.plm.Entity, true);
            }

            StartCoroutine(AnimatedJump());
            return true;
        }

        private void Update()
        {
            transform.rotation = pm.cameraBase.rotation;
            cooldown.UpdateCooldown(pm.ec.EnvironmentTimeScale);

            if (Singleton<InputManager>.Instance.GetDigitalInput("Run", false) && pm.plm.stamina >= 0)
                pm.plm.stamina = Mathf.Max(pm.plm.stamina - pm.plm.staminaDrop * Time.deltaTime * pm.PlayerTimeScale, 0f);
        }

        private IEnumerator AnimatedJump()
        {
            while (true)
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(groundHit);

                if (cooldown.cooldownIsEnd)
                {
                    overrider.SetInteractionState(true);
                    overrider.SetFrozen(false);
                    overrider.Release();
                    Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).UpdateTargets(null, targetIdx);
                    foreach (NPC npc in pm.ec.Npcs)
                        npc.Navigator.Entity.IgnoreEntity(pm.plm.Entity, false);
                    Destroy(gameObject);
                    yield break;
                }

                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(boing);
                float acceleration = Singleton<InputManager>.Instance.GetDigitalInput("Run", false) && pm.plm.stamina >= 0 ? 28 : 18;
                Force force = new(pm.transform.forward, acceleration, -5.34f);
                entity.AddForce(force);
                pm.plm.Entity.Override(overrider);
                overrider.SetFrozen(true);
                overrider.SetInteractionState(false);

                float time = 0f;

                while (true)
                {
                    height = time.QuadraticEquation(-3f, 7f, 0f);
                    time += 2 * pm.ec.EnvironmentTimeScale * Time.deltaTime;

                    if (height > maxHeight)
                        height = maxHeight;

                    if ((transform.position - pm.transform.position).magnitude > 5f)
                    {
                        height = 0f;
                        break;
                    }

                    pm.Teleport(transform.position);

                    if (height < 0f)
                    {
                        height = 0f;
                        break;
                    }

                    entity.SetHeight(pm.plm.Entity.InternalHeight + height);

                    yield return null;
                }

                entity.RemoveForce(force);
                yield return new WaitForSeconds(0f);
            }
        }

        protected float height = 0f;
        protected int targetIdx = 15;
        protected const float maxHeight = 4.5f;
        protected readonly EntityOverrider overrider = new();
        [SerializeField]
        protected Entity entity;

        [SerializeField]
        protected internal Transform rendererBase;

        public Cooldown cooldown = new(25, 0);

        public SoundObject boing, groundHit;
    }
}

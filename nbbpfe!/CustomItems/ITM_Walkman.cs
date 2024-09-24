using MTM101BaldAPI.Reflection;
using nbbpfe.Enums;
using nbppfe.Extensions;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using nbbpfe.FundamentalsManager;

namespace nbppfe.CustomItems
{
    public class ITM_Walkman : Item, IItemPrefab
    {
        public void Setup()
        {
            Sprite walkmanSprite = CustomItemsEnum.Walkman.ToItem().itemSpriteLarge;
            walkmanRenderer = ObjectCreationExtensions.CreateSpriteBillboard(walkmanSprite, true);
            walkmanRenderer.transform.SetParent(transform);

            Vector3 spriteSize = walkmanRenderer.bounds.size;
            float radius = Mathf.Max(spriteSize.x, Mathf.Max(spriteSize.y, spriteSize.z)) / 2f;

            entity = gameObject.CreateEntity(radius, radius, walkmanRenderer.transform);
            gameObject.layer = LayerStorage.standardEntities;

            audMan = gameObject.CreatePropagatedAudioManager(100, 300);

            var tape = Resources.FindObjectsOfTypeAll<TapePlayer>().First();
            antiHearingSound = (SoundObject)tape.ReflectionGetVariable("beep");
            inseretSound = (SoundObject)tape.ReflectionGetVariable("audInsert");
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            ec = pm.ec;
            transform.position = pm.transform.position;
            direction = pm.GetPlayerCamera().transform.forward;
            coolown.endAction = Destroy;
            entity.Initialize(ec, transform.position);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(FundamentalLoaderManager.GenericThrowSound);
            entity.OnEntityMoveInitialCollision += (hit) =>
            {
                if (!reflected)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                    StartCoroutine(ThrowAnim());
                    reflected = true;
                }
            };

            return true;
        }

        private void Update()
        {
            if (active)
            {
                coolown.UpdateCooldown(ec.EnvironmentTimeScale);
                if (ec.GetBaldi() != null)
                    ec.GetBaldi().Distract();
            }

            entity.UpdateInternalMovement(direction * 42 * ec.EnvironmentTimeScale);
        }

        private IEnumerator ThrowAnim()
        {
            inAnimation = true;
            entity.SetFrozen(true);
            float fallSpeed = 10f;
            bool falling = true;

            while (falling)
            {
                fallSpeed -= ec.EnvironmentTimeScale * Time.deltaTime * 28f;
                walkmanRenderer.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;

                if (walkmanRenderer.transform.localPosition.y <= fallLimit)
                {
                    walkmanRenderer.transform.localPosition = new Vector3(walkmanRenderer.transform.localPosition.x, -6, walkmanRenderer.transform.localPosition.z);
                    falling = false;
                }

                yield return null;
            }

            Vector3 newPos = ec.CellFromPosition(transform.position).FloorWorldPosition;
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);

            while (!falling)
            {
                fallSpeed += ec.EnvironmentTimeScale * Time.deltaTime * 48f;
                walkmanRenderer.transform.localPosition += Vector3.up * fallSpeed * Time.deltaTime;

                if (walkmanRenderer.transform.localPosition.y >= 0)
                {
                    walkmanRenderer.transform.localPosition = Vector3.up * 0;
                    break;
                }

                yield return null;
            }
            Active();

            while (true)
            {
                Vector3 originalPos = walkmanRenderer.transform.localPosition;
                while (true)
                {
                    float shakeAmount = 0.2f;
                    walkmanRenderer.transform.localPosition = originalPos + new Vector3(
                        Random.Range(-shakeAmount, shakeAmount),
                        Random.Range(-shakeAmount, shakeAmount),
                        Random.Range(-shakeAmount, shakeAmount)
                    );

                    yield return null;
                }
            }
        }

        public void Active()
        {
            dijkstraMap = new DijkstraMap(ec, PathType.Const, [transform]);
            dijkstraMap.QueueUpdate();
            dijkstraMap.Activate();
            ec.MakeSilent(25);
            audMan.PlaySingle(inseretSound);
            audMan.PlaySingle(antiHearingSound);
            StartCoroutine(Flee());
            active = true;
        }

        private IEnumerator Flee()
        {
            while (dijkstraMap.PendingUpdate)
                yield return null;
            using (List<NPC>.Enumerator enumerator = ec.Npcs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    NPC npc = enumerator.Current;
                    if (npc.Navigator.enabled)
                    {
                        NavigationState_WanderFleeOverride navigationState_WanderFleeOverride = new NavigationState_WanderFleeOverride(npc, 31, dijkstraMap);
                        fleeStates.Add(navigationState_WanderFleeOverride);
                        npc.navigationStateMachine.ChangeState(navigationState_WanderFleeOverride);
                    }
                }
                goto IL_D1;
            }
        IL_BA:
            yield return null;
        IL_D1:
            if (!audMan.QueuedAudioIsPlaying)
            {
                foreach (NavigationState_WanderFleeOverride navigationState_WanderFleeOverride2 in fleeStates) {
                    navigationState_WanderFleeOverride2.End();
                }
                dijkstraMap.Deactivate();
                fleeStates.Clear();
                yield break;
            }
            goto IL_BA;
        }



        public void Destroy() =>
            Destroy(base.gameObject);

        public Entity entity;
        protected Cooldown coolown = new Cooldown(27, 0);
        protected EnvironmentController ec;
        public SoundObject antiHearingSound, inseretSound;
        public AudioManager audMan;
        public Vector3 direction;
        public bool inAnimation, active, reflected;
        public float fallLimit = -5f;
        public SpriteRenderer walkmanRenderer;
        private List<NavigationState_WanderFleeOverride> fleeStates = new List<NavigationState_WanderFleeOverride>();
        private DijkstraMap dijkstraMap;
    }
}

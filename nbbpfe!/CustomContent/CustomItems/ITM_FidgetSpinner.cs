using MTM101BaldAPI;
using nbppfe.Extensions;
using nbppfe.FundamentalsManager;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_FidgetSpinner : Item, IItemPrefab, IEntityTrigger
    {
        public void Setup() 
        {
            fidgetRenderer = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("Fidget_Drop", Paths.GetPath(PathsEnum.Items, "FidgetSpinner"), 15), false).AddSpriteHolder(out var renderer, -5 + 0.01f).renderers[0].GetComponent<SpriteRenderer>();
            var holder = fidgetRenderer.transform.parent;
            gameObject.AddBoxCollider(Vector3.zero, new Vector3(8, 10, 8), true);
            holder.gameObject.ConvertToPrefab(true);
            holder.transform.SetParent(transform);
            fidgetRenderer.transform.SetParent(holder.transform);
            fidgetRenderer.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            float spinnerSize = fidgetRenderer.GetSpriteSize() / 2.4f;
            entity = gameObject.CreateEntity(spinnerSize, spinnerSize);

            audMan = gameObject.CreatePropagatedAudioManager(60, 140);
            spinningSound = AssetsLoader.CreateSound("FidgetSpinnerSpinning", Paths.GetPath(PathsEnum.Items, "FidgetSpinner"), "Sfx_Spinning", SoundType.Effect, Color.white, 1);
        }


        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            transform.position = pm.ec.CellFromPosition(pm.transform.position).FloorWorldPosition;
            audMan.SetLoop(true);
            audMan.QueueAudio(spinningSound, true);
            direction = pm.GetCustomCam().transform.forward;
            entity.Initialize(pm.ec, transform.position);
            coolodwn.endAction = OnCooldownEnd;

            entity.OnEntityMoveInitialCollision += (hit) =>
            {
                direction = Vector3.Reflect(direction, hit.normal);

                if (target != null && speed <= 65)
                    speed += 7;
            };

            return true;
        }


        private void Update() 
        {
            //Pixel guy stolen part start here
            rotation.x = 90f;
            rotation.y += 17.5f * speed * pm.ec.EnvironmentTimeScale;
            rotation.y %= 360f;
            fidgetRenderer.transform.eulerAngles = rotation;
            //Pixel guy stolen part end here

            entity.UpdateInternalMovement(direction * speed * pm.ec.EnvironmentTimeScale);

            if (target != null)
            {
                target.Teleport(transform.position);
                coolodwn.UpdateCooldown(pm.ec.EnvironmentTimeScale);
            }
        }

        public void EntityTriggerEnter(Collider other)
        {
            if (other.CompareTag("NPC") && target == null || other.CompareTag("Player") && target == null && playerExist)
            {
                var npc = other.GetComponent<Entity>();
                npc.GetComponent<ActivityModifier>().moveMods.Add(movMod);
                target = npc;
            }
        }

        public void EntityTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && target == null)
                playerExist = true;       
        }

        public void EntityTriggerStay(Collider other) { }

        public void OnCooldownEnd()
        {
            target.GetComponent<ActivityModifier>().moveMods.Remove(movMod);
            target.Teleport(pm.ec.CellFromPosition(transform.position).FloorWorldPosition);
            target = null;
            Destroy(gameObject);
        }

        public SpriteRenderer fidgetRenderer;
        private Vector3 rotation;
        public int speed = 42;
        public Entity entity;
        public Vector3 direction;
        public Entity target;
        public MovementModifier movMod = new (Vector3.zero, 0);
        public Cooldown coolodwn = new Cooldown(33, 0);
        public AudioManager audMan;
        public SoundObject spinningSound;
        public bool playerExist;
    }
}

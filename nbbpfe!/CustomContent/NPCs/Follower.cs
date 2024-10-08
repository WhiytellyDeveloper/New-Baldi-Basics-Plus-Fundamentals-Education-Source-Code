using nbppfe.Extensions;
using nbppfe.PrefabSystem;
using UnityEngine;
using nbbpfe.FundamentalsManager;
using System.Collections;

namespace nbppfe.CustomContent.NPCs
{
    public class Follower : NPC, INPCPrefab
    {
        public void Setup()
        {
            Poster.textData[0].color = Color.white;
            navigator.Entity.SetGrounded(false);
        }

        public void PostLoading()
        {
            fog = new Fog
            {
                priority = 0,
                color = AssetsLoader.SetHexaColor("#404049"),
                strength = 0f,
                startDist = 0,
                maxDist = 75,
            };
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new Follower_Wandering(this));
        }

        public IEnumerator FadeOnFog()
        {
            ec.AddFog(fog);
            fog.strength = 0f;
            float duration = 0;
            float increment = 1f / 120f;

            while (duration < 120f)
            {
                duration++;
                fog.strength += increment;
                ec.UpdateFog();
                yield return new WaitForSeconds(1f);
            }

            fog.strength = 1f;
            ec.UpdateFog();
        }

        public Fog fog;
    }

    public class Follower_StateBase : NpcState {
        protected Follower fol;
        public Follower_StateBase(Follower follower) : base(follower) { fol = follower; }      
    }

    public class Follower_Wandering : Follower_StateBase
    {
        public Follower_Wandering(Follower follower) : base(follower) { }

        public override void Initialize()
        {
            base.Initialize();
            fol.Navigator.SetSpeed(8, 8);
            ChangeNavigationState(new NavigationState_WanderRandom(fol, 0));
        }

        public override void PlayerInSight(PlayerManager player)
        {
            base.PlayerInSight(player);
            fol.behaviorStateMachine.ChangeState(new Follower_Sighted(fol, player));
        }
    }

    public class Follower_Sighted : Follower_StateBase
    {
        private PlayerManager pm;

        public Follower_Sighted(Follower follower, PlayerManager player) : base(follower)
        {
            pm = player;
        }

        public override void Initialize()
        {
            base.Initialize();
            fol.Navigator.SetSpeed(8, 15);
        }

        public override void Update()
        {
            base.Update();
            ChangeNavigationState(new NavigationState_TargetPlayer(fol, 100, pm.transform.position));
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == pm.gameObject)
                fol.behaviorStateMachine.ChangeState(new Follower_Following(fol, pm));         
        }
    }

    public class Follower_Following : Follower_StateBase
    {
        private PlayerManager pm;

        public Follower_Following(Follower follower, PlayerManager player) : base(follower)
        {
            pm = player;
        }

        public override void Initialize()
        {
            base.Initialize();
            fol.Navigator.SetSpeed(8, 16);
            fol.StartCoroutine(fol.FadeOnFog());
        }

        public override void Update()
        {
            base.Update();
            ChangeNavigationState(new NavigationState_TargetPlayer(fol, int.MaxValue, pm.transform.position));

            if (fol.transform.position.GetDistanceFrom(pm.transform.position) >= 100)
            {
                fol.ec.RemoveFog(fol.fog);
                fol.behaviorStateMachine.ChangeState(new Follower_Wandering(fol));
            }
        }

        public override void OnStateTriggerEnter(Collider other)
        {
            base.OnStateTriggerEnter(other);

            if (other.gameObject == pm.gameObject)   
                fol.Navigator.Entity.SetFrozen(true);         
        }

        public override void OnStateTriggerExit(Collider other)
        {
            base.OnStateTriggerExit(other);

            if (other.gameObject == pm.gameObject)
                fol.Navigator.Entity.SetFrozen(false);        
        }
    }
}

using nbppfe.Enums;
using nbppfe.FundamentalsManager.Loaders;
using nbppfe.FundamentalSystems;
using nbppfe.PrefabSystem;
using PixelInternalAPI.Classes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbppfe.CustomContent.NPCs
{
    public class HandleKlap : NPC, INPCPrefab, IClickable<int>
    {
        public void Setup() 
        {
            sprites = NPCLoader.spritesFormSpritesheet[CustomNPCsEnum.HandleKlap];
            voicelines = NPCLoader.soundsObjects[CustomNPCsEnum.HandleKlap].ToList();
            voicelines.RemoveAt(1);
            audClap = NPCLoader.soundsObjects[CustomNPCsEnum.HandleKlap].Skip(1).Take(1).First();
            audMan = gameObject.GetComponent<AudioManager>();
            gameObject.layer = LayerStorage.iClickableLayer;
        }

        public void PostLoading() =>
            idleBack.endAction = BackToIdleSprite;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new HandleKlap_Wandering(this));
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();

            cooldown.UpdateCooldown(TimeScale);
            idleBack.UpdateCooldown(TimeScale);
        }

        public void OnEndCooldown() =>
            cooldown.Restart();

        public void SetSprite(int id) =>
            spriteRenderer[0].sprite = sprites[id];

        public void Clap()
        {
            bool inTime = !clapCooldown.cooldownIsEnd;
            if (clapWait)
            {
                clapWait = false;

                switch (inTime)
                {
                    case true:
                        audMan.PlaySingle(audClap);
                        audMan.QueueAudio(voicelines[1]);
                        SetSprite(3);
                        Singleton<CoreGameManager>.Instance.AddPoints(10, lastPlayerClicked, true);
                        break;
                    case false:
                        audMan.QueueAudio(voicelines[2]);
                        SetSprite(4);
                        break;
                }
                idleBack.Restart();
                idleBack.Pause(false);
                behaviorStateMachine.ChangeState(new HandleKlap_Wandering(this));
                clapCooldown.Restart();
                cooldown.Restart();
            }
        }

        public void BackToIdleSprite() =>
            SetSprite(0);

        public void Clicked(int player) { lastPlayerClicked = player; Clap(); }
        public void ClickableSighted(int player) { if (!ClickableHidden()) SetSprite(2); }
        public void ClickableUnsighted(int player) { if (!ClickableHidden()) SetSprite(1); }
        public bool ClickableHidden() { return !clapWait; }
        public bool ClickableRequiresNormalHeight() { return true; }

        public Cooldown cooldown = new(20, 0), clapCooldown = new(2, 0), idleBack = new(2, 0, null, null, true);
        public AudioManager audMan;
        public List<SoundObject> voicelines;
        public SoundObject audClap;
        public Sprite[] sprites;
        public bool clapWait;
        public int lastPlayerClicked;
    }

    public class HandleKlap_StateBase : NpcState {
        protected HandleKlap hk;
        public HandleKlap_StateBase(HandleKlap klap) : base(klap) { npc = klap; this.hk = klap; }
    }

    public class HandleKlap_Wandering : HandleKlap_StateBase
    {
        public HandleKlap_Wandering(HandleKlap klap) : base(klap) { }

        public override void Initialize()
        {
            base.Initialize();
            npc.Navigator.SetSpeed(14);
            hk.clapWait = false;
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }

        public override void InPlayerSight(PlayerManager player)
        {
            base.InPlayerSight(player);

            if (hk.cooldown.cooldownIsEnd)
                npc.behaviorStateMachine.ChangeState(new HandleKlap_ClapWait(hk));
        }
    }

    public class HandleKlap_ClapWait : HandleKlap_StateBase
    {
        public HandleKlap_ClapWait(HandleKlap klap) : base(klap) { }

        public override void Initialize()
        {
            base.Initialize();
            hk.clapWait = true;
            hk.audMan.QueueAudio(hk.voicelines[0]);
            hk.clapCooldown.endAction = hk.Clap;
            ChangeNavigationState(new NavigationState_DoNothing(npc, 0));
            hk.SetSprite(1);
        }

        public override void Update()
        {
            base.Update();
            hk.clapCooldown.UpdateCooldown(hk.TimeScale);
        }
    }
}

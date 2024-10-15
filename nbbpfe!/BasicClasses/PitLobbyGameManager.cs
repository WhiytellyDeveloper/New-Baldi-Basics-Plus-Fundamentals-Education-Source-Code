using nbbpfe.FundamentalsManager;
using nbppfe.BasicClasses.CustomObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbppfe.BasicClasses
{
    public class PitLobbyGameManager : BaseGameManager
    {
        public List<TipBook> books = [];

        public override void Initialize()
        {
            base.Initialize();
            Singleton<CoreGameManager>.Instance.GetHud(0).SetNotebookDisplay(false);
            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.Teleport(new Vector3(65, 5, 20));
            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.transform.localRotation = Direction.North.ToRotation();

            foreach (Pickup pickup in ec.items.Skip(1))
            {
                if (books.Count == 0)
                {
                    //TipBook book = Instantiate<TipBook>(AssetsLoader.Get<TipBook>("TipBook_0"), transform);
                    //books.Add(book);
                    //book.transform.position = pickup.transform.position;
                }
                pickup.gameObject.SetActive(false);
            }

        }

        public override void BeginPlay()
        {
            base.BeginPlay();
        }

        protected override void VirtualUpdate()
        {
            base.VirtualUpdate();

            if (Singleton<CoreGameManager>.Instance.GetPlayer(0) != null)
                Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.AddStamina(100f, true);

            if (ec.elevators.Count > 0)
            {
                for (int j = 0; j < ec.elevators.Count; j++)
                {
                    if (ec.elevators[j].IsSpawn)
                    {
                        if (!ec.elevators[j].Door.IsOpen)
                            ec.elevators[j].Door.Open(true, false);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log($"{ec.CellFromPosition(Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position).position.x}, {ec.CellFromPosition(Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position).position.z}");
            }
        }


        public override void LoadNextLevel()
        {
            base.PrepareToLoad();
            if (Singleton<ElevatorScreen>.Instance != null)
            {
                elevatorScreen = Singleton<ElevatorScreen>.Instance;
                elevatorScreen.Reinit();
                elevatorScreen.OnLoadReady += base.LoadNextLevel;
                return;
            }
            elevatorScreen = Object.Instantiate<ElevatorScreen>(elevatorScreenPre);
            elevatorScreen.OnLoadReady += base.LoadNextLevel;
            elevatorScreen.Initialize();

        }

        protected override void LoadSceneObject(SceneObject sceneObject, bool restarting)
        {
            base.LoadSceneObject(sceneObject, restarting);
            Destroy(gameObject);
        }
    }
}

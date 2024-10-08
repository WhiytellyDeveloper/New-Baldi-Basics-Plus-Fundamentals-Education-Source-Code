
using UnityEngine;

namespace nbppfe.FundamentalsManager
{
    public class FundamentalsGameController : Singleton<FundamentalsGameController>
    {

        public void AddNotebooks()
        {

            /*if (active)
            {
                notebookTotal = noteboooks;
                active = true;
            }
            Singleton<BaseGameManager>.Instance.CollectNotebooks(noteboooks);
            Singleton<BaseGameManager>.Instance.AddNotebookTotal(noteboooks);
            Singleton<CoreGameManager>.Instance.GetHud(0).UpdateNotebookText(0, Singleton<BaseGameManager>.Instance.FoundNotebooks.ToString() + "/" + Mathf.Max(Singleton<BaseGameManager>.Instance.Ec.notebookTotal, Singleton<BaseGameManager>.Instance.FoundNotebooks).ToString(), false);
            noteboooks = 0;
            */
        }

        public void OnExit() {
            noteboooks = 0;
            notebookTotal = 0;
        }

        public void OnReloadLevel() {
            active = false;
        }

        public int noteboooks, notebookTotal;
        public bool active = true;
    }
}

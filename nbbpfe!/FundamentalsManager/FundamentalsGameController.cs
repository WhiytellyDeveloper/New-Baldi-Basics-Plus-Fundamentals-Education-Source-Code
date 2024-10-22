using nbppfe.Extensions;
using nbppfe.FundamentalsManager.Loaders;
using System.Collections;
using System.Linq;
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

        public void OnExit()
        {
            noteboooks = 0;
            notebookTotal = 0;
        }

        public void OnReloadLevel()
        {
            active = false;
        }

        public void OnEventOver() =>
            StartCoroutine(EventTimer(3));


        private IEnumerator EventTimer(float time)
        {
            while (time > 3f)
            {
                time -= Time.deltaTime * Singleton<BaseGameManager>.Instance.Ec.EnvironmentTimeScale;
                yield return null;
            }
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(audEventNotification);
            Singleton<CoreGameManager>.Instance.GetHud(0).BaldiTv.AnnounceEvent(EventsLoader.eventEndVoicelines.CatchRandomItem());
        }


        public int noteboooks, notebookTotal;
        public bool active = true;
        public SoundObject audEventNotification = Resources.FindObjectsOfTypeAll<SoundObject>().Where(x => x.name == "mus_EventNotification_Low").FirstOrDefault();
    }
}

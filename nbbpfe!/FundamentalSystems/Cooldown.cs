using System;
using UnityEngine;

namespace nbppfe.FundamentalSystems
{
    public class Cooldown
    {
        public float startCooldown = 60;
        public float cooldown = 60;
        public float endCooldown = 0;
        public Action endAction;
        public Action restartAction;
        private bool end;

        public bool cooldownIsEnd
        {
            get
            {
                return end;
            }
        }

        private bool paused;

        public bool isPaused
        {
            get
            {
                return paused;
            }
        }

        public Cooldown(float startTime, float endTime, Action action = null, Action restartAct = null, bool startPaused = false, bool startIn0 = false)
        {
            if (startPaused)
                Pause(true);

            cooldown = startTime;
            startCooldown = startTime;
            if (startIn0)
                cooldown = 0.1f;
            endCooldown = endTime;
            endAction = action;
            restartAction = restartAct;
        }

        public void UpdateCooldown(float timeScale)
        {
            if (!paused && !end)
            {
                cooldown -= Time.deltaTime * timeScale;

                if (cooldown <= endCooldown)
                {
                    cooldown = endCooldown;
                    if (!end)
                    {
                        end = true;
                        endAction?.Invoke();
                    }
                }
            }
        }

        public void Restart()
        {
            cooldown = startCooldown;
            end = false;
            restartAction?.Invoke();
        }

        public void Pause(bool value) =>
            paused = value;
    }
}

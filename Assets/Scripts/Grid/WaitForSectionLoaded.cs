using UnityEngine;

namespace FarmSim.Grid
{
    public class WaitForSectionLoaded : CustomYieldInstruction
    {
        private readonly NodeGrid nodeGrid;
        private readonly float timeout;
        private readonly float startTime;
        private bool timedOut;

        public bool TimedOut => timedOut;

        public override bool keepWaiting
        {
            get
            {
                if (Time.realtimeSinceStartup - startTime >= timeout)
                {
                    timedOut = true;
                }

                return !nodeGrid.LoadedSection && !timedOut;
            }
        }

        public WaitForSectionLoaded(NodeGrid _nodeGrid, float _timeout = 10)
        {
            nodeGrid = _nodeGrid;
            timeout = _timeout;
            startTime = Time.realtimeSinceStartup;
        }
    }
}
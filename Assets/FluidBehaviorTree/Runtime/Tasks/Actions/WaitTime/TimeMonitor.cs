using UnityEngine;

namespace FluidBehaviorTree.Runtime.Tasks.Actions.WaitTime
{
    public class TimeMonitor : ITimeMonitor
    {
#region ITimeMonitor Implementation

        public float DeltaTime => Time.deltaTime;

#endregion
    }
}

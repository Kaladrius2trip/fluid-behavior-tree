﻿namespace FluidBehaviorTree.Runtime.Tasks.Actions
{
    public abstract class ActionBase : TaskBase
    {
        protected abstract TaskStatus OnUpdate();

        protected override TaskStatus GetUpdate()
        {
            return OnUpdate();
        }
    }
}

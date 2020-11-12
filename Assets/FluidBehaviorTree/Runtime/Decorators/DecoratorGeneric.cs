using System;

using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class DecoratorGeneric : DecoratorBase
    {
        public Func<ITask, TaskStatus> updateLogic;

        protected override TaskStatus OnUpdate()
        {
            if (updateLogic != null)
            {
                return updateLogic(Child);
            }

            return Child.Update();
        }
    }
}

using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class RepeatUntilFailure : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/EventBusy.png";

        protected override TaskStatus OnUpdate()
        {
            if (Child.Update() == TaskStatus.Failure)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Process;
        }
    }
}

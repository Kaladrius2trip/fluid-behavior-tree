using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class RepeatUntilSuccess : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/EventAvailable.png";

        protected override TaskStatus OnUpdate()
        {
            if (Child.Update() == TaskStatus.Success)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Process;
        }
    }
}

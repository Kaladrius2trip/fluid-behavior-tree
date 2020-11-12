using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class ReturnFailure : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Cancel.png";

        protected override TaskStatus OnUpdate()
        {
            TaskStatus status = Child.Update();
            if (status == TaskStatus.Process)
            {
                return status;
            }

            return TaskStatus.Failure;
        }
    }
}

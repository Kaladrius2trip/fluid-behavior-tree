using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class ReturnSuccess : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Checkmark.png";

        protected override TaskStatus OnUpdate()
        {
            TaskStatus status = Child.Update();
            if (status == TaskStatus.Process)
            {
                return status;
            }

            return TaskStatus.Success;
        }
    }
}

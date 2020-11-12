using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class Inverter : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Invert.png";

        protected override TaskStatus OnUpdate()
        {
            TaskStatus childStatus = Child.Update();
            TaskStatus status = childStatus;

            switch (childStatus)
            {
                case TaskStatus.Success:
                    status = TaskStatus.Failure;
                    break;
                case TaskStatus.Failure:
                    status = TaskStatus.Success;
                    break;
            }

            return status;
        }
    }
}

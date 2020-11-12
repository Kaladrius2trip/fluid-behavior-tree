using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public class RepeatForever : DecoratorBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Repeat.png";

        protected override TaskStatus OnUpdate()
        {
            Child.Update();
            return TaskStatus.Process;
        }
    }
}

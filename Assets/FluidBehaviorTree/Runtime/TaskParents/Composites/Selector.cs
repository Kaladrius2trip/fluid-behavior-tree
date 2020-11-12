using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents.Composites
{
    public class Selector : CompositeBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/LinearScale.png";

        protected override TaskStatus OnUpdate()
        {
            for (int i = ChildIndex; i < Children.Count; i++)
            {
                ITask child = Children[ChildIndex];

                switch (child.Update())
                {
                    case TaskStatus.Success:
                        return TaskStatus.Success;
                    case TaskStatus.Process:
                        return TaskStatus.Process;
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }
    }
}

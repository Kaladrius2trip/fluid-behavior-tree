using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents.Composites
{
    public class Sequence : CompositeBase
    {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/RightArrow.png";

        protected override TaskStatus OnUpdate()
        {
            for (int i = ChildIndex; i < Children.Count; i++)
            {
                ITask child = Children[ChildIndex];

                TaskStatus status = child.Update();
                if (status != TaskStatus.Success)
                {
                    return status;
                }

                ChildIndex++;
            }

            return TaskStatus.Success;
        }
    }
}

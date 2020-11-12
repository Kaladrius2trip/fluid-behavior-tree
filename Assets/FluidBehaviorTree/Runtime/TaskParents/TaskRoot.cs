using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents
{
    public class TaskRoot : TaskParentBase
    {
        public override string Name { get; set; } = "Root";
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/DownArrow.png";
        protected override int MaxChildren { get; } = 1;

        public override void End() { }

        protected override TaskStatus OnUpdate()
        {
            if (Children.Count == 0)
            {
                return TaskStatus.Success;
            }

            ITask child = Children[0];
            return child.Update();
        }
    }
}

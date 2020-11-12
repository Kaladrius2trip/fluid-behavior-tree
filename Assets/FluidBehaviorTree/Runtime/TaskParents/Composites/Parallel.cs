using System.Collections.Generic;

using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents.Composites
{
    public class Parallel : CompositeBase
    {
        private readonly Dictionary<ITask, TaskStatus> _childStatus = new Dictionary<ITask, TaskStatus>();

        public override string IconPath { get; } = $"{PACKAGE_ROOT}/CompareArrows.png";

        public override void Reset()
        {
            _childStatus.Clear();

            base.Reset();
        }

        public override void End()
        {
            foreach (ITask child in Children)
            {
                child.End();
            }
        }

        protected override TaskStatus OnUpdate()
        {
            var successCount = 0;
            var failureCount = 0;

            foreach (ITask child in Children)
            {
                if (_childStatus.TryGetValue(child, out TaskStatus prevStatus) && prevStatus == TaskStatus.Success)
                {
                    successCount++;
                    continue;
                }

                TaskStatus status = child.Update();
                _childStatus[child] = status;

                switch (status)
                {
                    case TaskStatus.Failure:
                        failureCount++;
                        break;
                    case TaskStatus.Success:
                        successCount++;
                        break;
                }
            }

            if (successCount == Children.Count)
            {
                End();
                return TaskStatus.Success;
            }

            if (failureCount > 0)
            {
                End();
                return TaskStatus.Failure;
            }

            return TaskStatus.Process;
        }
    }
}

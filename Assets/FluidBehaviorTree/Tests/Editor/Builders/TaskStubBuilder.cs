using System.Collections.Generic;

using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

using NSubstitute;

namespace FluidBehaviorTree.Tests.Editor.Builders
{
    public class TaskStubBuilder
    {
        private bool _enabled = true;
        private TaskStatus _status = TaskStatus.Success;
        private List<ITask> _children;

        public TaskStubBuilder WithEnabled(bool enabled)
        {
            _enabled = enabled;

            return this;
        }

        public TaskStubBuilder WithUpdateStatus(TaskStatus status)
        {
            _status = status;

            return this;
        }

        public TaskStubBuilder SetChildren(List<ITask> children)
        {
            _children = children;
            return this;
        }

        public ITask Build()
        {
            var task = Substitute.For<ITaskComposite>();
            task.Enabled.Returns(_enabled);
            task.Update().ReturnsForAnyArgs(_status);
            task.Children.Returns(_children);

            return task;
        }
    }
}

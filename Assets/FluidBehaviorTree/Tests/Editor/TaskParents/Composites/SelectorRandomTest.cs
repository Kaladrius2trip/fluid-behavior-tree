using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.TaskParents.Composites
{
    public class SelectorRandomTest
    {
        public class UpdateMethod
        {
            [Test]
            public void It_should_return_success_if_a_child_returns_success()
            {
                ITask child = A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build();
                ITaskComposite selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);

                Assert.AreEqual(TaskStatus.Success, selectorRandom.Update());
            }

            [Test]
            public void It_should_return_continue_if_a_child_returns_continue()
            {
                ITask child = A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build();
                ITaskComposite selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);

                Assert.AreEqual(TaskStatus.Process, selectorRandom.Update());
            }

            [Test]
            public void It_should_return_failure_if_empty()
            {
                ITaskComposite selectorRandom = new SelectorRandom();

                Assert.AreEqual(TaskStatus.Failure, selectorRandom.Update());
            }

            [Test]
            public void It_should_return_failure_if_child_returns_failure()
            {
                ITask child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                ITaskComposite selectorRandom = new SelectorRandom();
                selectorRandom.AddChild(child);

                Assert.AreEqual(TaskStatus.Failure, selectorRandom.Update());
            }
        }
    }
}

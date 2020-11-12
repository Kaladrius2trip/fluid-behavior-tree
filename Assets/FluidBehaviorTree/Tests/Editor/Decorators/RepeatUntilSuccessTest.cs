using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class RepeatUntilSuccessTest
    {
        public class UpdateMethod
        {
            private ITaskComposite repeater;

            [SetUp]
            public void Setup_repeater()
            {
                repeater = new RepeatUntilSuccess();
            }

            [Test]
            public void Returns_continue_on_child_failure()
            {
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Process, repeater.Update());
            }

            [Test]
            public void Returns_success_on_child_success()
            {
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build());

                Assert.AreEqual(TaskStatus.Success, repeater.Update());
            }

            [Test]
            public void Returns_continue_on_child_continue()
            {
                repeater.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build());

                Assert.AreEqual(TaskStatus.Process, repeater.Update());
            }
        }
    }
}
